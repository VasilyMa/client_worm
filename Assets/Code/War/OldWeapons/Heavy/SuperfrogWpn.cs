using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Лягушка, которая летает и можно ей управлять.
     */
    public class SuperfrogWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("суперлягушка", desc => new SuperfrogWpn (desc), () => _.War.Assets.SuperfrogIcon);


        public SuperfrogWpn (WeaponDescriptor desc) : base (desc) {}

        
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