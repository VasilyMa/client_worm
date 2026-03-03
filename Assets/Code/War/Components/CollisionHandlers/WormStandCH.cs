using Math;
using UnityEngine;
using War.Entities;
using Collision = War.Systems.Collisions.Collision;


namespace War.Components.CollisionHandlers {

    public class WormStandCH : CollisionHandler <Worm> {

        public override void OnBeforeCollision (Collision collision) {
            // var v1 = Entity.Velocity;
            // var v2 = collision.IsLandCollision ? XY.Zero : collision.Collider2.Entity.Velocity;
            // Debug.Log ((v1 - v2).Length);
        }


        public override void OnCollision (Collision c) {
            // todo: столкновения должны переводить его в Fall
        }

    }

}
