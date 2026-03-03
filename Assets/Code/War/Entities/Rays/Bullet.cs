using System.Collections.Generic;
using Core;
using Math;
using War.Components;
using War.Systems.Blasts;
using War.Systems.Damage;


namespace War.Entities.Rays {

    public class Bullet : Ray {

        private XY _o, _v;


        public Bullet (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            var c = Cast (_v);
            if (c != null) {
                if (c.IsLandCollision) {
                    _.War.Land.DestroyTerrain (_o + c.Offset, Balance.HoleBullet);
                }
                else {
                    (c.Collider2.Entity as IDamageable)?.TakeDamage (Balance.DamageBullet);
                    (c.Collider2.Entity as IBlastable )?.TakeBlast  (_v.WithLength (Balance.PushBullet));
                }
            }
            Despawn ();
        }

    }

    public class PistolBullet : Ray {

        private XY _o, _v;


        public PistolBullet (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            var c = Cast (_v);
            if (c != null) {
                if (c.IsLandCollision) {
                    _.War.Land.DestroyTerrain (_o + c.Offset, Balance.HoleBullet);
                }
                else {
                    (c.Collider2.Entity as IDamageable)?.TakeDamage (Balance.DamageBulletHeavy);
                    (c.Collider2.Entity as IBlastable )?.TakeBlast  (_v.WithLength (Balance.PushBulletHeavy));
                }
            }
            Despawn ();
        }

    }

    public class PoisonArrow : Ray {

        private XY _o, _v;


        public PoisonArrow (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            var c = Cast (_v);
            if (c != null) {
                if (c.IsLandCollision) {
                    // todo создать стрелу вместо удаления ландшафта
                    _.War.Land.DestroyTerrain (_o + c.Offset, Balance.HoleBullet);
                }
                else {
                    (c.Collider2.Entity as IDamageable)?.TakeDamage (Balance.DamageCrossbowBolt);
                    (c.Collider2.Entity as Worm       )?.AddPoison  (Balance.PoisonCrossbowBolt);
                    (c.Collider2.Entity as IBlastable )?.TakeBlast  (_v.WithLength (Balance.PushBulletHeavy));
                }
            }
            Despawn ();
        }

    }

    public class BlasterBullet : Ray {

        private XY _o, _v;


        public BlasterBullet (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            var c = Cast (_v);
            if (c != null) _.War.MakeExplosionSmall (_o + c.Offset);
            Despawn ();
        }

    }

    public class HeatRay : Ray {

        private XY _o, _v;


        public HeatRay (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            var c = Cast (_v);
            if (c != null) {
                // (c.Collider2?.Entity as IDamageable)?.TakeDamage (Balance.DamageBulletHeavy);
                _.War.MakeExplosionFireTiny (_o + c.Offset);
            }
            Despawn ();
        }

    }


    public class UltraWave : Ray {

        private XY _o, _v;


        public UltraWave (XY o, XY v) {
            _o = o;
            _v = v;
            Collider = new CircleCollider (o, 0);
        }


        public override void OnSpawn () {
            // ультраволна проходит через все объекты и метод кастования будет отличаться
            // задача упрощается тем что урон от ультраволны получают только червяки

            var worms = _.War.WormsSystem.FindUltraWaveTargets (Collider, _v);
            foreach (var worm in worms) worm.TakeDamage (Balance.DamageBullet);

            Despawn ();
        }

    }

}
