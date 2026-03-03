using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет мгновенно переместиться в указанную точку.
     */
    public class TeleportWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("телепорт", desc => new TeleportWpn (desc), () => _.War.Assets.TeleportIcon);


        public TeleportWpn (WeaponDescriptor desc) : base (desc) {}

        
        public override void OnEquip () {
            throw new System.NotImplementedException ();
        }


        public override void OnDraw () {
            throw new System.NotImplementedException ();
        }


        public override void Update (TurnData td) {
            throw new System.NotImplementedException ();
        }


        public override void OnInterrupt () {
            throw new System.NotImplementedException ();
        }


        public override void OnUnequip () {
            throw new System.NotImplementedException ();
        }

    }

}