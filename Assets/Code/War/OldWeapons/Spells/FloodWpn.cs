using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Заставляет уровень воды на карте подняться выше.
     */
    public class FloodWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("наводнение", desc => new FloodWpn (desc), () => _.War.Assets.FloodIcon);


        public FloodWpn (WeaponDescriptor desc) : base (desc) {}

        
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