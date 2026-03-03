using System;
using UnityEngine;


namespace War.OldWeapons {

    public sealed class WeaponDescriptor {

        private int _id;
        public int Id {
            get { return _id; }
            set {
                if (value == 0) throw new ArgumentException         ("0 зарезервирован для отсутствия оружия.");
                if (_id   != 0) throw new InvalidOperationException ("Нельзя два раза инициализировать id."   );
                _id = value;
            }
        }

        public readonly string Name, NameLowerCase;

        private Func <WeaponDescriptor, Weapon> _weapon;
        public Weapon MakeWeapon () => _weapon (this);

        private Func <GameObject> _icon;
        public GameObject Icon => _icon ();


        public WeaponDescriptor (
            string                          nameLowerCase,
            Func <WeaponDescriptor, Weapon> fWeapon,
            Func <GameObject>               fIcon
        ) {
            NameLowerCase = nameLowerCase;
            var chars     = nameLowerCase.ToCharArray ();
            chars [0]     = char.ToUpper (chars [0]);
            Name          = new string (chars);
            _weapon       = fWeapon;
            _icon         = fIcon;
        }

    }

}
