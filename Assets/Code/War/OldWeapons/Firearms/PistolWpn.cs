using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Позволяет сделать 5 точных, но слабых выстрелов.
     */
    public class PistolWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("пистолет", desc => new PistolWpn (desc), () => _.War.Assets.PistolIcon);


        public PistolWpn (WeaponDescriptor desc) : base (desc) {}
//            Shots        = 5;
//            ShotCooldown = Time.Seconds (1);
//        }
//        
//        
//        public override void OnShot () {
//            var origin    = Entity.Position + Worm.HeadOffset;
//            var offset    = (TurnData.XY - origin).WithLength (10000);
//            var collision = _.OldWar.World.CastRay (origin, offset);
//            if (collision != null) {
//                var hit = origin + collision.Offset;
//                Explosives.Explode1 (hit);
//                var c2 = collision.Collider2;
//                if (c2 != null) {
//                    c2.Entity.TakeDamage (Balance.DamageBulletHeavy);
//                    c2.Entity.TakeBlast  (offset.WithLength (Balance.PushBulletHeavy));
//                }
//                else {
//                    _.OldWar.World.DestroyTerrain (hit, Balance.HoleBullet);
//                }
//            }
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