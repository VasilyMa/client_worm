using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;
using War.Systems.Blasts;
using War.Systems.Damage;
using War.Systems.Updating;
using Collision = War.Systems.Collisions.Collision;


namespace War.Entities.Projectiles {

    public class MolotovBottle : MobileEntity, IUpdatable, IBlastable, IDamageable {

        public bool       Alive      => !Despawned;
        public GameObject GameObject { get; private set; }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            _.War.DamageSystem.Add (this);
            
            AddCircleCollider (Balance.ShellRadius);

            GameObject = Object.Instantiate (
                _.War.Assets.Molotov,
                (Vector3) Position,
                Quaternion.Euler (0, 0, _.War.Random.Float (360))
            );
            
            PositionChanged += (from, to) => GameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (GameObject);
        }


        public void TakeDamage    (int damage) => Explode ();
        public void TakeAxeDamage ()           => Explode ();


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity.Y += WarScene.Gravity;
            // GameObject.transform.localRotation =  Quaternion.Euler (0, 0, _.War.Random.Float (360));
            // todo: вращение
        }


        public override void OnCollision (Collision collision) => Explode ();


        private void Explode () {
            Despawn ();
            _.War.MakeExplosionFireSmall (Position);
        }

    }

}