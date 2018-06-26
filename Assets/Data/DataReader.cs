using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace ng
{
    public class DataReader : MonoBehaviour
    {
        public static bool GetBit(uint x, int pos)
        {
            return (((x) & (1 << (pos))) != 0);
        }

        public static bool Convtrue(string str)
        {
            return (string.Compare(str, "true", true) == 0);
        }

        public static ResData GetData(XmlNode node) // Сериализатор! TODO
        {
            XmlAttribute id = node.Attributes["id"];
            XmlAttribute x = node.Attributes["x"];
            XmlAttribute y = node.Attributes["y"];
            //XmlAttribute layermotion = node.Attributes["layermotion"];
            XmlAttribute scale = node.Attributes["scale"];
            XmlAttribute angle = node.Attributes["angle"];
            XmlAttribute layer = node.Attributes["layer"];
            //XmlAttribute style = node.Attributes["style"];
            XmlAttribute visible = node.Attributes["visible"];
            //XmlAttribute alpha = node.Attributes["alpha"];
            XmlAttribute src = node.Attributes["src"];
            //XmlAttribute smooth = node.Attributes["smooth"];

            //XmlAttribute width = node.Attributes["width"];
            //XmlAttribute height = node.Attributes["height"];
            //XmlAttribute loop = node.Attributes["loop"];
            //XmlAttribute delay = node.Attributes["delay"];

            //XmlAttribute command = node.Attributes["command"];
            //XmlAttribute time = node.Attributes["time"];
            //XmlAttribute volume = node.Attributes["volume"];
            //XmlAttribute speed = node.Attributes["speed"];

            //XmlAttribute size = node.Attributes["size"];
            //XmlAttribute text = node.InnerText;
            //XmlAttribute fontId = node.Attributes["font"];
            //XmlAttribute color = node.Attributes["color"];

            ResData res = new ResData
            {
                bitMask = 0
            };
            
            // На что только не пойдёшь ради живущего внутри перфекциониста.

            // [id].
            if (id != null)
            {
                res.id = id.Value;
                res.bitMask = res.bitMask | (1 << 17);
            }
            else
            {
                res.id = "null";
            }
            // [x].
            if (x != null)
            {
                res.x = float.Parse(x.Value);
                res.bitMask = res.bitMask | (1 << 7);
            }
            else
            {
                res.x = 0;
            }
            // [y].
            if (y != null)
            {
                res.y = float.Parse(y.Value);
                res.bitMask = res.bitMask | (1 << 8);
            }
            else
            {
                res.y = 0;
            }
            // [layermotion].
            /*if (layermotion != null) 
            {
                res.layermotion = Convtrue(layermotion.Value);
                res.bitMask = res.bitMask | (1 << 16);
            }
            else
            {
                res.layermotion = true;
            }*/
            // [scale].
            if (scale != null)
            {
                res.scale = float.Parse(scale.Value) * 100f;
                res.bitMask = res.bitMask | (1 << 9);
            }
            else
            {
                res.scale = 100f;
            }
            // [angle].
            if (angle != null)
            {
                res.angle = float.Parse(angle.Value);
                res.bitMask = res.bitMask | (1 << 10);
            }
            else
            {
                res.angle = 0;
            }
            // [layer].
            if (layer != null)
            {
                res.layer = int.Parse(layer.Value) * Constants.LayerDivision;
                res.bitMask = res.bitMask | (1 << 1);
            }
            else
            {
                res.layer = 0;
            }
            // [style].
            /*if (style != null) 
            {
                res.style = style.Value;
                res.bitMask = res.bitMask | (1 << 20);
            } 
            else 
            {
                res.style = "null";
            }*/
            // [visible].
            if (visible != null)
            {
                res.visible = Convtrue(visible.Value);
                res.bitMask = res.bitMask | (1 << 15);
            }
            else
            {
                res.visible = true;
            }
            // [alpha].
            /*if (alpha != null)
            {
                res.alpha = 255 * int.Parse(alpha.Value) / 100;
                res.bitMask = res.bitMask | (1 << 4);
            }
            else
            {
                res.alpha = 255; // [!]
            }*/
            // [src].
            if (src != null)
            {
                res.src = src.Value;
                res.bitMask = res.bitMask | (1 << 18);
            }
            else
            {
                res.src = "null";
            }
            // [smooth].
            /*if (smooth != null)
            {
                res.smooth = Convtrue(smooth.Value);
                res.bitMask = res.bitMask | (1 << 14);
            }
            else
            {
                res.smooth = true;
            }*/
            // [width].
            /*if (width != null)
            {
                res.width = int.Parse(width.Value);
                res.bitMask = res.bitMask | (1 << 2);
            }
            else
            {
                res.width = 0;
            }*/
            // [height].
            /*if (height != null)
            {
                res.height = int.Parse(height.Value);
                res.bitMask = res.bitMask | (1 << 3);
            }
            else
            {
                res.height = 0;
            }*/
            // [loop].
            /*if (loop != null)
            {
                res.loop = Convtrue(loop.Value);
                res.bitMask = res.bitMask | (1 << 13);
            }
            else
            {
                res.loop = false;
            }*/
            // [delay].
            /*if (delay != null)
            {
                res.delay = int.Parse(delay.Value);
                res.bitMask = res.bitMask | (1 << 0);
            }
            else
            {
                res.delay = 40;
            }*/
            // [command].
            /*if (command != null)
            {
                res.command = command.Value;
                res.bitMask = res.bitMask | (1 << 23);
            }
            else
            {
                res.command = "null";
            }*/
            // [time].
            /*if (time != null)
            {
                res.time = int.Parse(time.Value);
                res.bitMask = res.bitMask | (1 << 5);
            }
            else
            {
                res.time = 1000;
            }*/
            // [volume].
            /*if (volume != null)
            {
                res.volume = float.Parse(volume.Value);
                res.bitMask = res.bitMask | (1 << 11);
            }
            else
            {
                res.volume = 100;
            }*/
            // [speed].
            /*if (speed != null)
            {
                res.speed = float.Parse(speed.Value);
                res.bitMask = res.bitMask | (1 << 12);
            }
            else
            {
                res.speed = 1;
            }*/
            // [size].
            /*if (size != null)
            {
                res.size = uint.Parse(size.Value);
                res.bitMask = res.bitMask | (1 << 6);
            }
            else
            {
                res.size = 1;
            }*/
            // [text].
            /*if (text != null)
            {
                res.text = text;
                res.bitMask = res.bitMask | (1 << 19);
            }
            else
            {
                res.text = "NO TEXT";
            }*/
            // [fontId].
            /*if (fontId != null)
            {
                res.fontId = fontId.Value;
                res.bitMask = res.bitMask | (1 << 22);
            }
            else
            {
                res.fontId = "standart";
            }*/
            // [color].
            /*if (color != null)
            {
                res.color = color.Value;
                res.bitMask = res.bitMask | (1 << 21);
            }
            else
            {
                res.color = "black";
            }*/

            return res;
        }
    }
}
