using System.Collections.Generic;
using UnityEngine;

namespace ng
{
    public interface IDisplayable
    {
        void Edit(ResData data);
    };

    public class Scene // Перенести создание в Scene
    {
        public int orderLayer = 0;                                      // Позиция в порядке слоёв
        public SortedDictionary<string, IDisplayable> objects;          // Dictionary с объектами
        //public SortedDictionary<string, Sound> sounds;                // Dictionary с звуков
        //public SortedDictionary<string, Music> musics;                // Dictionary с музыкой

        public void CreateObject(int type, ResData data, Transform canvastr)
        {
            orderLayer += 1;
            data.layer += orderLayer;
            switch (type)
            {
                case 1:
                    objects[data.id] = new Sprite(data, ResourceManager.GetPrefab("spritePrefab"), canvastr);
                    break;
                case 2:
                    objects[data.id] = new AnimateSprite(data, ResourceManager.GetPrefab("spritePrefab"), canvastr);
                    break;
                case 3:
                    objects[data.id] = new Video(data, ResourceManager.GetPrefab("spritePrefab"), canvastr);
                    break;
                case 4:
                    objects[data.id] = new Text(data, ResourceManager.GetPrefab("spritePrefab"), canvastr);
                    break;

            }
        }

        public void EditObject(ResData data)
        {
            objects[data.id].Edit(data);
        }

        public Scene()
        {
            objects = new SortedDictionary<string, IDisplayable>();
            /*sounds = new SortedDictionary<string, Sound>();
            musics = new SortedDictionary<string, Music>();*/
        }
    };
}
