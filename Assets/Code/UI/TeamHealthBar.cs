using TMPro;
using UnityEngine;


namespace UI {

    public class TeamHealthBar : MonoBehaviour {

        [SerializeField] private TMP_Text _name;
        [SerializeField] private Bar      _bar;
        private                  Color    _color;


        public string Name {
            get { return _name.text; }
            set { _name.text = value; }
        }


        public Color Color {
            get { return _color; }
            set { _name.color = _bar.Color = _color = value; }
        }

    }

}
