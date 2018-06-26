using UnityEngine;
using System.Xml;

namespace ng
{
    public class CXmlReader : MonoBehaviour
    {
        public Transform sceneParent;
        private Transform m_canvastr;
        Scene scene;
        XmlNode currentNode;
        XmlDocument xmlDoc;
        //Загрузка и настройка xml файла.
        public void ReadXMLFile(string nameFile)
        {
            //Debug.Log(pre.ToString());
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreComments = true
            };
            XmlReader reader = XmlReader.Create(Constants.SctiptPath + nameFile + ".xml", readerSettings);
            xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement xmlRoot = xmlDoc.DocumentElement;
            currentNode = xmlRoot.FirstChild;

            Debug.Log("<b><color=red>Вызвал первый " + currentNode.Name + "</color></b>");
            UpdateScene();
        }
        // Главное обновление.
        public void Update()
        {
            EventController();
        }
        // Обход сценария xml. 
        public void UpdateScene()
        {
            if (currentNode != null)
            {
                ReloadScene();
                LoadNode(currentNode);
                if (currentNode.SelectSingleNode("EVENT") != null)          // Если эта нода имеет первый ненулевой EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    currentNode = currentNode.SelectSingleNode("EVENT");
                }
                else                                                        // Иначе, мы находимся в самом EVENT или EVENT нет // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    if (currentNode.NextSibling != null)                    // Если следующая нода ненулевая, если мы в EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                    {
                        currentNode = currentNode.NextSibling;
                    }
                    else                                                    // Следующая нода нулевая // [УБРАТЬ КОММЕНТАРИЙ]
                    {
                        if (currentNode.Name == "EVENT")                    // Если мы в EVENT // [УБРАТЬ КОММЕНТАРИЙ]
                        {
                            currentNode = currentNode.ParentNode;
                            if (currentNode.NextSibling != null)
                            {
                                currentNode = currentNode.NextSibling;
                            }
                        }
                        else                                                // Если это SCENE // [УБРАТЬ КОММЕНТАРИЙ]
                        {
                            currentNode = null;
                            Debug.Log("Конец сценария");
                        }
                    }
                }
                bool boolJump = SearchJump(currentNode);
                if (boolJump)                                               // Есть ли JUMP // [УБРАТЬ КОММЕНТАРИЙ]
                {
                    Jump(currentNode.SelectSingleNode("JUMP"));
                }
            }
        }
        // Пересоздание сцены.
        public void ReloadScene()
        {
            if (currentNode.Name == "SCENE")
            {
                scene = new Scene();
                m_canvastr = sceneParent.GetComponentInChildren<Canvas>().gameObject.transform;
                foreach (Transform t in m_canvastr)
                {
                    Destroy(t.gameObject);
                }
                string line = "============================================================================";
                Debug.Log("<b>" + line + " Пересоздаём Scene [" + currentNode.Attributes["id"].Value + "]</b>");
            }
        }
        // Контроль логики переключений. Вероятно, не нужно, убрать! [TO DO]
        public void EventController()
        {
            if (OnInputEvent() == 1)
            {
                UpdateScene();
            }
        }
        // Воздействия со стороны пользователя. Убрать Key, если нагрузка! [TO DO] 
        public int OnInputEvent()
        { 
            if (CheckKey(KeyCode.Mouse0) || CheckKey(KeyCode.Return) || CheckKey(KeyCode.Space)) return 1;
            return 0;
        }
        // Упрощение GetKeyDown.
        public bool CheckKey(KeyCode code)
        {
            return Input.GetKeyDown(code);
        }
        // Обрабатываем EVENT(if).
        public void GetEventIF(XmlNode eventNode)
        {
            string var = eventNode.Attributes["var"].Value;
            string operation = eventNode.Attributes["operation"].Value;
            string value = eventNode.Attributes["value"].Value;
        }
        //  Обрабатываем EVENT(time).
        public void GetEventTime(XmlNode eventNode)
        {
            float time = float.Parse(eventNode.Attributes["time"].Value);
        }
        // Функция поиска тэга JUMP.
        public bool SearchJump(XmlNode jumpNode)
        {
            if (currentNode != null && currentNode.SelectSingleNode("JUMP") != null)
            {
                return true;
            }
            return false;
        }
        // Исполнение функции JUMP.
        public void Jump(XmlNode jumpNode)
        {
            string toId = jumpNode.Attributes["id"].Value;
            XmlNode node = jumpNode;

            if (node.ParentNode.Name == "EVENT")
            {
                node = node.ParentNode;
            }
            if (node.ParentNode.Name == "SCENE")
            {
                node = node.ParentNode;
            }

            if (jumpNode.Attributes["to"].Value == "event")
            {
                currentNode = node.SelectSingleNode("EVENT[@id = '" + toId + "']");
                return;
            }
            else
            if (jumpNode.Attributes["to"].Value == "scene")
            {
                if (node.ParentNode.Name == "SCRIPT")
                {
                    node = node.ParentNode;
                }
                currentNode = node.SelectSingleNode("SCENE[@id = '" + toId + "']");
            }
        }
        // Загрузка и создание объектов по ноде.
        public void LoadNode(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode childnode in node.ChildNodes) // Сделать отдельные классы [TO DO]
                {
                    ResData data = DataReader.GetData(childnode);
                    switch (childnode.Name)
                    {
                        case "SPRITE":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем SPRITE");
                                scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log(string.Format("<b>Создаю объект <color=teal>спрайта</color></b> ({0}, {1})", data.id, data.src));
                                scene.CreateObject(1, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "ANIMATESPRITE":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем ANIMATESPRITE");
                                scene.objects[data.id].Edit(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>анимированного спрайта</color> " + data.id + "</b>");
                                scene.CreateObject(2, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "VIDEO":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем VIDEO");
                                scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>видео</color> " + data.id + "</b>");
                                scene.CreateObject(3, data, m_canvastr);
                                continue;
                            }
                            break;
                        case "TEXT":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем TEXT");
                                scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>текста</color> " + data.id + "</b>");
                                scene.CreateObject(4, data, m_canvastr);
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
