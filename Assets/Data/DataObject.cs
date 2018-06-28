using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace ng
{
    enum OA
    {
      delay = 0, layer, width, height, alpha, time, size, x, y, scale,
      angle, volume, speed, loop, visible, layermotion, id, src,
      text, style, color, fontId, command
    }

    [Serializable]
    public class XmlData
    {
        [XmlAttribute("delay")]                                                     // Параметр задержки		    0
        public string Delay { get; set; }
        [XmlAttribute("layer")]                                                     // Слой отображения		        1
        public string Layer { get; set; }
        [XmlAttribute("width")]                                                     // Ширина объекта			    2
        public string Width { get; set; }
        [XmlAttribute("height")]                                                    // Высота объекта			    3
        public string Height { get; set; }
        [XmlAttribute("alpha")]                                                     // Прозрачность			        4
        public string Alpha { get; set; }
        [XmlAttribute("time")]                                                      // Время                        5
        public string Time { get; set; }
        [XmlAttribute("size")]                                                      // Размер текста			    6
        public string Size { get; set; }
        [XmlAttribute("x")]                                                         // Позиция по X			        7
        public string X { get; set; }
        [XmlAttribute("y")]                                                         // Позиция по Y			        8
        public string Y { get; set; }
        [XmlAttribute("scale")]                                                     // Масштаб (sX = sY)		    9
        public string Scale { get; set; }
        [XmlAttribute("angle")]                                                     // Угол поворота                10
        public string Angle { get; set; }
        [XmlAttribute("volume")]                                                    // Громкость			        11
        public string Volume { get; set; }
        [XmlAttribute("speed")]                                                     // Быстрота музыки              12
        public string Speed { get; set; }
        [XmlAttribute("loop")]                                                      // Зацикливание			        13
        public string Loop { get; set; }
        [XmlAttribute("visible")]                                                   // Видимость				    14
        public string Visible { get; set; }
        [XmlAttribute("layermotion")]                                               // Движение слоёв			    15
        public string Layermotion { get; set; }
        [XmlAttribute("id")]                                                        // Идентификатор объекта	    16
        public string Id { get; set; }
        [XmlAttribute("src")]                                                       // Путь до ресурса			    17
        public string Src { get; set; }
        [XmlText]                                                                   // Содержание текста		    18	
        public string Text { get; set; }
        [XmlAttribute("style")]                                                     // Стиль текста			        19
        public string Style { get; set; }
        [XmlAttribute("color")]                                                     // Цвет текста				    20
        public string Color { get; set; }
        [XmlAttribute("fontId")]                                                    // Идентификатор шрифта	        21
        public string FontId { get; set; }
        [XmlAttribute("command")]                                                   // Команда					    22
        public string Command { get; set; }
    }

    public class Data
    {
        public static bool Convtrue(string str)
        {
            return (string.Compare(str, "true", true) == 0 ? true : false);
        }

        public int delay;
        public int layer;
        public int width;
        public int height;
        public int alpha;
        public float time;
        public int size;
        public float x;
        public float y;
        public float scale;
        public float angle;
        public float volume;
        public float speed;
        public bool loop;
        public bool visible;
        public bool layermotion;
        public string id;
        public string src;
        public string text;
        public string style;
        public string color;
        public string fontId;
        public string command;
        public uint bitMask;

        public Data(XmlData xmld)
        {
            if (xmld.Delay != null)
            {
                delay = int.Parse(xmld.Delay);
                bitMask = bitMask | (1 << (int)OA.delay);
            }
            else delay = 40;
            if (xmld.Layer != null)
            {
                layer = int.Parse(xmld.Layer) * Constants.LayerDivision;
                bitMask = bitMask | (1 << (int)OA.layer);
            }
            else layer = 0;

            if (xmld.Width != null)
            {
                width = int.Parse(xmld.Width);
                bitMask = bitMask | (1 << (int)OA.width);
            }
            else width = 0; 
            if (xmld.Height != null)
            {
                height = int.Parse(xmld.Height);
                bitMask = bitMask | (1 << (int)OA.height);
            }
            else height = 0;
            if (xmld.Alpha != null)
            {
                alpha = 255 * int.Parse(xmld.Alpha) / 100;
                bitMask = bitMask | (1 << (int)OA.alpha);
            }
            else alpha = 255;
            if (xmld.Time != null)
            {
                time = float.Parse(xmld.Time);
                bitMask = bitMask | (1 << (int)OA.time);
            }
            else time = 1000;
            if (xmld.Size != null)
            {
                size = int.Parse(xmld.Size);
                bitMask = bitMask | (1 << (int)OA.size);
            }
            else size = 1;
            if (xmld.X != null)
            {
                x = float.Parse(xmld.X);
                bitMask = bitMask | (1 << (int)OA.x);
            }
            else x = 0;
            if (xmld.Y != null)
            {
                y = float.Parse(xmld.Y);
                bitMask = bitMask | (1 << (int)OA.y);
            }
            else y = 0;
            if (xmld.Scale != null)
            {
                scale = float.Parse(xmld.Scale) * 100f;
                bitMask = bitMask | (1 << (int)OA.scale);
            }
            else scale = 100f;
            if (xmld.Angle != null)
            {
                angle = float.Parse(xmld.Angle);
                bitMask = bitMask | (1 << (int)OA.angle);
            }
            else angle = 0;
            if (xmld.Volume != null)
            {
                volume = float.Parse(xmld.Volume);
                bitMask = bitMask | (1 << (int)OA.volume);
            }
            else volume = 100;
            if (xmld.Speed != null)
            {
                speed = float.Parse(xmld.Speed);
                bitMask = bitMask | (1 << (int)OA.speed);
            }
            else speed = 1;
            if (xmld.Loop != null)
            {
                loop = Convtrue(xmld.Loop);
                bitMask = bitMask | (1 << (int)OA.loop);
            }
            else loop = false;
            if (xmld.Visible != null)
            {
                visible = Convtrue(xmld.Visible);
                bitMask = bitMask | (1 << (int)OA.visible);
            }
            else visible = true;
            if (xmld.Layermotion != null)
            {
                layermotion = Convtrue(xmld.Layermotion);
                bitMask = bitMask | (1 << (int)OA.layermotion);
            }
            else layermotion = true;
            if (xmld.Id != null)
            {
                id = xmld.Id;
                bitMask = bitMask | (1 << (int)OA.id);
            }
            else id = "null";
            if (xmld.Src != null)
            {
                src = xmld.Src;
                bitMask = bitMask | (1 << (int)OA.src);
            }
            else src = "null";
            if (xmld.Text != null)
            {
                text = xmld.Text;
                bitMask = bitMask | (1 << (int)OA.text);
            }
            else text = "NO TEXT";
            if (xmld.Style != null)
            {
                style = xmld.Style;
                bitMask = bitMask | (1 << (int)OA.style);
            }
            else style = "null";

            if (xmld.Color != null)
            {
                color = xmld.Color;
                bitMask = bitMask | (1 << (int)OA.color);
            }
            else color = "black";
            if (xmld.FontId != null)
            {
                fontId = xmld.FontId;
                bitMask = bitMask | (1 << (int)OA.fontId);
            }
            else fontId = "standart";
            if (xmld.Command != null)
            {
                command = xmld.Command;
                bitMask = bitMask | (1 << (int)OA.command);
            }
            else command = "null";
        }
    }

    public class DataObject : MonoBehaviour
    {
        public static bool GetBit(uint x, int pos)
        {
            return (((x) & (1 << (pos))) != 0);
        }
    }
}
