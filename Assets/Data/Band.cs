using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Band : MonoBehaviour {
    private float ratioRef;
    private float ratio;
    private float k;
    private float sizeY;
    private float sizeX;
    RectTransform rt;

    public void UpdateBand(string place, Vector2 devRect)
    {
        rt = GetComponent<RectTransform>();
        ratioRef = devRect.x / devRect.y;
        ratio = (float)Screen.width / (float)Screen.height;

        // Настройка скрывающих полос.
        if (ratio < ratioRef)
        {
            k = (Screen.width / (float)devRect.x);
            sizeY = (Math.Abs(Screen.height - devRect.y * k) / (2.0f * k));
            sizeX = devRect.x;
            rt.sizeDelta = new Vector2(sizeX, sizeY);

            if (place == "lefttop")
            {
                rt.localPosition = new Vector2(0, (devRect.y / 2.0f) + (sizeY / 2.0f));
            }
            if (place == "rightbottom")
            {
                rt.localPosition = new Vector2(0, -((devRect.y / 2.0f) + (sizeY / 2.0f)));
            }
        }
        else if (ratio >= ratioRef)
        {
            k = (Screen.height / (float)devRect.y);
            sizeX = (Math.Abs(Screen.width - devRect.x * k) / (2.0f * k));
            sizeY = devRect.y;
            rt.sizeDelta = new Vector2(sizeX, sizeY);

            if (place == "lefttop")
            {
                rt.localPosition = new Vector2((devRect.x / 2.0f) + (sizeX / 2.0f), 0);
            }
            if (place == "rightbottom")
            {
                rt.localPosition = new Vector2(-((devRect.x / 2.0f) + (sizeX / 2.0f)), 0);
            }
        }
    }
}