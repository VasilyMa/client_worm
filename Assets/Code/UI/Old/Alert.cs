using UnityEngine;
using UnityEngine.UI;


namespace UI.Old {

    public class Alert : MonoBehaviour {

        [SerializeField] private Text        _text;
        [SerializeField] private Image       _image;
        [SerializeField] private Material [] _materials;


        private float _currentAlpha;


        private float CurrentAlpha {
            get { return _currentAlpha; }
            set {
                _currentAlpha = value;
                foreach (var material in _materials) {
                    material.SetFloat ("_Alpha", value);
                }
            }
        }


        [Range (0, 1)] public float Alpha;


        public string Text {
            get { return _text.text; }
            set { _text.text = value; }
        }


        public Sprite Symbol {
            get { return _image.sprite; }
            set { _image.sprite = value; }
        }


        private void Awake () {
            CurrentAlpha = Alpha;
        }


        private void Update () {
            if (CurrentAlpha == Alpha) return;
            CurrentAlpha = Mathf.MoveTowards (CurrentAlpha, Alpha, 0.03f);
        }

    }

}