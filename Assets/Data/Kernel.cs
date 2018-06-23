using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

namespace ng
{
    public struct ResData
    {
        //public int delay;               // Параметр задержки		    0
        public int layer;                   // Слой отображения		    1
        //public int width;               // Ширина объекта			    2
        //public int height;              // Высота объекта			    3
        //public int alpha;               // Прозрачность			    4
        //public int time;                // Время                      5
        //public uint size;               // Размер текста			    6
        public float x;                     // Позиция по X			        7
        public float y;                     // Позиция по Y			        8
        public float scale;                 // Масштаб (sX = sY)		    9
        public float angle;                 // Угол поворота                10
        //public float volume;            // Громкость			        11
        //public float speed;             // Быстрота музыки            12
        //public bool loop;               // Зацикливание			    13
        //public bool smooth;             // Размытие				    14          // Вероятнее всего не понадобится
        public bool visible;                // Видимость				    15
        //public bool layermotion;        // Движение слоёв			    16
        public string id;                   // Идентификатор объекта	    17
        public string src;                  // Путь до ресурса			18
        //public string text;             // Содержание текста		    19		
        //public string style;            // Стиль текста			    20
        //public string color;            // Цвет текста				21	
        //public string fontId;           // Идентификатор шрифта	    22
        //public string command;          // Команда					23

        public uint bitMask;              // Битовая маска изменений
    }

    public class Sprite : IDisplayable
    {
        public GameObject objectSprite;
        public void Edit(ResData data)
        {
            if (DataReader.GetBit(data.bitMask, 1)) // Слой
            {
                sr.sortingOrder = data.layer;
            }
            if (DataReader.GetBit(data.bitMask, 7)) // X позиция
            {
                tr.position = new Vector3(data.x, tr.position.y);
            }
            if (DataReader.GetBit(data.bitMask, 8)) // Y позиция
            {
                tr.position = new Vector3(tr.position.x, data.y);
            }
            if (DataReader.GetBit(data.bitMask, 15)) // Видимость
            {
                objectSprite.SetActive(data.visible);
            }
            if (DataReader.GetBit(data.bitMask, 9)) // Видимость
            {
                tr.localScale = new Vector3(data.scale, data.scale);
            }
            if (DataReader.GetBit(data.bitMask, 18)) // src
            {
                sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
            }
            if (DataReader.GetBit(data.bitMask, 10)) // Угол поворота
            {
                tr.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));
            }
        }

        public Sprite(ResData data, GameObject pre, Transform sceneParent)
        {
            canvastr = sceneParent.GetComponentInChildren<Canvas>().gameObject.transform;
            objectSprite = GameObject.Instantiate(pre, canvastr);
            objectSprite.name = data.id;
            objectSprite.SetActive(data.visible);

            tr = objectSprite.GetComponent<Transform>();
            tr.position = new Vector3(data.x, data.y);
            tr.localScale = new Vector3(data.scale, data.scale);
            tr.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));

            sr = objectSprite.GetComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
            sr.sortingOrder = data.layer;
        }

        private SpriteRenderer sr;
        private Transform tr;
        private Transform canvastr;
    };

    /*public class Anamatesprite : IDisplayable
    {
        public override void Edit(ResData data)
        {
        }
        public Anamatesprite(ResData data, GameObject pre, Transform nodeParent)
        {
        }
    };*/

    public class Video : IDisplayable
    {
        public void Edit(ResData data)
        {
        }
        public Video(ResData data, GameObject pre, Transform nodeParent)
        {
        }
    };

    public class Text : IDisplayable
    {
        public void Edit(ResData data)
        {
        }
        public Text(ResData data, GameObject pre, Transform nodeParent)
        {
        }
    };

    /*public class Music
    {
        public void Edit(ResData data)
        {
        }
        public Music(ResData data)
        {
        }
    };

    public class Sound
    {
        public void Edit(ResData data)
        {
        }
        public Sound(ResData data)
        {
        }
    };*/

    public interface IDisplayable
    {
        void Edit(ResData data);
    };

    public class Scene // Перенести создание в Scene
    {
        public int orderLayer = 0;                                      // Позиция в порядке слоёв
        public SortedDictionary<string, IDisplayable> objects;          // Dictionary с объектами
        //public SortedDictionary<string, Sound> sounds;                // Dictionary с звуков
        //public SortedDictionary<string, Music> musics;                // Dictionary с музыкой

        public void CreateObject(int type, ResData data, Transform sceneParent)
        {
            orderLayer += 1;
            data.layer += orderLayer;
            switch (type)
            {
                case 1:
                    objects[data.id] = new Sprite(data, ResourceManager.GetPrefab("spritePrefab"), sceneParent);
                    break;
                /*case 2:
                    objects[data.id] = new AnimateSprite(data, ResourceManager.GetPrefab("spritePrefab"), sceneParent);
                    break;*/
                case 3:
                    objects[data.id] = new Video(data, ResourceManager.GetPrefab("spritePrefab"), sceneParent);
                    break;
                case 4:
                    objects[data.id] = new Text(data, ResourceManager.GetPrefab("spritePrefab"), sceneParent);
                    break;

            }
        }

        public void EditObject(ResData data)
        {
            objects[data.id].Edit(data);
        }

        public Scene()
        {
            objects = new SortedDictionary<string, IDisplayable>();
            /*sounds = new SortedDictionary<string, Sound>();
            musics = new SortedDictionary<string, Music>();*/
        }
    };

    public class Kernel : MonoBehaviour
    {
        // Версия движка.
        public string version;
        // Ширина и высота монитора разработчика.
        public int devMonitorWidth;
        public int devMonitorHeight;
        // Цвет заднего фона.
        public Color backgroundColor;
        // Main camera.
        public Camera cameraObject;

        void Start ()
        {
            cameraObject.GetComponent<Camera>().backgroundColor = backgroundColor;
            GetComponent<CXmlReader>().ReadXMLFile("scriptTest");
        }
    }
}



// Придётся добавить canvas
