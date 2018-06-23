using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace ng
{
    public class CXmlReader : MonoBehaviour
    {
        public Transform sceneParent;
        private Transform canvastr;
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
                if (currentNode.Name == "SCENE")
                {
                    scene = new Scene();
                    canvastr = sceneParent.GetComponentInChildren<Canvas>().gameObject.transform;
                    foreach (Transform t in canvastr)
                    {
                        //if (t != sceneParent) // Если Scene закинули в Scene (случайно)
                        Destroy(t.gameObject);
                    }
                    string line = "============================================================================";
                    Debug.Log("<b>" + line + " Пересоздаём Scene [" + currentNode.Attributes["id"].Value + "]</b>");
                }
                LoadNode(currentNode);
                if (currentNode.SelectSingleNode("EVENT") != null)
                {
                    currentNode = currentNode.SelectSingleNode("EVENT");
                }
                else
                {
                    if (currentNode.NextSibling != null)
                    {
                        currentNode = currentNode.NextSibling;
                    }
                    else 
                    if (currentNode.Name == "EVENT")
                    {
                        currentNode = currentNode.ParentNode;
                        if (currentNode.NextSibling != null)
                        {
                            currentNode = currentNode.NextSibling;
                        }
                    }
                    else
                    {
                        currentNode = null;
                        Debug.Log("Конец сценария");
                    }
                }
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
            if (Key(KeyCode.Mouse0) || Key(KeyCode.Return) || Key(KeyCode.Space)) return 1;
            return 0;
        }
        // Упрощение GetKeyDown.
        public bool Key(KeyCode code)
        {
            return Input.GetKeyDown(code);
        }
        // Загрузка и создание объектов по ноде.
        public void LoadNode(XmlNode node)
        {
            if (node == null)
            {
                goto NullNode;
            }
            else
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
                                scene.CreateObject(1, data, sceneParent);
                                continue;
                            }
                            break;
                        /*case "ANIMATESPRITE":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем ANIMATESPRITE");
                                scene.objects[data.id].Edit(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>анимированного спрайта</color> " + data.id + "</b>");
                                scene.CreateObject(2, data, sceneParent);
                                continue;
                            }
                            break;*/
                        case "VIDEO":
                            if (scene.objects.ContainsKey(data.id))
                            {
                                Debug.Log("Изменяем VIDEO");
                                scene.EditObject(data);
                            }
                            else
                            {
                                Debug.Log("<b>Создаю объект <color=teal>видео</color> " + data.id + "</b>");
                                scene.CreateObject(3, data, sceneParent);
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
                                scene.CreateObject(4, data, sceneParent);
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
            NullNode:
                Debug.Log("Нода отсутствует");
        }
    }
}
