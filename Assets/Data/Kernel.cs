using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace ng
{
    enum ResDT
    {
        _delay = 0, _layer, _width, _height, _alpha, _time, _size, _x, _y,
        _scale, _angle, _volume, _speed, _loop, _smooth, _visible, _layermotion, _id,
        _src, _text, _style, _color, _fontId, _command
    }

    public struct ResData
    {
        //public int delay;               // Параметр задержки		0
        //public int layer;               // Слой отображения		    1
        //public int width;               // Ширина объекта			2
        //public int height;              // Высота объекта			3
        //public int alpha;               // Прозрачность			    4
        //public int time;                // Время                    5
        //public uint size;               // Размер текста			6
        //public float x;                 // Позиция по X			    7
        //public float y;                 // Позиция по Y			    8
        //public float scale;             // Масштаб (sX = sY)		9
        //public float angle;             // Угол поворота            10
        //public float volume;            // Громкость			    11
        //public float speed;             // Быстрота музыки          12
        //public bool loop;               // Зацикливание			    13
        //public bool smooth;             // Размытие				    14
        //public bool visible;            // Видимость				15
        //public bool layermotion;        // Движение слоёв			16
        public string id;                 // Идентификатор объекта	17
        public string src;                // Путь до ресурса			18
        //public string text;             // Содержание текста		19		
        //public string style;            // Стиль текста			    20
        //public string color;            // Цвет текста				21	
        //public string fontId;           // Идентификатор шрифта	    22
        //public string command;          // Команда					23

        public uint bitMask;            // Битовая маска изменений
    }

    public class Sprite : Displayable
	{
	    Sprite() { }
        Sprite(ResData rd) { }
    };

    public class Displayable
    {
        //~Displayable() { }
    };

    public class Scene
    {
        protected List<Displayable> layers = new List<Displayable>(21);         // Слои
        Dictionary<string, Displayable> objects;                                // Dictionary с объектами

        Scene() // Конструктор стандарт
        {

        }
        Scene(XmlNode node) // Конструктор по ресурсам // Заменить XmlNode на DataClass
        {

        }
        ~Scene() // Деструктор
        {
            // Как стало понятно, в C# вместе с удалением Class удаляется и его содержимое
        }
    };

    public class Kernel : MonoBehaviour {

        void Start () {
            GetComponent<CXmlReader>().ReadXMLFile("scriptTest");
        }
	
	    void Update () {
		
	    }
    }
}

