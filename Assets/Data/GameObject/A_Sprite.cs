using UnityEngine;

namespace ng
{
    public class Sprite : IDisplayable
    {
        public GameObject objectSprite;

        public void Edit(Data data)
        {
            // Слой.
            if (DataObject.GetBit(data.bitMask, (int)OA.layer))
            {
                m_sr.sortingOrder = data.layer;
            }
            // X позиция.
            if (DataObject.GetBit(data.bitMask, (int)OA.x))
            {
                m_tr.position = new Vector3(data.x, m_tr.position.y);
            }
            // Y позиция.
            if (DataObject.GetBit(data.bitMask, (int)OA.y))
            {
                m_tr.position = new Vector3(m_tr.position.x, data.y);
            }
            // Видимость.
            if (DataObject.GetBit(data.bitMask, (int)OA.visible))
            {
                objectSprite.SetActive(data.visible);
            }
            // Размер.
            if (DataObject.GetBit(data.bitMask, (int)OA.scale))
            {
                m_tr.localScale = new Vector3(data.scale, data.scale);
            }
            // src.
            if (DataObject.GetBit(data.bitMask, (int)OA.src))
            {
                m_sr.sprite = Resources.Load<UnityEngine.Sprite>("Scenario/Textures/" + data.src);
                // Изменение collider-а под нужное src.
                m_boundSide = objectSprite.GetComponent<SpriteRenderer>().bounds.size;
                m_colliderBox.size = m_boundSide;
            }
            // Угол поворота.
            if (DataObject.GetBit(data.bitMask, (int)OA.angle))
            {
                m_tr.rotation = Quaternion.AngleAxis(data.angle, new Vector3(0f, 0f, 1f));
            }
        }
        public Sprite(Data data, GameObject pre, Transform canvastr)
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
