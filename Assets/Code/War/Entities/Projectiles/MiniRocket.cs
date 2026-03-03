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

    public class MiniRocket : MobileEntity, IUpdatable, IBlastable {
        
        private GameObject _gameObject;


        public bool Alive => !Despawned;


        public MiniRocket () {
            MassRank = Balance.ShellMassRank;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            
            AddCircleCollider (Balance.ShellRadius);

            _gameObject = Object.Instantiate (
                _.War.Assets.MiniRocket,
                (Vector3) Position,
                Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg)
            );
            
            PositionChanged += (from, to) => _gameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (_gameObject);
        }


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity += new XY (0, WarScene.Gravity);
            _gameObject.transform.localRotation = Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg);
            _.War.Spawn (new Smoke (15 + _.War.Random.Int(15), Position + new XY (_.War.Random.Angle ()) * 5, XY.Zero));
        }


        public override void OnCollision (Collision collision) {
            Despawn ();
            _.War.MakeExplosionSmall (Position);
        }

    }

}