using UnityEngine;


namespace UI.Panels {

    public abstract class Panel : MonoBehaviour {

        protected float       _openness;
        private   CanvasGroup _canvasGroup;
        
        public    bool        Open;
        public    float       MinOpenness = -1;
        public    float       MaxOpenness = 1;
        public    float       Speed       = 5;


        // ReSharper disable once Unity.NoNullCoalescing
        // Я не инициализирую это в эвейке потому что к методу Show имеют доступ другие эвейки
        private CanvasGroup CanvasGroup => _canvasGroup = _canvasGroup ?? GetComponent <CanvasGroup> ();


        protected void Awake () {
            CanvasGroup.blocksRaycasts = Open;
            _openness = Open ? MaxOpenness : MinOpenness;
            Adjust ();
            Activate ();
        }


        private void Update () {
            OnUpdate ();
            if (Open) {
                if (_openness >= MaxOpenness) return;
                _openness = Mathf.Min (MaxOpenness, _openness + Time.deltaTime * Speed);
            }
            else {
                if (_openness <= MinOpenness) return;
                _openness = Mathf.Max (MinOpenness, _openness - Time.deltaTime * Speed);
            }
            Adjust ();
        }


        protected virtual void Activate  () {}

        protected virtual void OnDestroy () {}

        protected virtual void OnUpdate  () {}


        public void Show (bool instantly = false) {
            CanvasGroup.blocksRaycasts =
            Open = true;
            if (!instantly) return;
            _openness = MaxOpenness;
            Adjust ();
        }


        public void Hide (bool instantly = false) {
            CanvasGroup.blocksRaycasts =
            Open = false;
            if (!instantly) return;
            _openness = MinOpenness;
            Adjust ();
        }


        public void Toggle (bool instantly = false) {
            CanvasGroup.blocksRaycasts =
            Open = !Open;
            if (!instantly) return;
            _openness = Open ? MaxOpenness : MinOpenness;
            Adjust ();
        }


        protected abstract void Adjust ();

    }

}