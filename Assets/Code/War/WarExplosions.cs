using System;
using Math;
using UnityEngine;
using Utils;
using War.Entities;
using War.Entities.Liquids;
using War.Entities.Smokes;
using War.Indication;
using static War.Balance;


namespace War {

    public partial class WarScene {
        
        // взрыв вызывает такие действия:
        //     создание дырки в земле
        //     урон
        //     отбрасывание
        //     создание частиц


        public void MakeExplosionTiny (XY o) {
            Land.DestroyTerrain (o, HoleExplosionTiny);
            DamageSystem.DealDamage (o, RadiusExplosionTiny, 10, DamageExplosionTiny);
            BlastSystem.MakeBlast (o, o, RadiusExplosionTiny, PushExplosionTiny);
            SpawnSmoke (o, RadiusExplosionTiny);
        }


        public void MakeExplosionSmall (XY o) {
            Land.DestroyTerrain (o, HoleExplosionSmall);
            DamageSystem.DealDamage (o, RadiusExplosionSmall, 10, DamageExplosionSmall);
            BlastSystem.MakeBlast (o, o, RadiusExplosionSmall, PushExplosionSmall);
            SpawnSmoke (o, RadiusExplosionSmall);
        }


        public void MakeExplosion (XY o) {
            Land.DestroyTerrain (o, HoleExplosion);
            DamageSystem.DealDamage (o, RadiusExplosion, 10, DamageExplosion);
            BlastSystem.MakeBlast (o, o, RadiusExplosion, PushExplosion);
            SpawnSmoke (o, RadiusExplosion);
        }


        public void MakeExplosionLarge (XY o) {
            Land.DestroyTerrain (o, HoleExplosionLarge);
            DamageSystem.DealDamage (o, RadiusExplosionLarge, 10, DamageExplosionLarge);
            BlastSystem.MakeBlast (o, o, RadiusExplosionLarge, PushExplosionLarge);
            SpawnSmoke (o, RadiusExplosionLarge);
        }


        public void MakeExplosionHuge (XY o) {
            Land.DestroyTerrain (o, HoleExplosionHuge);
            DamageSystem.DealDamage (o, RadiusExplosionHuge, 10, DamageExplosionHuge);
            BlastSystem.MakeBlast (o, o, RadiusExplosionHuge, PushExplosionHuge);
            SpawnSmoke (o, RadiusExplosionHuge);
        }


        public void MakeExplosionNuclear (XY o) {
            Land.DestroyTerrain (o, HoleExplosionNuclear);
            DamageSystem.DealDamage (o, RadiusExplosionNuclear, 10, DamageExplosionNuclear);
            BlastSystem.MakeBlast (o, o, RadiusExplosionNuclear, PushExplosionNuclear);
            SpawnSmoke (o, RadiusExplosionNuclear);
            // todo: добавить радиацию
        }


        public void MakeExplosionCluster (XY o) {
            Land.DestroyTerrain (o, HoleExplosionTiny);
            DamageSystem.DealDamage (o, RadiusExplosion, 10, DamageExplosionTiny);
            BlastSystem.MakeBlast (o, o, RadiusExplosion, PushExplosionTiny);
            SpawnSmoke (o, 20, 100, 25, 35);
        }


        public void MakeExplosionPoisonSmall (XY o) {
            SpawnPoisonGas (o, 3, RadiusExplosionSmall);
        }


        public void MakeExplosionPoison (XY o) {
            SpawnPoisonGas (o, 4, RadiusExplosion);
        }


        public void MakeExplosionFireTiny (XY o) {
            Land.DestroyTerrain (o, HoleBullet);
            DamageSystem.DealDamage (o, RadiusExplosionTiny, 5, DamageHeatRay);
            SpawnFire (o, 20, RadiusExplosionTiny);
        }


        public void MakeExplosionFireSmall (XY o) {
            Land.DestroyTerrain (o, HoleExplosionTiny);
            DamageSystem.DealDamage (o, RadiusExplosionTiny, 5, DamageExplosionTiny);
            BlastSystem.MakeBlast (o, o, RadiusExplosionTiny, PushExplosionTiny);
            SpawnFire (o, 110, RadiusExplosionSmall);
        }


        public void MakeExplosionFire (XY o) {
            Land.DestroyTerrain (o, HoleExplosionSmall);
            DamageSystem.DealDamage (o, RadiusExplosionSmall, 10, DamageExplosionSmall);
            BlastSystem.MakeBlast (o, o, RadiusExplosionSmall, PushExplosionSmall);
            SpawnFire (o, 180, RadiusExplosion);
        }


        public void MakeIceBall (XY o) {
            Land.CreateTerrain (o, HoleExplosion);
        }


        public void MakeExplosionFlashbang (XY o) {
            
        }


        public void ExtinguishFire (XY o) {
            FireSystem.ExtinguishFire (o, HoleExplosion * 1.5f);
        }


        public void SpawnSmoke (XY center, float radius) {
            SpawnSmoke (center, (int) radius, radius, 25, 25 + radius * 0.5f);
        }


        public void SpawnSmoke (XY center, int particles, float radius, float minSize, float maxSize) {
            for (int i = 0; i < particles; i++) {
                var v = Danmaku.Cloud (SmokeWindFactor * radius);
                Spawn (new Smoke (Random.Float (minSize, maxSize), center, v));
            }
        }


        // урон в флоате но он округлится вверх и от дробной части зависит длительность когда максимум наносится
        public void SpawnPoisonGas (XY center, float damage, float radius) {
            for (int i = 0; i < radius; i += 2) {
                var v = Danmaku.Cloud (SmokeWindFactor * radius);
                Spawn (new PoisonGas (Random.Float (25, damage * 20), center, v));
            }
        }


        public void SpawnFire (XY center, int particles, float radius) {
            for (int i = 0; i < particles; i++) {
                var v = Danmaku.Cloud (LiquidWindFactor * radius * 2);
                Spawn (new Flame (), center, v);
            }
        }


        public void LaunchAirstrike (
            Func <MobileEntity> generator,
            XY target,
            int bombs,
            bool leftToRight,
            PointCrosshair crosshair
        ) {
            Debug.Log (target);
            
            float avx = leftToRight ? BomberSpeed : -BomberSpeed; // скорость самолета
            float bvx = leftToRight ? 5f : -5f;                   // скорость бомб начальная
            float y   = Land.Height + BomberHeight;
            float dy  = target.Y - y;
            float t   = Mathf.Sqrt (2 * dy / Gravity);
            float dx  = bvx * t;
            float x   = target.X - dx - 60 * avx;                 // 60 = 1 секунда

            var bomber = new Bomber (generator, bombs, bvx, crosshair);
            Spawn (bomber, new XY (x, y), new XY (avx, 0f));
        }

    }

}