using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Позволяет сделать 2 выстрела за ход.
     */
    public class BlasterWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("бластер", desc => new BlasterWpn (desc), () => _.War.Assets.BlasterIcon);


        public BlasterWpn (WeaponDescriptor desc) : base (desc) {}
//            Attacks = 2;
//        }
//        
//        
//        public override void OnShot () {
//            var origin    = Entity.Position + Worm.HeadOffset;
//            var offset    = (TurnData.XY - origin).WithLength (10000);
//            var collision = _.OldWar.World.CastRay (origin, offset);
//            if (collision != null) {
//                Explosives.Explode15 (Entity.Position + collision.Offset);
//            }
//            if (Ammo == 0) Attacks = 0;
//        }


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