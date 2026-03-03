using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет цепляться за землю и подтягиваться. Не тратит ход.
     */
    public class RopeWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("веревка", desc => new RopeWpn (desc), () => _.War.Assets.RopeIcon);


        public RopeWpn (WeaponDescriptor desc) : base (desc) {}

        
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