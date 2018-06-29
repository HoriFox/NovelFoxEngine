using UnityEngine;

namespace ng
{
    public class Sprite : IDisplayable
    {
        public GameObject objectSprite;

        public void Edit(Data data)
        {
            // Слой.
            if (Data.GetBit(data.bitMask, (int)OA.layer))
            {
                // Z: 100...3400
                float specialZ = (float)(3.4 - ((data.layer + 1000) / 10000.0));
                m_rt.position = new Vector3(m_rt.position.x, m_rt.position.y, specialZ);
                m_sr.sortingOrder = data.layer;
            }
            // X позиция.
            if (Data.GetBit(data.bitMask, (int)OA.x))
            {
                m_rt.offsetMin = new Vector2(data.x, m_rt.offsetMin.y); // new Vector2(left, bottom);
                m_rt.offsetMax = new Vector2(data.x, m_rt.offsetMin.y); // new Vector2(-right, -top);
            }
            // Y позиция.
            if (Data.GetBit(data.bitMask, (int)OA.y))
            {
                m_rt.offsetMin = new Vector2(m_rt.offsetMin.x, data.y); // new Vector2(left, bottom);
                m_rt.offsetMax = new Vector2(m_rt.offsetMin.x, data.y); // new Vector2(-right, -top);
            }
            // Видимость.
            if (Data.GetBit(data.bitMask, (int)OA.visible))
            {
                objectSprite.SetActive(data.visible);
            }
            // Размер.
            if (Data.GetBit(data.bitMask, (int)OA.scale))
            {
                m_rt.localScale = new Vector3(data.scale, data.scale);
            }
            // src.
            if (Data.GetBit(data.bitMask, (int)OA.src))
            {
                m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
                // Изменение collider-а под нужное src.
                m_rect = objectSprite.GetComponent<SpriteRenderer>().sprite.textureRect;
                m_colliderBox.size = new Vector2(m_rect.width / 100, m_rect.height / 100);
            }
            // Угол поворота.
            if (Data.GetBit(data.bitMask, (int)OA.angle))
            {
                m_rt.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));
            }
        }
        public Sprite(Data data, GameObject pre, Transform canvastr)
        {
            objectSprite = GameObject.Instantiate(pre, canvastr);
            objectSprite.name = data.id;
            objectSprite.SetActive(data.visible);

            m_sr = objectSprite.GetComponent<SpriteRenderer>();
            m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
            m_sr.sortingOrder = data.layer;

            m_rt = objectSprite.GetComponent<RectTransform>();
            // Z: 100...3400
            float specialZ = (float)(3.4 - ((data.layer + 1000) / 10000.0));
            m_rt.position = new Vector3(m_rt.position.x, m_rt.position.y, specialZ);

            m_rt.offsetMin = new Vector2(data.x, data.y); // new Vector2(left, bottom);
            m_rt.offsetMax = new Vector2(data.x, data.y); // new Vector2(-right, -top);

            m_rt.localScale = new Vector3(data.scale, data.scale);
            m_rt.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));


            SetCollider2D(data.scale);
        }
        // Быстрое "натягивание" collider-а на любой объект.
        private void SetCollider2D(float scale)
        {
            m_rect = objectSprite.GetComponent<SpriteRenderer>().sprite.textureRect;
            m_colliderBox = objectSprite.GetComponent<BoxCollider2D>() as BoxCollider2D;
            m_colliderBox.size = new Vector2(m_rect.width / 100, m_rect.height / 100);
        }

        private BoxCollider2D m_colliderBox;
        private Rect m_rect;
        private SpriteRenderer m_sr;
        private RectTransform m_rt;
    };
}
