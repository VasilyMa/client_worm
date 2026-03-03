using UnityEngine;


namespace UI {
    
    public class WindBar : MonoBehaviour {

        private const int Step = 40;

        [SerializeField] private GameObject    _arrowPrefab;
        [SerializeField] private RectTransform _mask;

        private RectTransform [] _arrows;

        private float _wind;
        [Range (-6, 6)] public float Wind;


        private void Awake () {
            _arrows = new RectTransform[11];
            for (int i = 0; i < 11; i++) {
                _arrows [i] = (RectTransform) Instantiate (_arrowPrefab, _mask, false).transform;
            }
        }


        private float CurrentWind {
            get { return _wind; }
            set {
                _wind = value;
                if (value < 0) {
                    _mask.anchorMin        =
                    _mask.anchorMax        = new Vector2 (1, 0.5f);
                    _mask.localScale       = new Vector3 (-1, 1, 1);
                    _mask.anchoredPosition = new Vector2 (-10, 0);
                }
                else {
                    _mask.anchorMin        =
                    _mask.anchorMax        = new Vector2 (0, 0.5f);
                    _mask.localScale       = new Vector3 (1, 1, 1);
                    _mask.anchoredPosition = new Vector2 (10, 0);
                }
                value           = Mathf.Abs (value);
                _mask.sizeDelta = new Vector2 (value * Step, value * Step);
            }
        }


        private void Update () {
            for (int i = 0; i < _arrows.Length; i++) {
                _arrows [i].anchoredPosition = new Vector2 ((i - 1) * Step + Mathf.Repeat (Time.time * 100, Step), 0);
            }
            CurrentWind = Mathf.MoveTowards (_wind, Wind, 12 * Time.deltaTime);
        }

    }

}
