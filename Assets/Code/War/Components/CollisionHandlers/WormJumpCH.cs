using Math;
using UnityEngine;
using War.Entities;
using Collision = War.Systems.Collisions.Collision;


namespace War.Components.CollisionHandlers {

    public class WormJumpCH : CollisionHandler <Worm> {

        public override void OnBeforeCollision (Collision collision) {
            if (!collision.IsLandCollision && collision.Collider2.Entity.PassableFor (Entity)) return;
            // сюда мы попадем если ударились об землю или что-то тяжелое
            
            var v1 = Entity.Velocity;
            var v2 = collision.IsLandCollision ? XY.Zero : collision.Collider2.Entity.Velocity;
            
            if ((v1 - v2).SqrLength <= 100) return;
            // мы проходим если разность скоростей больше 10
            
            Entity.Fall (); // решение проблемы с нетолкаемостью стоячего червяка
        }


        public override void OnCollision (Collision collision) {
            // урон от падения
            // Debug.Log (Entity.Velocity);
            
            // идея такая
            // мы приземляется, если мы стукнулись низом
            // мы отскакиваем по правилам физики, если стукнулись боком
            // мы отскакиваем строго вниз, если стукнулись головой
            
            // а так как коллайдерам доверять нельзя мы будем доверять нормалям
            float angle = collision.Normal.Angle;

            if      (angle < Mathf.PI - 0.1f && angle >  0.1f) Entity.AfterJump ();
            else if (angle > 0.1f - Mathf.PI && angle < -0.1f) Entity.Velocity = XY.Zero;
        }

    }

}