using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using War.Weapons;


//using War.OldWeapons;


namespace UI.Elements {

    public class WeaponButton : ButtonBase <Graphic> {

        [Flags] private enum DisableFlags { None = 0, Explicit = 1, FromAmmo = 2, FromDelay = 4 }
        private DisableFlags _flags;

        /* итак
         * кнопка оружия должна иметь следующие параметры
         *     иконка оружия
         *     доступность оружия - если недоступно, оно станет полупрозрачным
         *     количество оружия - если 0, то иконка не показывается, если бесконечное, то число не показывается
         *     задержка оружия - если не 0, то оружие нельзя выбрать
         * как это будет в терминах кнопки
         *     доступность - состояние Disabled
         *     количество - просто текстовое поле
         *     задержка - тоже
         */


        [SerializeField] private Transform _container;
        [SerializeField] private TMP_Text  _ammoText;
        [SerializeField] private TMP_Text  _delayText;
        [SerializeField] private Graphic   _selectionMark;

        public WeaponDescriptor Descriptor { get; private set; }


//        public event Action <WeaponDescriptor> OnClick;


        public int Ammo {
            set {
                _ammoText.text = value > 0 ? value.ToString () : "";
                if (value == 0) _flags |=  DisableFlags.FromAmmo;
                else            _flags &= ~DisableFlags.FromAmmo;
                UpdateAvailability ();
            }
        }


        public int Delay {
            set {
                _delayText.text = value > 0 ? value.ToString () : "";
                if (value == 0) _flags &= ~DisableFlags.FromDelay;
                else            _flags |=  DisableFlags.FromDelay;
                UpdateAvailability ();
            }
        }


        public bool Locked {
            set {
                if (value) _flags |=  DisableFlags.Explicit;
                else       _flags &= ~DisableFlags.Explicit;
                UpdateAvailability ();
            }
        }


        protected override bool AllowRightClick => false;


        protected override void Awake () {
//            base.Awake ();
        }


        private void UpdateAvailability () {
            Interactable    =  _flags == DisableFlags.None;
            Graphic.enabled = (_flags &  DisableFlags.FromAmmo) == 0;
        }


        protected override void OnLeftClick () {
            // будет выбрано то оружие на которое мы нажмем
//            _.War.OnWeaponButtonClick (_descriptor);
            _.War.InputSystem.SelectWeapon (Descriptor);
//            OnClick?.Invoke (_descriptor);
        }


        public void Init (WeaponDescriptor descriptor) {
            Descriptor = descriptor;
            var o = Instantiate (descriptor.Icon, _container, false);
            Graphic = o.GetComponent <Graphic> ();
            UpdateAvailability ();
        }


        protected override void UpdateScale () {
            float scale = Interactable && UnderPointer ? Scale * 1.1f : Scale;
            _container.localScale = new Vector3 (scale, scale, 1);
        }


        public void Highlight (bool value = true) {
             
        }

    }

}
