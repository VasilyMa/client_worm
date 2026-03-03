using System;


namespace War.Entities {

    public class Entity {

        public bool Despawned { get; private set; }
        

        public virtual void OnSpawn () {}


        protected void Despawn () {
            if (Despawned) throw new InvalidOperationException ();
            Despawned = true;
            OnDespawn ();
        }


        protected virtual void OnDespawn () {}

    }

}
