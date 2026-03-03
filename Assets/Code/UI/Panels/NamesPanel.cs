using Core;
using UI.Elements;
using UnityEngine;
using Input = UnityEngine.Input;


namespace UI.Panels {

    public class NamesPanel : SlidingPanel {

        [SerializeField] private Input [] _nameInputs;
        [SerializeField] private Button   _saveButton;
        [SerializeField] private Button   _resetButton;


        protected override void Activate () {
            _saveButton.onLeftClick.AddListener (SaveClicked);
        }


        protected override void OnDestroy () {
            _saveButton.onLeftClick.RemoveListener (SaveClicked);
        }


        private void SaveClicked () => _.Menu.ShowMain ();

    }

}