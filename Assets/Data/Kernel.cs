using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace ng
{
    public struct ResData
    {
        //public int delay;               // Параметр задержки		    0
        public int layer;                 // Слой отображения		    1
        //public int width;               // Ширина объекта			    2
        //public int height;              // Высота объекта			    3
        //public int alpha;               // Прозрачность			    4
        //public int time;                // Время                      5
        //public uint size;               // Размер текста			    6
        //public float x;                 // Позиция по X			    7
        //public float y;                 // Позиция по Y			    8
        //public float scale;             // Масштаб (sX = sY)		    9
        //public float angle;             // Угол поворота              10
        //public float volume;            // Громкость			        11
        //public float speed;             // Быстрота музыки            12
        //public bool loop;               // Зацикливание			    13
        //public bool smooth;             // Размытие				    14
        //public bool visible;            // Видимость				    15
        //public bool layermotion;        // Движение слоёв			    16
        public string id;                 // Идентификатор объекта	    17
        public string src;                // Путь до ресурса			18
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
        }

        public Sprite(ResData data, GameObject pre, Transform nodeParent)
        {
            objectSprite = GameObject.Instantiate(pre, nodeParent);
            objectSprite.name = data.id;
            //tr = objectSprite.GetComponent<Transform>();
            sr = objectSprite.GetComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
            sr.sortingOrder = data.layer;
        }

        private SpriteRenderer sr;

        //public SpriteRenderer SriteRenderer { get; set; }
        //private Transform tr;
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
        //SpriteRenderer SriteRenderer { get; set; }
        void Edit(ResData data);
    };

    public class Scene // Перенести создание в Scene
    {
        public int orderLayer = 0;                                  // Позиция в порядке слоёв
        public SortedDictionary<string, IDisplayable> objects;      // Dictionary с объектами
        //public SortedDictionary<string, Sound> sounds;              // Dictionary с звуков
        //public SortedDictionary<string, Music> musics;              // Dictionary с музыкой

        public void CreateObject(int type, ResData data, Transform sceneParent)
        {
            orderLayer += 1;
            data.layer += orderLayer;
            switch (type)
            {
                case 1:
                    objects[data.id] = new Sprite(data, ResourceManager.GetUpgradePrefab("spritePrefab"), sceneParent);
                    break;
                /*case 2:
                    objects[data.id] = new AnimateSprite(data, ResourceManager.GetUpgradePrefab("spritePrefab"), sceneParent);
                    break;*/
                case 3:
                    objects[data.id] = new Video(data, ResourceManager.GetUpgradePrefab("spritePrefab"), sceneParent);
                    break;
                case 4:
                    objects[data.id] = new Text(data, ResourceManager.GetUpgradePrefab("spritePrefab"), sceneParent);
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
        void Start ()
        {
            GetComponent<CXmlReader>().ReadXMLFile("scriptTest");
        }
	
	    void Update ()
        {
	    }
    }
}

