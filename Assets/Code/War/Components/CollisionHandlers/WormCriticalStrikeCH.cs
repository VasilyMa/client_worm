using War.Entities;
using War.Systems.Collisions;


namespace War.Components.CollisionHandlers {

    public class WormCriticalStrikeCH : WormJumpCH {

        public override void OnCollision (Collision collision) {
            var worm = collision.Collider2?.Entity as Worm;
            if (worm == null) {
                // мы должны получать урон от падения здесь но его пока нет
                base.OnCollision (collision);
            }
            else {
                worm.TakeDamage (50);
                worm.Fall ();
                worm.Velocity = Entity.Velocity;
                Entity.AfterJump ();
            }
        }

    }

}