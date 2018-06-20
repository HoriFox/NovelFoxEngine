using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace ng
{
    public class CXmlReader : MonoBehaviour
    {
        XmlNode currentNode;
        XmlDocument xmlDoc;

        public void ReadXMLFile(string nameFile)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(Constants.SctiptPath + nameFile + ".xml");
            XmlElement xmlRoot = xmlDoc.DocumentElement; // Получаем корневой элемент

            currentNode = xmlRoot.FirstChild; //Получаем первую ноду (SCENE) в корневом элементе SCRIPT
            Debug.Log("<color=red>>>> Вызвал первый " + currentNode.Name + "</color>");
            UpdateScene();
        }

        public void Update()
        {
            if (OnInputEvent() == 1)
            {
                UpdateScene();
            }
        }

        public void UpdateScene() // Главный цикл сцен
        {
            if (currentNode != null) // "Если текущая нода существует..."
            {
                EventController();
                if (currentNode.NextSibling != null) // "Если следующая нода существует..."
                {
                    currentNode = currentNode.NextSibling;
                    Debug.Log("Готов воспроизвести следующую сцену с id - " + currentNode.Attributes["id"].Value);
                }
                else
                {
                    currentNode = null;
                }
            }
        }

        public void EventController() // Контроль логики переключений // Вероятно, не нужно, убрать! // [TO DO]
        {
            LoadNode(currentNode);
        }

        public int OnInputEvent()  // Воздействия со стороны пользователя
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0)) return 1;
            return 0;
        }

        public void LoadNode(XmlNode node)
        {
            if (node == null) goto NullNode;
            else
            {
                foreach (XmlNode childnode in node.ChildNodes) // В блоке scene/event
                {
                    ResData data = DataReader.GetData(childnode); // Тут будет чтение всей даты по тэгу
                    if (childnode.Name == "SPRITE")
                    {
                        Debug.Log(string.Format("<b>Создаю объект <color=teal>спрайта</color></b> ({0}, {1})", data.id, data.src));
                        continue;
                    }
                    if (childnode.Name == "ANIMATESPRITE")
                    {
                        Debug.Log("<b>Создаю объект <color=teal>анимированного спрайта</color> " + data.id + "</b>");
                        continue;
                    }
                    if (childnode.Name == "VIDEO")
                    {
                        Debug.Log("<b>Создаю объект <color=teal>видео</color> " + data.id + "</b>");
                        continue;
                    }
                    if (childnode.Name == "TEXT")
                    {
                        Debug.Log("<b>Создаю объект <color=teal>текста</color> " + data.id + "</b>");
                        continue;
                    }
                    if (childnode.Name == "MUSIC")
                    {
                        Debug.Log(string.Format("<b>Создаю объект <color=teal>музыки</color></b> ({0}, {1})", data.id, data.src));
                        continue;
                    }
                    if (childnode.Name == "SOUND")
                    {
                        Debug.Log(string.Format("<b>Создаю объект <color=teal>звука</color></b> ({0}, {1})", data.id, data.src));
                        continue;
                    }
                }
            }
        NullNode:
            Debug.Log("Нода отсутствует");
        }
    }
}
