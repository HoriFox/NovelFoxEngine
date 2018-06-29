using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ng{
    public class ObjectScript : MonoBehaviour
    {
        // Объект выделяем.
        public bool isSelectable = false;
        // Нахождение мыши над объектом.
        //public bool mouseOnObject = false;   // После тестов убрать! TO DO
        private CXmlLoader xmlLoader;

        private void Start()
        {
            xmlLoader = GameObject.Find("NGGame").GetComponent<CXmlLoader>();
        }
        //// Мышь навелась на объект.
        //void OnMouseEnter()                 // После тестов убрать! TO DO
        //{
        //    mouseOnObject = true;
        //}
        //// Мышь ушла с объекта.
        //void OnMouseExit()                  // После тестов убрать! TO DO
        //{
        //    mouseOnObject = false;
        //}
        //// Каждый кадр, пока мышь находится на объекте.
        //private void OnMouseOver()
        //{
        //    Debug.Log(name + " выделен");
        //}
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
