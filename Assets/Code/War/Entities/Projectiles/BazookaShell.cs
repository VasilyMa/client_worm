using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;
using War.Entities.Smokes;
using War.Systems.Blasts;
using War.Systems.Updating;
using Collision = War.Systems.Collisions.Collision;


namespace War.Entities.Projectiles {

    public class BazookaShell : MobileEntity, IUpdatable, IBlastable {

        public bool Alive => !Despawned;
        public GameObject GameObject { get; private set; }


        public BazookaShell () {
            MassRank = Balance.ShellMassRank;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            
            AddCircleCollider (Balance.ShellRadius);

            GameObject = Object.Instantiate (
                _.War.Assets.BazookaShell,
                (Vector3) Position,
                Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg)
            );
            
            PositionChanged += (from, to) => GameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (GameObject);
        }


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity += new XY (0, WarScene.Gravity);
            GameObject.transform.localRotation = Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg);
            _.War.Spawn (new Smoke (25 + _.War.Random.Int(25), Position + new XY (_.War.Random.Angle ()) * 5, XY.Zero));
        }


        public override void OnCollision (Collision collision) {
            Despawn ();
            _.War.MakeExplosion (Position);
        }

    }

}
