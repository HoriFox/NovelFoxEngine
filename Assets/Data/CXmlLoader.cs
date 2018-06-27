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

            m_currentNode = m_xmlRoot.FirstChild;   // Быстрая логика.
            UpdateScene();                          // Обновление сцены.
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
                if (m_currentNode.SelectSingleNode("EVENT") != null)          // Если эта нода имеет первый ненулевой EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    m_currentNode = m_currentNode.SelectSingleNode("EVENT");
                }
                else                                                        // Иначе, мы находимся в самом EVENT или EVENT нет // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    if (m_currentNode.NextSibling != null)                    // Если следующая нода ненулевая, если мы в EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                    {
                        m_currentNode = m_currentNode.NextSibling;
                    }
                    else                                                    // Следующая нода нулевая // [УБРАТЬ КОММЕНТАРИЙ]
                    {
                        if (m_currentNode.Name == "EVENT")                    // Если мы в EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                        {
                            m_currentNode = m_currentNode.ParentNode;
                            if (m_currentNode.NextSibling != null)
                            {
                                m_currentNode = m_currentNode.NextSibling;
                            }
                        }
                        else                                                // Если это SCENE // [УБРАТЬ КОММЕНТАРИЙ]
                        {
                            m_currentNode = null;
                            Debug.Log("Конец сценария");
                        }
                    }
                }
                // Проверка ноды на наличие тэга JUMP.
                bool boolJump = SearchJump(m_currentNode);
                if (boolJump)                                               // Есть ли JUMP // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    AddJump(m_currentNode.SelectSingleNode("JUMP"));
                }
                // Проверка ноды на наличие тэга CHOICE.
                bool choiceJump = SearchChoice(m_currentNode);
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
        }

        // Контроль логики переключений.
        public void EventController()
        {
            // Для проверки скорости проработки скрипта - отключить условие.
            if (OnInputEvent() == 1 )
            {
                LogicTransitions();
                UpdateScene();
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

        // Функция поиска тэга JUMP.
        public bool SearchJump(XmlNode jumpNode)
        {
            return (m_currentNode != null && m_currentNode.SelectSingleNode("JUMP") != null);
        }
        // Функция поиска тэга CHOICE.
        public bool SearchChoice(XmlNode choiceNode)
        {
            return (m_currentNode != null && m_currentNode.SelectSingleNode("CHOICE") != null);
        }
        // Проверка на стандарт EVENT - IF
        public bool IsEventIF(XmlNode eventNode)
        {
            return (eventNode != null && eventNode.Attributes["var"] != null);
        }
        // Проверка на стандарт EVENT - TIME
        public bool IsEventTime(XmlNode eventNode)
        {
            return (eventNode != null && eventNode.Attributes["time"] != null);
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~ Функции добавления нужный свойств по найденному ТЭГУ ~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Обрабатываем EVENT(if).
        public void AddEventIF(XmlNode eventNode)
        {
            string var = eventNode.Attributes["var"].Value;
            string operation = eventNode.Attributes["operation"].Value;
            string value = eventNode.Attributes["value"].Value;
        }
        //  Обрабатываем EVENT(time).
        public void AddEventTime(XmlNode eventNode)
        {
            float time = float.Parse(eventNode.Attributes["time"].Value);
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
                foreach (XmlNode childnode in node.ChildNodes) // Сделать отдельные классы [TO DO]
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
