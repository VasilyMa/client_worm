using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;
using War.Components;
using War.Indication;
using War.Systems.Blasts;
using War.Systems.Updating;
using Collision = War.Systems.Collisions.Collision;


namespace War.Entities.Projectiles {

    public class HomingMissile : MobileEntity, IUpdatable, IBlastable {

        private readonly XY             _target;
        private          int            _spawnedAt;
        private          GameObject     _gameObject;
        private readonly PointCrosshair _crosshair;


        public HomingMissile (XY target, PointCrosshair crosshair) {
            _target    = target;
            _crosshair = crosshair;
            if (crosshair) crosshair.References++;
        }


        public override void OnSpawn () {
            _spawnedAt = _.War.Time;

            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            AddCollider (new CircleCollider (0, 0, Balance.ShellRadius));

            _gameObject = Object.Instantiate (
                _.War.Assets.HomingMissile,
                (Vector3) Position,
                Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg)
            );
            PositionChanged += (a, b) => _gameObject.transform.localPosition = (Vector3) b;
        }


        protected override void OnDespawn () {
            Object.Destroy (_gameObject);
            if (_crosshair) _crosshair.References--;
            base.OnDespawn ();
        }


        public override void OnCollision (Collision collision) {
            Despawn ();
            _.War.MakeExplosion (Position);
        }


        public bool Alive => !Despawned;


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            int t = _.War.Time - _spawnedAt;
            if (t < 30 || t >= 330) {
                Velocity.Y += WarScene.Gravity;
            }
            else {
                var desiredVelocity = (_target - Position).WithLength (20);
                var acceleration = (desiredVelocity - Velocity).WithLengthClamped (0.5f);
                Velocity += acceleration;
            }
            _gameObject.transform.localRotation = Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg);
        }

    }

}
