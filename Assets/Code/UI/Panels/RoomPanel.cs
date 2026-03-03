using Core;
using UI.Elements;
using UnityEngine;


namespace UI.Panels {

    public class RoomPanel : SlidingPanel {

        [SerializeField] private Button _cancelButton;


        protected override void Activate () {
            _cancelButton.onLeftClick.AddListener (CancelClicked);
        }


        protected override void OnDestroy () {
            _cancelButton.onLeftClick.RemoveListener (CancelClicked);
        }


        private void CancelClicked () => _.Menu.ShowMain ();

    }

}