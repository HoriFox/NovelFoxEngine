using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ng
{
    public class CXmlLoader : MonoBehaviour
    {
        public Transform sceneParent;
        private Transform m_canvastr;
        private Scene m_scene;
        private XmlNode m_currentNode;
        private XmlDocument m_xmlDoc;
        private XmlElement m_xmlRoot;
        TimerPrefabScript timer;
        Kernel kernel;
        // Id выбранного объекта. 
        public string nameChoiceSelected;
        // Объект выбран
        public bool isObjectSelected;
        // Эта нода с Choice.
        private bool m_nodeChoice;
        // Пропустить ли загрузку сцены с обработкой логики.
        private bool m_onlyLogicLoadinge;
        // EVENT IF выполняется положительно.
        private bool m_eventIfSuitable;

        // Словарь с объектами выделения
        Dictionary<string, ObjectScript> selectionObject;

        //Загрузка и настройка xml файла.
        public void ReadXMLFile(string nameFile)
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreComments = true
            };
            XmlReader reader = XmlReader.Create(Constants.SctiptPath + nameFile + ".xml", readerSettings);
            m_xmlDoc = new XmlDocument();
            m_xmlDoc.Load(reader);
            m_xmlRoot = m_xmlDoc.DocumentElement;

            m_onlyLogicLoadinge = false;
            m_nodeChoice = false;
            isObjectSelected = false;
            nameChoiceSelected = null;
            m_eventIfSuitable = true;

            m_currentNode = m_xmlRoot.FirstChild;   // Быстрая логика.
            UpdateScene();                          // Обновление сцены.
            CheckBlock();
        }
        public void Awake()
        {
            timer = GameObject.Find("NGTimer").GetComponent<TimerPrefabScript>();
            kernel = GameObject.Find("NGGame").GetComponent<Kernel>();

            selectionObject = new Dictionary<string, ObjectScript>();
        }
        // Главное обновление.
        public void Update()
        {
            EventController();
        }
        // Обновление сцены.
        public void UpdateScene()
        {
            if (m_currentNode != null)
            {
                ReloadScene();
                LoadNode(m_currentNode);
            }
        }
        // Пересоздание сцены.
        public void ReloadScene()
        {
            if (m_currentNode.Name == "SCENE")
            {
                m_scene = new Scene();
                m_canvastr = sceneParent.GetComponentInChildren<Canvas>().gameObject.transform;
                foreach (Transform t in m_canvastr)
                {
                    Destroy(t.gameObject);
                }
                string line = "============================================================================";
                Debug.Log("<b>" + line + " Пересоздаём Scene [" + m_currentNode.Attributes["id"].Value + "]</b>");
            }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~ Логика переключений ~~~~~~~~~~~~~~~~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Логика переходов по сценарию.
        public void LogicTransitions()
        {
            if (m_currentNode != null)
            {
                if (m_currentNode.SelectSingleNode("EVENT") != null)         
                {
                    m_currentNode = m_currentNode.SelectSingleNode("EVENT");
                }
                else                                                        
                {
                    if (m_currentNode.NextSibling != null)                   
                    {
                        m_currentNode = m_currentNode.NextSibling;
                    }
                    else                                                    
                    {
                        if (m_currentNode.Name == "EVENT")                    
                        {
                            m_currentNode = m_currentNode.ParentNode;
                            if (m_currentNode.NextSibling != null)
                            {
                                m_currentNode = m_currentNode.NextSibling;
                            }
                        }
                        else                                               
                        {
                            m_currentNode = null;
                            Debug.Log("Конец сценария");
                        }
                    }
                }
                if (m_currentNode != null)
                {
                    CheckBlock();
                }
            }
        }
        // Устанавливаем на изначальные положения триггеры.
        public void ResetCheckBlock()
        {
            m_nodeChoice = false;
            m_onlyLogicLoadinge = false;
            isObjectSelected = false;
            // Обнуляем Id объекта заселекченного в прошлый раз.
            nameChoiceSelected = null;
            m_eventIfSuitable = true;
        }
        // Блок проверок на необычные ТЭГИ.
        public void CheckBlock()
        {
            ResetCheckBlock();

            // Проверка и установка IF расширения на EVENT.
            bool boolEventIf = IsEventIF(m_currentNode);
            if (boolEventIf)
            {
                AddEventIF(m_currentNode);
            }
            // Если EVENT IF выполняется положительно, то продолжаем обрабатывать.
            if (m_eventIfSuitable)
            {
                // Обновим пакет объектов, возможно, что нужные CHOICE объекты в этом EVENT.
                if (m_currentNode.Name == "EVENT")
                {
                    LoadNode(m_currentNode); // Загружает содержимое целых два раза. Ну нафиг. TO DO
                }
                // Проверка ноды на наличие тэга VAR.
                bool boolVar = SearchVar();
                if (boolVar)
                {
                    // Добавляем все существующие VAR-ы.
                    AddVars(m_currentNode);
                }
                // Проверка и установка CHOICE расширения на EVENT.
                bool boolEventChoice = SearchChoice();
                if (boolEventChoice)
                {
                    m_onlyLogicLoadinge = true;
                    m_nodeChoice = true;
                    AddChoice(m_currentNode.SelectSingleNode("CHOICE"));
                }
                // Проверка и установка TIME расширения на EVENT.
                bool boolEventTime = IsEventTime(m_currentNode);
                if (boolEventTime)
                {
                    AddEventTime(m_currentNode);
                }
                // Проверка ноды на наличие тэга JUMP.
                bool boolJump = SearchJump();
                if (boolJump)
                {
                    AddJump(m_currentNode.SelectSingleNode("JUMP"));
                }
            }
        }
        // Контроль логики переключений.
        public void EventController()
        {
            // Для проверки скорости проработки скрипта - отключить два уровня условий.
            if ((m_nodeChoice == true && isObjectSelected == true) || (m_nodeChoice == false && isObjectSelected == false))
            {
                if (OnInputEvent() == 1 || timer.status == 1 || m_onlyLogicLoadinge == true)
                {
                    if (m_nodeChoice == true) FinishChoice(m_currentNode.SelectSingleNode("CHOICE"));
                    if (timer.status == 1 || timer.status == 2) timer.ResetTimer();
                    LogicTransitions();
                    // Если нода подходит для загрузки - загружаем.
                    if (m_onlyLogicLoadinge == false)
                    {
                        UpdateScene();
                    }
                }
            }
        }
        // Воздействия со стороны пользователя. 
        public int OnInputEvent()
        { 
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return) 
                || Input.GetKeyDown(KeyCode.Space)) return 1;
            return 0;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~ Функции поиска и оповещения о нахождении нужного ТЭГА ~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Функция поиска тэга VAR.
        public bool SearchVar()
        {
            if (m_currentNode != null)
            {
                return (m_currentNode.SelectSingleNode("VAR") != null);
            }
            return false;
        }
        // Функция поиска тэга JUMP.
        public bool SearchJump()
        {
            if (m_currentNode != null)
            {
                return (m_currentNode.SelectSingleNode("JUMP") != null);
            }
            return false;
        }
        // Функция поиска тэга CHOICE.
        public bool SearchChoice()
        {
            if (m_currentNode != null)
            {
                return (m_currentNode.SelectSingleNode("CHOICE") != null);
            }
            return false;
        }
        // Проверка на стандарт EVENT - IF
        public bool IsEventIF(XmlNode eventNode)
        {
            if (eventNode != null)
            {
                return ((eventNode.Attributes["var"] != null  && eventNode.Attributes["value"] != null)
                    || (eventNode.Attributes["var1"] != null && eventNode.Attributes["var2"] != null));
            }
            return false;
        }
        // Проверка на стандарт EVENT - TIME
        public bool IsEventTime(XmlNode eventNode)
        {
            if (eventNode != null)
            {
                return (eventNode.Attributes["time"] != null);
            }
            return false;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~ Функции добавления нужный свойств по найденному ТЭГУ ~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Обрабатываем VAR-ы.
        public void AddVars(XmlNode node)
        {
            XmlNodeList varsNodes = node.SelectNodes("VAR");
            foreach (XmlNode var in varsNodes)
            {
                AddOneVar(var);
            }
        }
        // Обрабатываем один VAR.
        public void AddOneVar(XmlNode nodeVar)
        {
            //Debug.Log("Я обработаю VAR");
            // Команда над переменной: "delete".
            XmlAttribute commandAtr = nodeVar.Attributes["command"];
            // Название переменной, куда будет класть.
            XmlAttribute nameAtr = nodeVar.Attributes["name"];  
            // Операции над двумя переменными: " " - "=", "+" - "+", "-" - "-"
            XmlAttribute operationAtr = nodeVar.Attributes["operation"];
            // Значение второго числа в операции.
            XmlAttribute valueAtr = nodeVar.Attributes["value"];
            // Название переменной для второго числа в операции.
            XmlAttribute varAtr = nodeVar.Attributes["var"];
            HandlerVar(commandAtr, nameAtr, operationAtr, valueAtr, varAtr);
        }
        // Обработчик переменных.
        public void HandlerVar(XmlAttribute commandAtr, XmlAttribute nameAtr, XmlAttribute operationAtr, XmlAttribute valueAtr, XmlAttribute varAtr)
        {
            int value = 0;
            if (commandAtr != null && nameAtr != null)
            {
                if (commandAtr.Value == "delete")
                {
                    kernel.var_hash.Remove(nameAtr.Value);
                    return;
                }
            }
            if (valueAtr != null)
            {
                value = int.Parse(valueAtr.Value);
            }
            else
            if (varAtr != null)
            {
                if (kernel.var_hash.ContainsKey(varAtr.Value))
                {
                    value = kernel.var_hash[varAtr.Value];
                }
                else return;
            }
            if (nameAtr != null)
            {
                if (operationAtr == null)
                {
                    kernel.var_hash[nameAtr.Value] = value;
                }
                else
                if (operationAtr.Value == "+")
                {
                    if (kernel.var_hash.ContainsKey(nameAtr.Value))
                    {
                        kernel.var_hash[nameAtr.Value] += value;
                    }
                    else
                    {
                        kernel.var_hash[nameAtr.Value] = value;
                    }
                }
                else
                if (operationAtr.Value == "-")
                {
                    if (kernel.var_hash.ContainsKey(nameAtr.Value))
                    {
                        kernel.var_hash[nameAtr.Value] -= value;
                    }
                    else
                    {
                        kernel.var_hash[nameAtr.Value] = 0 - value;
                    }
                }
            }
        }
        // Обрабатываем EVENT(if).
        public void AddEventIF(XmlNode eventNode)
        {
            int valueFirst = 0;
            int valueSecond = 0;
            string operation;
            XmlAttribute varAtr = eventNode.Attributes["var"];
            XmlAttribute varFirstAtr = eventNode.Attributes["var1"];
            XmlAttribute varSecondAtr = eventNode.Attributes["var2"];
            XmlAttribute operationAtr = eventNode.Attributes["operation"];
            XmlAttribute valueAtr = eventNode.Attributes["value"];
            if (varAtr != null && valueAtr != null)
            {
                if (kernel.var_hash.ContainsKey(varAtr.Value))
                {
                    valueFirst = kernel.var_hash[varAtr.Value];
                    valueSecond = int.Parse(valueAtr.Value);
                }
                else
                {
                    m_onlyLogicLoadinge = true;
                    m_eventIfSuitable = false;
                    return;
                }
            }
            else
            if (varFirstAtr != null && varSecondAtr != null)
            {
                if (kernel.var_hash.ContainsKey(varFirstAtr.Value) 
                    && kernel.var_hash.ContainsKey(varSecondAtr.Value))
                {
                    valueFirst = kernel.var_hash[varFirstAtr.Value];
                    valueSecond = kernel.var_hash[varSecondAtr.Value];
                }
                else
                {
                    m_onlyLogicLoadinge = true;
                    m_eventIfSuitable = false;
                    return;
                }
            }
            operation = (operationAtr != null) ? operationAtr.Value : "==";
            if (operation == "==")
            {
                if (valueFirst == valueSecond)
                {
                    m_onlyLogicLoadinge = false;
                }
                else
                {
                    m_onlyLogicLoadinge = true;
                    m_eventIfSuitable = false;
                }
            }
            else
            if (operation == "!=")
            {
                if (valueFirst != valueSecond)
                {
                    m_onlyLogicLoadinge = false;
                }
                else
                {
                    m_onlyLogicLoadinge = true;
                    m_eventIfSuitable = false;
                }
            }
        }
        //  Обрабатываем EVENT(time).
        public void AddEventTime(XmlNode eventNode)
        {
            if (!m_nodeChoice)
            {
                float time = float.Parse(eventNode.Attributes["time"].Value);
                timer.SetTimer(time);
            }
        }
        // Обрабатываем CHOICE.
        public void AddChoice(XmlNode choiceNode)
        {
            ObjectScript objectScript;
            XmlAttribute id;
            foreach (XmlNode node in choiceNode.SelectNodes("SELECTION"))
            {
                id = node.Attributes["id"];
                if (id != null)
                {
                    objectScript = GameObject.Find(id.Value).GetComponent<ObjectScript>();
                    if (objectScript != null)
                    {
                        objectScript.SetSelectable();
                        selectionObject[id.Value] = objectScript;
                    }
                }
            }
        }
        public void FinishChoice(XmlNode choiceNode)
        {
            // Название переменной, куда будет класть.
            XmlAttribute nameAtr = choiceNode.Attributes["var"];
            // Выбранный после CHOICE объект.
            XmlNode selectionNode = choiceNode.SelectSingleNode("SELECTION[@id = '" + nameChoiceSelected + "']");
            // Операции над двумя переменными: " " - "=", "+" - "+", "-" - "-"
            XmlAttribute operationAtr = selectionNode.Attributes["operation"];
            // Значение второго числа в операции.
            XmlAttribute valueAtr = selectionNode.Attributes["value"];
            // Название переменной для второго числа в операции.
            XmlAttribute varAtr = selectionNode.Attributes["var2"];
            HandlerVar(null, nameAtr, operationAtr, valueAtr, varAtr);

            foreach (var os in selectionObject)
            {
                os.Value.ResetSelectable();
            }
            selectionObject = new Dictionary<string, ObjectScript>();
        }
        // Обрабатываем JUMP.
        public void AddJump(XmlNode jumpNode)
        {
            string toId = jumpNode.Attributes["id"].Value;
            XmlNode node = jumpNode;

            node = (node.ParentNode.Name == "EVENT") ?  node.ParentNode : node;
            node = (node.ParentNode.Name == "SCENE") ? node.ParentNode : node;

            if (jumpNode.Attributes["to"].Value == "scene")
            {
                node = (node.ParentNode.Name == "SCRIPT") ? node.ParentNode : node;

                m_currentNode = node.SelectSingleNode("SCENE[@id = '" + toId + "']");
            }
            if (jumpNode.Attributes["to"].Value == "event")
            {
                m_currentNode = node.SelectSingleNode("EVENT[@id = '" + toId + "']");
            }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~~~ Главный загрузчик объектов по установленной ноде ~~~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Загрузка и создание объектов по ноде.
        public void LoadNode(XmlNode node)
        {
            if (node != null)
            {
                XmlRootAttribute xRoot;
                XmlSerializer formatter;
                XmlData xmlData;
                Data data;
                foreach (XmlNode childnode in node.ChildNodes)
                {
                    // Блок сериализации.
                    xRoot = new XmlRootAttribute
                    {
                        ElementName = childnode.Name,
                        IsNullable = true
                    };
                    formatter = new XmlSerializer(typeof(XmlData), xRoot);
                    xmlData = (XmlData)formatter.Deserialize(new XmlNodeReader(childnode));
                    data = new Data(xmlData, kernel);

                    // Блок перебора текущего тэга.
                    switch (childnode.Name)
                    {
                        case "SPRITE":
                            if (m_scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем SPRITE " + data.id);
                                m_scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log(string.Format("<b>Создаю объект <color=teal>спрайта</color></b> ({0}, {1})", data.id, data.src));
                                m_scene.CreateObject(1, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "ANIMATESPRITE":
                            if (m_scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем ANIMATESPRITE");
                                m_scene.objects[data.id].Edit(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>анимированного спрайта</color> " + data.id + "</b>");
                                m_scene.CreateObject(2, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "VIDEO":
                            if (m_scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем VIDEO");
                                m_scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>видео</color> " + data.id + "</b>");
                                m_scene.CreateObject(3, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "TEXT":
                            if (m_scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем TEXT");
                                m_scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>текста</color> " + data.id + "</b>");
                                m_scene.CreateObject(4, data, m_canvastr);
                                continue;
                            }
                            break;
                        /*case "MUSIC":
                            if (scene.musics.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем MUSIC");
                                scene.musics[data.id].Edit(data);
                            }
                            else
                            {
                                Debug.Log(string.Format("<b>Создаю объект <color=teal>музыки</color></b> ({0}, {1})", data.id, data.src));
                                scene.musics[data.id] = new Music(data);
                                continue;
                            }
                            break;
                        case "SOUND":
                            if (scene.sounds.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем SOUND");
                                scene.sounds[data.id].Edit(data);
                            }
                            else
                            {
                                Debug.Log(string.Format("<b>Создаю объект <color=teal>звука</color></b> ({0}, {1})", data.id, data.src));
                                scene.sounds[data.id] = new Sound(data);
                                continue;
                            }
                            break;*/
                    }
                }
            }
        }
    }
}
