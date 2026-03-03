using System;
using War.Weapons;


namespace War.Arsenals {

    public class Arsenal {

        public event Action <int, int> OnAmmoChanged;
        private int [] _ammo = new int [256]; // = new int[Enum.GetValues (typeof (WeaponId)).Length];


        public int this [int id] {
            get { return _ammo [id]; }
            protected set {
                _ammo [id] = value;
                OnAmmoChanged?.Invoke (id, value);
            }
        }
        public int this [WeaponDescriptor weapon] {
            get           { return this [weapon.Id];  }
            protected set { this [weapon.Id] = value; }
        }
        public int this [OldWeapons.WeaponDescriptor weapon] { // todo убрать
            get           { return this [weapon.Id];  }
            protected set { this [weapon.Id] = value; }
        }


        public void UseAmmo (int id, int ammo = 1) {
            if (this [id] < 0) return; // бесконечное
            if (this [id] < ammo) {
                throw new InvalidOperationException ("У игрока нет оружия, которое он использует!");
            }
            this [id] -= ammo;
        }
        public void UseAmmo (WeaponDescriptor weapon, int ammo = 1) => UseAmmo (weapon.Id, ammo);
        public void UseAmmo (OldWeapons.WeaponDescriptor weapon, int ammo = 1) => UseAmmo (weapon.Id, ammo); // todo убрать


        public void AddAmmo (int id, int ammo) {
            if (this [id] < 0) return; // и так бесконечное
            if (ammo < 0) this [id] =  -1;
            else          this [id] += ammo;
        }
        public void AddAmmo (WeaponDescriptor weapon, int ammo = 1) => AddAmmo (weapon.Id, ammo);
        public void AddAmmo (OldWeapons.WeaponDescriptor weapon, int ammo = 1) => AddAmmo (weapon.Id, ammo); // todo убрать

    }

}
