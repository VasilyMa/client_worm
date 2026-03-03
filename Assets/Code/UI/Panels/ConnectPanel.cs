using Core;
using UI.Elements;
using UnityEngine;
using Input = UI.Elements.Input;


namespace UI.Panels {

    public class ConnectPanel : SlidingPanel {

        [SerializeField] private Input  _serverInput;
        [SerializeField] private Input  _idInput;
        [SerializeField] private Button _okButton;
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
