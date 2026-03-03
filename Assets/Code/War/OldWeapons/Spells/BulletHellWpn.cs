using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Делает много выстрелов с неба в направлении карты.
     */
    public class BulletHellWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("свинцовый дождь", desc => new BulletHellWpn (desc), () => _.War.Assets.BulletHellIcon);


        public BulletHellWpn (WeaponDescriptor desc) : base (desc) {}

        
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