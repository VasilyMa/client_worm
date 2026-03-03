using Math;
using UnityEngine;
using War.Entities;
using Collision = War.Systems.Collisions.Collision;


namespace War.Components.CollisionHandlers {

    public class WormFallCh : CollisionHandler <Worm> {

        // здесь уже не очень ясно что делает этот флаг
        // private bool _fallDamage;
        // запомненная скорость с которой сравнивается новая скорость после столкновения?
        // private XY   _previousVelocity;
        

        public override void OnBeforeCollision (Collision collision) {
            // if (!collision.IsLandCollision && collision.Collider2.Entity.PassableFor (Entity)) return;
            //
            // _fallDamage       = true;
            // _previousVelocity = Entity.Velocity;
        }


        public override void OnCollision (Collision collision) {
            // итак, по идее что, если начальная скорость 10 и меньше, то урона нет
            // даже при лобовом столкновении
            // если же скорость превышает 10, мы наносим либо разницу, либо максимальный
            // который равен скорость минус 10
            
            // if (_fallDamage) {
            //     int damage = (int) Mathf.Ceil ((Entity.Velocity - _previousVelocity).Length - 10);
            //     if (damage > 0) Entity.TakeDamage(damage);
            //     // типа чтобы 2 раза не засчиталось? так все равно сменится класс и состояние пропадет
            //     _fallDamage = false;
            // }

            (collision.Collider2?.Entity as Worm)?.Fall (Entity);
            Entity.Fall (false);
            // Entity.TakeDamage (0); - todo: рассчитать урон по скорости и поменять ее возможно
        }

    }

}
