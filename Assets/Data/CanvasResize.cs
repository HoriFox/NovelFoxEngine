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
            // Проверка соотношения и установка нужной ориентации.
            canvas.matchWidthOrHeight = ((float)Screen.width * (1 / ratioRef) <= Screen.height) ? 0f : 1f;
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
