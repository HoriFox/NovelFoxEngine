using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ng
{
    public class ConfigPlatform : MonoBehaviour
    {

        void Start()
        {
            Kernel kernel = GameObject.Find("NGGame").GetComponent<Kernel>();
            PlayerSettings.productName = kernel.nameEngine;                     // Не работает
        }
    }
}
