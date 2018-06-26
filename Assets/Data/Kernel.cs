using UnityEngine;
using UnityEngine.UI;

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
        //public bool smooth;             // Размытие				    14          // Вероятнее всего не понадобится // [УБРАТЬ КОММЕНТАРИЙ]
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

    public class Kernel : MonoBehaviour
    {
        // Ширина и высота монитора разработчика.
        public Vector2 devRect;
        // Версия движка.
        public string nameEngine;
        // Версия движка.
        public string version;
        // Цвет заднего фона.
        public Color backgroundColor;
        // Цвет скрывающих полос.
        public Color bandsColor;
        // Main camera.
        public Camera cameraObject;
        // Переменный для проверки обновления размера окна.
        private Vector2 m_lastScreen;
        private Vector2 m_nowScreen;
        // Переменные холстов.
        private CanvasResize m_cr;
        private CanvasResize m_crb;
        // Переменные полос.
        private Band m_bandLT;
        private Band m_bandRB;

        void Start ()
        {
            cameraObject.GetComponent<Camera>().backgroundColor = backgroundColor;
            GetComponent<CXmlReader>().ReadXMLFile("scriptTest");

            GameObject.Find("NGBandRightBottom").GetComponent<Image>().color = bandsColor;
            GameObject.Find("NGBandLeftTop").GetComponent<Image>().color = bandsColor;

            m_bandRB = GameObject.Find("NGBandRightBottom").GetComponent<Band>();
            m_bandLT = GameObject.Find("NGBandLeftTop").GetComponent<Band>();

            m_crb = GameObject.Find("CanvasBand").GetComponent<CanvasResize>();
            m_cr = GameObject.Find("Canvas").GetComponent<CanvasResize>();

            m_lastScreen = new Vector2(0f, 0f);
        }

        public void UpdateBand()
        {
            m_bandLT.UpdateBand("lefttop", devRect);
            m_bandRB.UpdateBand("rightbottom", devRect);
        }

        void Update()
        {
            // Боюсь, что такие штуки только ухудшат быстро действие. [TO DO]
            m_nowScreen = new Vector2(Screen.width, Screen.height);
            if (m_lastScreen != m_nowScreen)
            {
                m_lastScreen = m_nowScreen;
                m_cr.ChangeNatchMode();
                m_crb.ChangeNatchMode();
                UpdateBand();
            }
        }
    }
}
