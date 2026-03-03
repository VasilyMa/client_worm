using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Крот, копанием которого можно управлять.
     */
    public class MoleWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("крот", desc => new MoleWpn (desc), () => _.War.Assets.MoleIcon);


        public MoleWpn (WeaponDescriptor desc) : base (desc) {}

        
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