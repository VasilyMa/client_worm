using TMPro;
using UnityEngine;


namespace UI.Elements {

    public class WeaponInfo : MonoBehaviour {

        [SerializeField] private RectTransform _container;
        [SerializeField] private TMP_Text      _text;
        [SerializeField] private RectTransform _bar;


        private float _declaredAt;
        private float _hiddenAt = float.NegativeInfinity;
        

        private void Update () {
            float delta       = Time.time - _declaredAt;
            float deltaHide   = Time.time - _hiddenAt;

            const float text0 = 0;
            const float text1 = 0.5f;
            const float bar0  = 0.5f;
            const float bar1  = 1;
            const float move0 = 0.75f;
            const float move1 = 2;
            
            const float hide0 = 0;
            const float hide1 = 0.5f;

            // начальная фаза: альфа и размер
            float invLerp     = Mathf.InverseLerp (text0, text1, delta);
            float invLerpHide = Mathf.InverseLerp (hide0, hide1, deltaHide);
            float alpha = invLerp * invLerp;
            alpha *= Mathf.Pow (1 - invLerpHide, 2); // когда скрываем элемент плавно убираем альфу
            _text.color = new Color (1, 1, 1, alpha);
            float size = Mathf.Lerp (4, 1, invLerp);
            _text.transform.localScale = new Vector3 (size, size, size);
            
            // фаза с вылезающей полоской
            invLerp = Mathf.InverseLerp (bar0, bar1, delta);
            size = 1 - Mathf.Pow (1 - invLerp, 2);
            size *= Mathf.Pow (1 - invLerpHide, 2); // опять же когда скрываем
            _bar.anchorMin = new Vector2 (1 - size, 0.5f);
            
            // фигня поднимается наверх
            invLerp = Mathf.InverseLerp (move0, move1, delta); // [0, 1]
            float t = invLerp - 0.5f;                          // [-0.5, 0.5]
            t = Mathf.Sin (Mathf.PI * t) * 0.5f;               // [-0.5, 0.5]
            t = Mathf.Sin (Mathf.PI * t) * 0.5f;               // [-0.5, 0.5]
            float pos = t + 0.5f;                              // [0, 1]
            _container.anchorMin = new Vector2 (0, pos);
            _container.anchorMax = new Vector2 (1, pos);
        }


        public void Declare (string weapon) {
            _text.text  = weapon;
            _declaredAt = Time.time;
            _hiddenAt   = float.PositiveInfinity;
        }


        public void Hide () {
            // скрываем оружие только если его перед этим показали
            // иначе надпись выскочит когда ее не было
            if (float.IsPositiveInfinity (_hiddenAt)) _hiddenAt = Time.time;
        }

    }

}