using UnityEngine;

namespace ng
{
    public class Sprite : IDisplayable
    {
        public GameObject objectSprite;

        public void Edit(ResData data)
        {
            // Слой.
            if (DataReader.GetBit(data.bitMask, 1))
            {
                m_sr.sortingOrder = data.layer;
            }
            // X позиция.
            if (DataReader.GetBit(data.bitMask, 7))
            {
                m_tr.position = new Vector3(data.x, m_tr.position.y);
            }
            // Y позиция.
            if (DataReader.GetBit(data.bitMask, 8))
            {
                m_tr.position = new Vector3(m_tr.position.x, data.y);
            }
            // Видимость.
            if (DataReader.GetBit(data.bitMask, 15))
            {
                objectSprite.SetActive(data.visible);
            }
            // Размер.
            if (DataReader.GetBit(data.bitMask, 9))
            {
                m_tr.localScale = new Vector3(data.scale, data.scale);
            }
            // src.
            if (DataReader.GetBit(data.bitMask, 18))
            {
                m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
                // Изменение collider-а под нужное src.
                m_boundSide = objectSprite.GetComponent<SpriteRenderer>().bounds.size;
                m_colliderBox.size = m_boundSide;
            }
            // Угол поворота.
            if (DataReader.GetBit(data.bitMask, 10))
            {
                m_tr.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));
            }
        }
        public Sprite(ResData data, GameObject pre, Transform canvastr)
        {
            objectSprite = GameObject.Instantiate(pre, canvastr);
            objectSprite.name = data.id;
            objectSprite.SetActive(data.visible);

            m_tr = objectSprite.GetComponent<Transform>();
            m_tr.position = new Vector3(data.x, data.y);
            m_tr.localScale = new Vector3(data.scale, data.scale);
            m_tr.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));

            m_sr = objectSprite.GetComponent<SpriteRenderer>();
            m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
            m_sr.sortingOrder = data.layer;

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
