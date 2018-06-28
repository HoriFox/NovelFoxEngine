using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

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
        // Подходит ли нода для загрузки.
        private bool m_nodeSuitable;
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
            m_nodeSuitable = true;

            m_currentNode = m_xmlRoot.FirstChild;   // Быстрая логика.
            UpdateScene();                          // Обновление сцены.
            CheckBlock();
        }
        public void Start()
        {
            timer = GameObject.Find("NGTimer").GetComponent<TimerPrefabScript>();
            kernel = GameObject.Find("NGGame").GetComponent<Kernel>();
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
            m_nodeSuitable = true;
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
                CheckBlock();
            }
        }

        // Блок проверок на необычные ТЭГИ.
        public void CheckBlock()
        {
            // Проверка ноды на наличие тэга VAR.
            bool boolVar = SearchVar();
            if (boolVar)
            {
                // Добавляем все существующие VAR-ы.
                AddVars(m_currentNode);
            }
            // Проверка ноды на наличие тэга JUMP.
            bool boolJump = SearchJump();
            if (boolJump)
            {
                AddJump(m_currentNode.SelectSingleNode("JUMP"));
            }
            // Проверка ноды на наличие тэга CHOICE.
            bool choiceJump = SearchChoice();
            if (choiceJump)
            {
                AddChoice(m_currentNode.SelectSingleNode("CHOICE"));
            }
            // Проверка и установка IF расширения на EVENT.
            bool boolEventIf = IsEventIF(m_currentNode);
            if (boolEventIf)
            {
                AddEventIF(m_currentNode);
            }
            // Проверка и установка TIME расширения на EVENT.
            bool boolEventTime = IsEventTime(m_currentNode);
            if (boolEventTime)
            {
                AddEventTime(m_currentNode);
            }
        }

        // Контроль логики переключений.
        public void EventController()
        {
            // Для проверки скорости проработки скрипта - отключить условие.
            if (OnInputEvent() == 1 || timer.status == 1 || m_nodeSuitable == false)
            {
                if (timer.status == 1 || timer.status == 2) timer.ResetTimer();
                LogicTransitions();
                // Если нода подходит для загрузки - загружаем.
                if (m_nodeSuitable == true)
                {
                    UpdateScene();
                }
            }
        }
        // Воздействия со стороны пользователя. Убрать Key, если нагрузка! [TO DO] 
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
            XmlNodeList varsNodes;

            varsNodes = node.SelectNodes("VAR");

            foreach (XmlNode var in varsNodes)
            {
                AddOneVar(var);
            }
        }
        // Обрабатываем один VAR.
        public void AddOneVar(XmlNode nodeVar)
        {
            int value = 0;
            XmlAttribute commandAtr = nodeVar.Attributes["command"];
            XmlAttribute nameAtr = nodeVar.Attributes["name"];
            XmlAttribute operationAtr = nodeVar.Attributes["operation"];
            XmlAttribute valueAtr = nodeVar.Attributes["value"];
            XmlAttribute varAtr = nodeVar.Attributes["var"];

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
                    m_nodeSuitable = false;
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
                    m_nodeSuitable = false;
                    return;
                }
            }

            operation = (operationAtr != null) ? operationAtr.Value : "==";

            if (operation == "==")
            {
                m_nodeSuitable = (valueFirst == valueSecond) ? true : false;
                Debug.Log(valueFirst + operation + valueSecond + " - " + m_nodeSuitable);
            }
            else
            if (operation == "!=")
            {
                m_nodeSuitable = (valueFirst != valueSecond) ? true : false;
                Debug.Log(valueFirst + operation + valueSecond + " - " + m_nodeSuitable);
            }
        }
        //  Обрабатываем EVENT(time).
        public void AddEventTime(XmlNode eventNode)
        {
            float time = float.Parse(eventNode.Attributes["time"].Value);
            timer.SetTimer(time);
        }
        // Обрабатываем CHOICE.
        public void AddChoice(XmlNode choiceNode)
        {

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
                foreach (XmlNode childnode in node.ChildNodes)
                {
                    //ResData data = DataObject.GetData(childnode);

                    XmlRootAttribute xRoot = new XmlRootAttribute
                    {
                        ElementName = childnode.Name,
                        IsNullable = true
                    };
                    XmlSerializer formatter = new XmlSerializer(typeof(XmlData), xRoot);
                    XmlData xmlData = (XmlData)formatter.Deserialize(new XmlNodeReader(childnode));
                    Data data = new Data(xmlData);

                    switch (childnode.Name)
                    {
                        case "SPRITE":
                            if (m_scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем SPRITE");
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
