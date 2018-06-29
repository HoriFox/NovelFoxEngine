using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ng
{
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
        // Хэш-словарь переменных.
        public Dictionary<string, int> var_hash;

        private void Awake()
        {
            // Спросить у Яна насчёт Awake и Start, где лучше "дообьявлять переменные"
            //var_hash = new Dictionary<string, int>();
        }

        void Start ()
        {
            var_hash = new Dictionary<string, int>();

            cameraObject.GetComponent<Camera>().backgroundColor = backgroundColor;
            GetComponent<CXmlLoader>().ReadXMLFile("scriptTest");

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

        // Коэффициент разрешение конечного игрока к разработчику.
        public Vector2 WindowsCoefficient()
        {
            float XCoef = Screen.width / devRect.x;
            float YCoef = Screen.height / devRect.y;
            return new Vector2(XCoef, YCoef);
        }

        void Update()
        {
            // Боюсь, что такие штуки только ухудшат быстро действие. [TO DO]
            m_nowScreen = new Vector2(Screen.width, Screen.height);
            if (m_lastScreen != m_nowScreen)
            {
                m_lastScreen = m_nowScreen;
                m_cr.ChangeMatchMode();
                m_crb.ChangeMatchMode();
                UpdateBand();
            }
        }
    }
}
