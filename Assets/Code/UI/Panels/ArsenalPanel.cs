using System.Collections.Generic;
using UI.Elements;
using UnityEngine;
using War.Arsenals;
using War.Weapons;

// using War.OldWeapons2;


namespace UI.Panels {

    public class ArsenalPanel : SlidingPanel {

        [SerializeField] private RectTransform _container;
        [SerializeField] private GameObject    _buttonPrefab;

        private List <WeaponButton> _buttons = new List <WeaponButton> {null};
        private WeaponButton _selectedButton;


        public void Init () {
            var weapons = WeaponDescriptors.All;
            for (int i = 1; i < weapons.Count; i++) {
                var button = Instantiate (_buttonPrefab, _container, false).GetComponent <WeaponButton> ();
                button.Init (weapons [i]);
                _buttons.Add (button);
            }
        }


        public void Track (Arsenal arsenal) {
            foreach (var button in _buttons) {
                if (button == null) continue;
                button.Ammo  = arsenal [button.Descriptor.Id];
                button.Delay = 0;
            }
        }


        public void Highlight (int weapon) {
            // ReSharper disable once Unity.NoNullPropagation
            _selectedButton?.Highlight (false);
            if (weapon > 0 && weapon < _buttons.Count) {
                _selectedButton = _buttons [weapon];
                _selectedButton.Highlight ();
            }
            else {
                _selectedButton = null;
            }
        }

    }

}
