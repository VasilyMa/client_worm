using UnityEngine;


namespace War.Indication {

    public class LineCrosshair : MonoBehaviour {

        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private GameObject _ringPrefab;
        private                  GameObject _ring;
        [SerializeField] private int        _dots;
        [SerializeField] private float      _dotsStart;
        [SerializeField] private float      _dotsStep;


        private void Awake () {
            for (int i = 0; i < _dots; i++) {
                float x = _dotsStart + i * _dotsStep;
                var dot = Instantiate (_dotPrefab, new Vector3 (x, 0), Quaternion.identity);
                dot.transform.SetParent (transform, false);
            }
            _ring = Instantiate (_ringPrefab, new Vector3 (_dotsStart, 0), Quaternion.identity);
            _ring.transform.SetParent (transform, false);
            _ring.SetActive (false);
        }


        public bool RingVisible {
            set { _ring.SetActive(value); }
        }


        public float RingPosition {
            set {
                var position = _ring.transform.localPosition;
                position.x = _dotsStart + value * (_dots - 1) * _dotsStep;
                _ring.transform.localPosition = position;
            }
        }


        public float Angle {
            set { gameObject.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, value * Mathf.Rad2Deg)); }
        }

    }

}