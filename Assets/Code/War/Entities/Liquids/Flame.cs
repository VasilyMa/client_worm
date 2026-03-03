using System.Collections.Generic;
using Core;
using DataTransfer.Data;
using Geometry;
using Math;
using UnityEngine;
using Utils;
using War.Entities.Smokes;
using War.Systems.Terrain;
using Collision = War.Systems.Collisions.Collision;
using ParticleSystem = War.Systems.Particles.ParticleSystem;


namespace War.Entities.Liquids {

    public class Flame : Liquid {

        /* Как огонь работает в игре:
         *  он должен лететь со случайными отклонениями
         *  при столкновении с землей скорость выставляется в 0
         *  если долго лежит на одном месте, устаканивается
         *  хитбокс огонька зависит от его хп
         * если червяк находятся рядом с огоньком:
         *  ему наносится урон и выставляется таймер когда он урон от огня не получает
         *  если таймер уже стоит то он уменьшается
         *  его отбрасывает в сторону (суммарный импульс ограничен)
         *  огонек становится меньше
         *
         * Вообще огоньки не должны мешать остальным объектам летать
         * поэтому наверно их коллайдер можно вообще в мир не добавлять
         *
         * урон от одного огонька устанавливается как 0,5
         */

        private ParticleSystem.ParticleHandler _ph;


        public int HP { get; private set; }
        private Worm _worm; // червяк выстреливший огонь не должен подвергаться его воздействию в первый 1 или 2 такта


        public Circle Hitbox => new Circle (Position, (10 + HP) * 0.5f);


        public Flame () {
            HP = 50;
        }


        public override void OnSpawn () {
            base.OnSpawn ();
            _.War.FireSystem.Add (this);
            _ph = _.War.FireParticles.Add (UpdateParticle);
        }


        protected override void OnDespawn () {
            _ph.Alive = false;
        }


        public override void Update (TurnData td) {
            base.Update (td);

            if (_.Random.Int (10) == 0) {
                _.War.Spawn (new Smoke (_.Random.Float (20), Position, new XY (0, _.Random.Float (10))));
            }
        }


        private void UpdateParticle (ref UnityEngine.ParticleSystem.Particle particle) {
            particle.position = (Vector3) Position;
            particle.startSize = 10 + HP;
            // particle.startColor = Idle ? Color.cyan : Color.white;
        }


        public override void OnCollision (Collision collision) {
            bool fix =
                collision.Primitive2.Type == Primitive.PType.Top                      ||
                collision.Primitive2.Type == Primitive.PType.Left  && 0 >= 0 ||
                collision.Primitive2.Type == Primitive.PType.Right && 0 <= 0 ||
                _.War.Random.Int (6) == 0;

            if (fix) Fix ();
            else     Velocity = XY.Zero;
        }


        public void OnTurnEnded (HashSet <LandTile> result) {
            _.War.Land.DestroyTerrainRaw (Position, Balance.HoleFire, result);
            HP -= 10;
            if (HP <= 0) Despawn ();
            else         Unfix ();
        }


        // true если огонь нанес урон
        public bool OnTrigger () {
            HP--;
            if (HP <= 0) Despawn ();
            return true;
        }


        public void Extinguish () {
            Despawn ();
        }

    }

}
