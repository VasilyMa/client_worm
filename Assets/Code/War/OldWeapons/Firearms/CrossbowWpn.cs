using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Наносит урон и отравляет цель. 2 выстрела за ход.
     */
    public class CrossbowWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("арбалет", desc => new CrossbowWpn (desc), () => _.War.Assets.CrossbowIcon);


        public CrossbowWpn (WeaponDescriptor desc) : base (desc) {}
//            Attacks = 2;
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
//                    var e = c2.Entity;
//                    e.TakeDamage (Balance.DamageCrossbowBolt);
//                    e.TakeBlast  (offset.WithLength (Balance.PushBulletHeavy));
//                    (e as Worm)?.TakePoison (Balance.PoisonCrossbowBolt, true);
//                }
//                else {
//                    _.OldWar.World.DestroyTerrain (hit, Balance.HoleBullet);
//                }
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