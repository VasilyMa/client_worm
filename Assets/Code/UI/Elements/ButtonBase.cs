using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UI.Elements {

    // базовый класс для нажимаемых элементов управления
    public class ButtonBase <T> : UiElement, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    where T : Graphic {

        [SerializeField] protected T Graphic;

        public Color NormalColor      = Colors.UIActive;
        public Color HighlightedColor = Colors.UISelected;
        public Color DisabledColor    = Colors.UIInactive;
        public Color ErrorColor       = Colors.UIError;


        private bool _underPointer;
        protected virtual bool UnderPointer {
            get { return _underPointer; }
            set {
                _underPointer = value;
                UpdateColor ();
                UpdateScale ();
            }
        }


        [SerializeField] private bool _interactable = true;
        public virtual bool Interactable {
            get { return _interactable; }
            set {
                _interactable = value;
                if (!value) StopCoroutine ();
                UpdateColor ();
                UpdateScale ();
            }
        }


        [SerializeField] private bool _errorFlag;
        public bool ErrorFlag {
            get { return _errorFlag; }
            set {
                _errorFlag = value;
                UpdateColor ();
            }
        }


        protected virtual bool AllowLeftClick  => true;
        protected virtual bool AllowRightClick => true;

        protected virtual void OnLeftClick  () {}
        protected virtual void OnRightClick () {}


        protected virtual void Awake () {
            UpdateColor ();
        }


        protected virtual void UpdateColor () {
            // для векторных изображений этот метод лагает
            Graphic.color =
            !_interactable ? DisabledColor :
            UnderPointer   ? HighlightedColor :
            ErrorFlag      ? ErrorColor : NormalColor;
        }


        public void OnPointerDown (PointerEventData eventData) {
            if (!Interactable) return;

            if (eventData.button == PointerEventData.InputButton.Left) {
                if (!AllowLeftClick) return;
                Push ();
                OnLeftClick ();
            }
            else if (eventData.button == PointerEventData.InputButton.Right) {
                if (!AllowRightClick) return;
                Push ();
                OnRightClick ();
            }
        }


        public void OnPointerEnter (PointerEventData eventData) => UnderPointer = true;
        public void OnPointerExit  (PointerEventData eventData) => UnderPointer = false;


        protected float Scale { get; private set; } = 1;
        protected override IEnumerator PushCoroutine () {
            Scale = 0.7f;
            while (Scale != 1) {
                yield return null;
                Scale = Mathf.MoveTowards (Scale, 1, 2 * Time.deltaTime);
                UpdateScale ();
            }
        }


        protected virtual void UpdateScale () {
            float scale = Interactable && UnderPointer ? Scale * 1.1f : Scale;
            Graphic.transform.localScale = new Vector3 (scale, scale, 1);
        }

    }

}
