using Collision = War.Systems.Collisions.Collision;


namespace War.Components.CollisionHandlers {

    public abstract class CollisionHandler <T> : Component <T> {

        public abstract void OnBeforeCollision (Collision collision); // вызывается до того как поменялись скорости
        public abstract void OnCollision       (Collision collision);

    }

}
