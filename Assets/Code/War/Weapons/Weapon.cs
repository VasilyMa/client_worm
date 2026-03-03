using Core;
using DataTransfer.Data;
using War.Entities;


namespace War.Weapons {

    // базовый класс для всех оружий, только интерфейсные методы
    public class Weapon {
        
        public Worm Worm;

        protected WeaponDescriptor _descriptor;
        private   bool             _declared;


        protected Weapon (WeaponDescriptor desc) {
            _descriptor = desc;
        }


        // настоящая инициализация, когда оружие выбрано в арсенале
        public virtual void OnEquip (TurnData td) {}
        

        // происходит при вставании червяка, если его толкали
        public virtual void OnReequip () {}


        // это когда он его убирает мгновенно
        // только при выборе другого оружия включается или когда заканчивается анимация убирания
        public virtual void OnUnequip () {}


        // включается анимация убирания
        public virtual void OnSlowUnequip () {
            Worm.UnequipWeapon ();
        }


        // вызывается только когда оружие у червяка в руках
        public virtual void Update (TurnData td) 
        {

        }


        protected void Declare () {
            _.War.WeaponInfo.Declare (_descriptor.Name);
            _.War.WeaponSystem.WeaponsLocked = true;
        }


        protected void UseAmmo (int ammo = 1) {
            // todo Worm.Team.Arsenal.UseAmmo (_descriptor, ammo);
        }


        // todo отделить поворот головы от поворота оружия так как мы можем поворачивать например прицел
        // protected static void Aim (float angle, Transform weapon) {
            //// Worm.Sprite.HeadRotation = angle;
            // bool flip = Mathf.RoundToInt (angle / Mathf.PI) % 2 != 0;
            // weapon.localScale        = new Vector3 (1, flip ? -1 : 1, 1);
            // weapon.localRotation     = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
        // }

    }

}