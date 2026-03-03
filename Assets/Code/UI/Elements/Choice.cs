using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace UI.Elements {

    // выбор из нескольких вариантов
    // по сути та же кнопка у которой клик переключает варианты
    // должна иметь состояние ошибки, возможность зациклить варианты
    public class Choice : ButtonBase <TMP_Text> {

        [SerializeField] private string [] _options;
        [SerializeField] private int       _selectedOption;

        public bool Cycled = true;


        [Serializable]
        public class ValueChangedEvent : UnityEvent <int> {}
        public ValueChangedEvent OnValueChanged;
        
        
        public int SelectedOption {
            get { return _selectedOption; }
            set {
                _selectedOption = value;
                Graphic.text = _options [value];
                OnValueChanged.Invoke (value);
            }
        }


        protected override void Awake () {
            base.Awake ();
            Graphic.text = _options [_selectedOption];
        }


        protected override bool AllowLeftClick  => Cycled || SelectedOption < _options.Length - 1;
        protected override bool AllowRightClick => Cycled || SelectedOption > 0;


        protected override void OnLeftClick () {
            if (SelectedOption < _options.Length - 1) SelectedOption++;
            else                                      SelectedOption = 0;
        }


        protected override void OnRightClick () {
            if (SelectedOption > 0) SelectedOption--;
            else                    SelectedOption = _options.Length - 1;
        }

    }

}
