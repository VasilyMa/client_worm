using System;
using Core;
using DataTransfer.Data;
using War.Components;
using War.Entities;


namespace War.OldWeapons {

    
    [Obsolete]
    public abstract class OldWeapon : Component <Worm> {

        public readonly WeaponDescriptor Descriptor;


        protected OldWeapon (WeaponDescriptor desc) {
            Descriptor = desc;
        }


        protected void Declare () {
            _.War.WeaponInfo.Declare (Descriptor.Name);
        }


        protected void UseAmmo (int ammo = 1) {
            Entity.Team.Arsenal.UseAmmo (Descriptor, ammo);
        }


        protected int Ammo => Entity.Team.Arsenal [Descriptor];


        public abstract void OnEquip ();
        public abstract void OnUnequip ();
        public abstract void Update (TurnData td);

    }

}
