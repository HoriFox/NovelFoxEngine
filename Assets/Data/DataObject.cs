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
    [Serializable]
    public class Data
    {
        private int m_delay;
        private int m_layer;
        private int m_width;
        private int m_height;
        private int m_alpha;   
        private int m_time;    
        private int m_size;
        private float m_x;    
        private float m_y;      
        private float m_scale;
        private float m_angle;
        private float m_volume;
        private float m_speed;
        private bool m_loop;
        private bool m_visible;
        private bool m_layermotion;
        private string m_id;
        private string m_src;
        private string m_text;    
        private string m_style;  
        private string m_color; 
        private string m_fontId;
        private string m_command;
        public uint m_bitMask;


        [XmlAttribute("delay")]                                                     // Параметр задержки		    0
        public int Delay
        {
            get { return m_delay; }
            set { m_delay = value; m_bitMask = m_bitMask | (1 << 0); }
        }
        [XmlAttribute("layer")]                                                     // Слой отображения		        1
        public int Layer
        {
            get { return m_layer; }
            set { m_layer = value; m_bitMask = m_bitMask | (1 << 1); }
        }
        [XmlAttribute("width")]                                                     // Ширина объекта			    2
        public int Width
        {
            get { return m_width; }
            set { m_width = value; m_bitMask = m_bitMask | (1 << 2); }
        }
        [XmlAttribute("height")]                                                    // Высота объекта			    3
        public int Height
        {
            get { return m_height; }
            set { m_height = value; m_bitMask = m_bitMask | (1 << 3); }
        }
        [XmlAttribute("alpha")]                                                     // Прозрачность			        4
        public int Alpha
        {
            get { return m_alpha; }
            set { m_alpha = value; m_bitMask = m_bitMask | (1 << 4); }
        }
        [XmlAttribute("time")]                                                      // Время                        5
        public int Time
        {
            get { return m_time; }
            set { m_time = value; m_bitMask = m_bitMask | (1 << 5); }
        }
        [XmlAttribute("size")]                                                      // Размер текста			    6
        public int Size
        {
            get { return m_size; }
            set { m_size = value; m_bitMask = m_bitMask | (1 << 6); }
        }
        [XmlAttribute("x")]                                                         // Позиция по X			        7
        public float X
        {
            get { return m_x; }
            set { m_x = value; m_bitMask = m_bitMask | (1 << 7); }
        }
        [XmlAttribute("y")]                                                         // Позиция по Y			        8
        public float Y
        {
            get { return m_y; }
            set { m_y = value; m_bitMask = m_bitMask | (1 << 8); }
        }
        [XmlAttribute("scale")]                                                     // Масштаб (sX = sY)		    9
        public float Scale
        {
            get { return m_scale; }
            set { m_scale = value; m_bitMask = m_bitMask | (1 << 9); }
        }
        [XmlAttribute("angle")]                                                     // Угол поворота                10
        public float Angle
        {
            get { return m_angle; }
            set { m_angle = value; m_bitMask = m_bitMask | (1 << 10); }
        }
        [XmlAttribute("volume")]                                                    // Громкость			        11
        public float Volume
        {
            get { return m_volume; }
            set { m_volume = value; m_bitMask = m_bitMask | (1 << 11); }
        }
        [XmlAttribute("speed")]                                                     // Быстрота музыки              12
        public float Speed
        {
            get { return m_speed; }
            set { m_speed = value; m_bitMask = m_bitMask | (1 << 12); }
        }
        [XmlAttribute("loop")]                                                      // Зацикливание			        13
        public bool Loop
        {
            get { return m_loop; }
            set { m_loop = value; m_bitMask = m_bitMask | (1 << 13); }
        }
        [XmlAttribute("visible")]                                                   // Видимость				    15
        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; m_bitMask = m_bitMask | (1 << 15); }
        }
        [XmlAttribute("layermotion")]                                               // Движение слоёв			    16
        public bool Layermotion
        {
            get { return m_layermotion; }
            set { m_layermotion = value; m_bitMask = m_bitMask | (1 << 16); }
        }
        [XmlAttribute("id")]                                                        // Идентификатор объекта	    17
        public string Id
        {
            get { return m_id; }
            set { m_id = value; m_bitMask = m_bitMask | (1 << 17); }
        }
        [XmlAttribute("src")]                                                       // Путь до ресурса			    18
        public string Src
        {
            get { return m_src; }
            set { m_src = value; m_bitMask = m_bitMask | (1 << 18); }
        }
        [XmlText]                                                                   // Содержание текста		    19	
        public string Text
        {
            get { return m_text; }
            set { m_text = value; m_bitMask = m_bitMask | (1 << 19); }
        }
        [XmlAttribute("style")]                                                     // Стиль текста			        20
        public string Style
        {
            get { return m_style; }
            set { m_style = value; m_bitMask = m_bitMask | (1 << 20); }
        }
        [XmlAttribute("color")]                                                     // Цвет текста				    21
        public string Color
        {
            get { return m_color; }
            set { m_color = value; m_bitMask = m_bitMask | (1 << 21); }
        }
        [XmlAttribute("fontId")]                                                    // Идентификатор шрифта	        22
        public string FontId
        {
            get { return m_fontId; }
            set { m_fontId = value; m_bitMask = m_bitMask | (1 << 22); }
        }
        [XmlAttribute("command")]                                                   // Команда					    23
        public string Command
        {
            get { return m_command; }
            set { m_command = value; m_bitMask = m_bitMask | (1 << 23); }
        }

        public Data()
        {
            Id = "null";
            X = 0;
            Y = 0;
            Layermotion = true;
            Scale = 100f;
            Angle = 0;
            Layer = 0;
            Style = "null";
            Visible = true;
            Alpha = 255;
            Src = "null";
            Width = 0;
            Height = 0;
            Loop = false;
            Delay = 40;
            Command = "null";
            Time = 1000;
            Volume = 100;
            Speed = 1;
            Size = 1;
            Text = "NO TEXT";
            FontId = "standart";
            Color = "black";
            m_bitMask = 0;
        }

        //public Data GetData(XmlNode node)
        //{
        //    XmlRootAttribute xRoot = new XmlRootAttribute
        //    {
        //        ElementName = node.Name,
        //        IsNullable = true
        //    };
        //    XmlSerializer formatter = new XmlSerializer(typeof(Data), xRoot);
        //    Data data = (Data)formatter.Deserialize(new XmlNodeReader(node));
        //    return data
        //}
    }

    public class DataObject : MonoBehaviour
    {
        public static bool GetBit(uint x, int pos)
        {
            return (((x) & (1 << (pos))) != 0);
        }
    }
}
