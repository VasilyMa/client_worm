using Core;
using DataTransfer.Data;
using UnityEngine;
using Worm = War.Entities.Worm;


namespace War.OldWeapons2 {

    // хоть оружие и сделано компонентом, но инициализация
    // будет происходить при выборе а не при прикреплении к червю
    public class Weapon {
        
        public Worm Worm;

        private WeaponDescriptor _descriptor;
        protected WeaponState State;
        public int DrawnAt { get; private set; } // момент доставания оружия, нужен для анимации


        protected Weapon (WeaponDescriptor desc) {
            _descriptor = desc;
        }


        // настоящая инициализация, когда оружие выбрано в арсенале
        public virtual void OnSelect (TurnData td) {
            var worm = _.War.ActiveWorm;
            if (worm.CanUseWeapon) {
                // worm.EquipWeapon (this, td); // это td передастся в OnAttach
            }
        }


        // финализация - когда оружие перестает быть выбранным
        // оно может еще какое-то время висеть на червяке
        public virtual void OnDeselect () {
            Worm.UnequipWeapon ();
        }
        

        // это типа когда червяк оружие достает
        // происходит при выборе оружия или при вставании червяка
        // вызывается из метода Worm.EquipWeapon
        public virtual void OnEquip (TurnData td) {
            DrawnAt = _.War.Time;
        }


        // это когда он его убирает
        // оружие может само инициировать это действие или червяка сбивают с ног
        // вызывается из метода Worm.EquipWeapon или Worm.UnequipWeapon
        public virtual void OnUnequip () {
            State = WeaponState.Invalid;
        }


        // вызывается только когда оружие у червяка в руках
        public virtual void Update (TurnData td) {
            
        }


        protected void Declare () {
            _.War.WeaponInfo.Declare (_descriptor.Name);
            _.War.WeaponSystem.WeaponsLocked = true;
        }


        // todo отделить поворот головы от поворота оружия так как мы можем поворачивать например прицел
        protected static void Aim (float angle, Transform weapon) {
            // Worm.Sprite.HeadRotation = angle;
            bool flip = Mathf.RoundToInt (angle / Mathf.PI) % 2 != 0;
            weapon.localScale        = new Vector3 (1, flip ? -1 : 1, 1);
            weapon.localRotation     = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
        }

    }

}
