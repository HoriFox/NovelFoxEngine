using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ng{
    public class SpritePrefabScript : MonoBehaviour
    {
        // Нахождение мыши над объектом.
        public bool mouseOnObject = false;

        // Мышь навелась на объект.
        void OnMouseEnter()
        {
            mouseOnObject = true;
        }

        // Мышь ушла с объекта.
        void OnMouseExit()
        {
            mouseOnObject = false;
        }
    }
}
