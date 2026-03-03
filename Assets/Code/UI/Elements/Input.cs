using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace UI.Elements {

    public class Input : ButtonBase <TMP_Text> {

        [SerializeField] private TMP_InputField _input;

        protected override bool AllowLeftClick => false;
        
        public             bool allowRightClick;
        protected override bool AllowRightClick => allowRightClick && !Selected;
        
        public UnityEvent onRightClick;
        protected override void OnRightClick () => onRightClick.Invoke ();


        private bool _selected;
        private bool Selected {
            get { return _selected; }
            set {
                if (!Interactable) return; // _interactable еще не выставлено при вызове из свойства
                _selected = _input.textComponent.enabled = value;
                Push ();
                UpdateColor ();
            }
        }


        public override bool Interactable {
            get { return base.Interactable; }
            set {
                DeselectInputField ();
                base.Interactable = value;
            }
        }


        public string Text {
            get { return _input.text; }
            set { _input.text = value; }
        }


        protected override void Awake () {
            base.Awake ();
            _input.onValueChanged.AddListener (UpdateText);
            _input.onEndEdit     .AddListener (_ => DeselectInputField ());
            _input.onSelect      .AddListener (_ => Selected = true);
            _input.onDeselect    .AddListener (_ => Selected = false);
            UpdateText (_input.text);
            _input.textComponent.enabled = false;
        }


        private void DeselectInputField () {
            var eventSystem = EventSystem.current;
            if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject (null);
        }


        private void UpdateText (string text) {
            bool endsWithSpace = text.Length > 0 && text [text.Length - 1] == ' ';
            Graphic.text = endsWithSpace ? text.Substring (0, text.Length - 1) + (char) 160 : text;
        }


        protected override void UpdateColor () {
            if (Selected) Graphic.color = Color.clear;
            else          base.UpdateColor ();
        }

    }

}
