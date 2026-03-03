using Core;
using UI.Elements;
using UnityEngine;


namespace UI.Panels {

    public class DonatePanel : SlidingPanel {

        [SerializeField] private Button _okButton;
        
        
        protected override void Activate () {
            _okButton.onLeftClick.AddListener (ClickedOk);
        }


        protected override void OnDestroy () {
            _okButton.onLeftClick.RemoveListener (ClickedOk);
        }


        private static void ClickedOk () => _.Menu.ShowMain ();

    }

}