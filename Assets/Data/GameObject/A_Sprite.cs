using UnityEngine;

namespace ng
{
    public class Sprite : IDisplayable
    {
        public GameObject objectSprite;

        public void Edit(Data data)
        {
            // Слой.
            if (DataObject.GetBit(data.m_bitMask, 1))
            {
                m_sr.sortingOrder = data.Layer;
            }
            // X позиция.
            if (DataObject.GetBit(data.m_bitMask, 7))
            {
                m_tr.position = new Vector3(data.X, m_tr.position.y);
            }
            // Y позиция.
            if (DataObject.GetBit(data.m_bitMask, 8))
            {
                m_tr.position = new Vector3(m_tr.position.x, data.Y);
            }
            // Видимость.
            if (DataObject.GetBit(data.m_bitMask, 15))
            {
                objectSprite.SetActive(data.Visible);
            }
            // Размер.
            if (DataObject.GetBit(data.m_bitMask, 9))
            {
                m_tr.localScale = new Vector3(data.Scale, data.Scale);
            }
            // src.
            if (DataObject.GetBit(data.m_bitMask, 18))
            {
                m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.Src);
                // Изменение collider-а под нужное src.
                m_boundSide = objectSprite.GetComponent<SpriteRenderer>().bounds.size;
                m_colliderBox.size = m_boundSide;
            }
            // Угол поворота.
            if (DataObject.GetBit(data.m_bitMask, 10))
            {
                m_tr.rotation = Quaternion.AngleAxis(data.Angle, new Vector3(0f, 0f, 1f));
            }
        }
        public Sprite(Data data, GameObject pre, Transform canvastr)
        {
            objectSprite = GameObject.Instantiate(pre, canvastr);
            objectSprite.name = data.Id;
            objectSprite.SetActive(data.Visible);

            m_tr = objectSprite.GetComponent<Transform>();
            m_tr.position = new Vector3(data.X, data.Y);
            m_tr.localScale = new Vector3(data.Scale, data.Scale);
            m_tr.rotation = Quaternion.AngleAxis(data.Angle, new Vector3(0f, 0f, 1f));

            m_sr = objectSprite.GetComponent<SpriteRenderer>();
            m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.Src);
            m_sr.sortingOrder = data.Layer;

            // Быстрое "натягивание" collider-а на любой объект.
            m_boundSide = objectSprite.GetComponent<SpriteRenderer>().bounds.size;
            m_colliderBox = objectSprite.GetComponent<BoxCollider2D>() as BoxCollider2D;
            m_colliderBox.size = m_boundSide;
        }

        private BoxCollider2D m_colliderBox;
        private Transform m_canvastr;
        private Vector3 m_boundSide;
        private SpriteRenderer m_sr;
        private Transform m_tr;
    };
}
