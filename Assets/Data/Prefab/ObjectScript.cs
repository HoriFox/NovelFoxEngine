using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ng{
    public class ObjectScript : MonoBehaviour
    {
        // Объект выделяем.
        public bool isSelectable = false;

        private CXmlLoader xmlLoader;

        private void Start()
        {
            xmlLoader = GameObject.Find("NGGame").GetComponent<CXmlLoader>();
        }
        // Произвели клик на объекте.
        private void OnMouseDown()
        {
            if (isSelectable == true)
            {
                xmlLoader.nameChoiceSelected = name;
                xmlLoader.isObjectSelected = true;
            }
        }
        // Установка выделяемости.
        public void SetSelectable()
        {
            isSelectable = true;
        }
        // Сбрасывание выделяемости.
        public void ResetSelectable()
        {
            isSelectable = false;
        }
    }
}
