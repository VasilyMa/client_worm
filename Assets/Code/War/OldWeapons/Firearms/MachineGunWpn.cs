using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Выпускает неточную очередь пуль в заданном направлении.
     */
    public class MachineGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("автомат", desc => new MachineGunWpn (desc), () => _.War.Assets.AssaultRifleIcon);


        public MachineGunWpn (WeaponDescriptor desc) : base (desc) {}
//            Shots        = 30;
//            ShotCooldown = 4;
//        }
//        
//        
//        public override void OnShot () {
//            var origin = Entity.Position + Worm.HeadOffset;
//            var offset =
//            (TurnData.XY - origin).WithLength (10000).Rotated ((_.OldWar.Random.Float () - _.OldWar.Random.Float ()) * 0.1f);
//            var collision = _.OldWar.World.CastRay (origin, offset);
//            
//            if (collision != null) {
//                var hit = origin + collision.Offset;
//                Explosives.Explode1 (hit);
//                var c2 = collision.Collider2;
//                if (c2 != null) {
//                    c2.Entity.TakeDamage (Balance.DamageBullet);
//                    c2.Entity.TakeBlast  (offset.WithLength (Balance.PushBullet));
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