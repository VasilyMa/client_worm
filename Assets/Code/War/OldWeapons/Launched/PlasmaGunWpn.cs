using System;
using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Launched {

    [Obsolete]
    public class PlasmaGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("плазма", desc => new PlasmaGunWpn (desc), () => _.War.Assets.PlasmaGunIcon);


        public PlasmaGunWpn (WeaponDescriptor desc) : base (desc) {}

        
        public override void OnEquip () {
            throw new NotImplementedException ();
        }


        public override void OnDraw () {
            throw new NotImplementedException ();
        }


        public override void Update (TurnData td) {
            throw new NotImplementedException ();
        }


        public override void OnInterrupt () {
            throw new NotImplementedException ();
        }


        public override void OnUnequip () {
            throw new NotImplementedException ();
        }

    }

}