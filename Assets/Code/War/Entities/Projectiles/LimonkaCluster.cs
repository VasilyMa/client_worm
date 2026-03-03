using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Systems.Blasts;
using War.Systems.Updating;
using Collision = War.Systems.Collisions.Collision;


namespace War.Entities.Projectiles {

    public class LimonkaCluster : MobileEntity, IUpdatable, IBlastable {

        private GameObject _gameObject;


        public bool Alive => !Despawned;


        public LimonkaCluster () {
            MassRank = Balance.ShellMassRank;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            
            AddCircleCollider (Balance.ClusterRadius);

            _gameObject = Object.Instantiate (
                _.War.Assets.LimonkaCluster,
                (Vector3) Position,
                Quaternion.identity
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
            Velocity.Y += WarScene.Gravity;
        }


        public override void OnCollision (Collision collision) {
            Despawn ();
            _.War.MakeExplosionTiny (Position);
        }

    }

}
