using UnityEngine;
using UnityEngine.UI;


namespace UI {

    [RequireComponent (typeof (RectTransform))]
    public class Bar : MonoBehaviour {

        private RectTransform _rt;

        [SerializeField] private Image _filler;


        private void Awake () {
            _rt = (RectTransform) transform;
        }


        public float Value {
            get { return _rt.anchorMax.x; }
            set {
                var anchorMax = _rt.anchorMax;
                anchorMax.x   = value;
                _rt.anchorMax = anchorMax;
            }
        }


        public Color Color {
            get { return _filler.color; }
            set { _filler.color = value; }
        }

    }

}
