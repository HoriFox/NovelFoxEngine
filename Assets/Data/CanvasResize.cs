using UnityEngine;
using UnityEngine.UI;

namespace ng
{
    public class CanvasResize : MonoBehaviour
    {
        public Kernel kernel;
        private CanvasScaler canvas;
        private float ratioRef;
        private float ratio;

        // Считаем и устанавливаем преимущество стороны width или height в resize-е.
        public void ChangeMatchMode()
        {
            ratioRef = kernel.devRect.x / kernel.devRect.y;
            //ratio = (float)Screen.width / (float)Screen.height;
            canvas.matchWidthOrHeight = ((float)Screen.width * (1 / ratioRef) <= Screen.height) ? 0f : 1f; // Странный способ. TO DO
        }
        private void Awake()
        {
            kernel = GameObject.Find("NGGame").GetComponent<Kernel>();
        }
        void Start()
        {
            // Устанавливаем родительское разрешение.
            canvas = GetComponent<CanvasScaler>();
            canvas.referenceResolution = new Vector2(kernel.devRect.x, kernel.devRect.y);
        }
    }
}
