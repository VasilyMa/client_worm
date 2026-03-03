using UnityEngine;


namespace UI.Panels {

    public class SlidingPanel : Panel {

        public Vector2 AnchorOpen;
        public Vector2 AnchorClosed;
        public Vector2 PositionOpen;
        public Vector2 PositionClosed;
        public Vector2 PivotOpen;
        public Vector2 PivotClosed;


        protected override void Adjust () {
            var rt = (RectTransform) transform;

            rt.anchorMin        =
            rt.anchorMax        = Vector2.Lerp (AnchorClosed,   AnchorOpen,   _openness);
            rt.anchoredPosition = Vector2.Lerp (PositionClosed, PositionOpen, _openness);
            rt.pivot            = Vector2.Lerp (PivotClosed,    PivotOpen,    _openness);
        }

    }

}
