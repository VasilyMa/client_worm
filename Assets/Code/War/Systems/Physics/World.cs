using System.Collections.Generic;
using Core;
using War.Entities;
using War.Systems.Collisions;
using War.Systems.Terrain;


namespace War.Systems.Physics {

    public class World {
        
        // на самом деле единственное назначение этого класса - обработка физики


        private List <MobileEntity> _entities = new List <MobileEntity> ();


        public void Add (MobileEntity e) => _entities.Add (e);
        

        public void Update () {
            _entities.RemoveAll (e => e.Despawned);
            foreach (var e in _entities) e.PhysicsBegin ();
            
            for (int it = Settings.PhysicsIterations; it > 0; it--)
            for (int i = 0; i < _entities.Count; i++) {
                var e = _entities [i];
                if (e.Despawned) continue;
                e.PhysicsUpdate (it <= 2); // очень важное магическое число
            }
            
            _entities.RemoveAll (e => e.Despawned);
            foreach (var e in _entities) e.PhysicsEnd ();
        }

    }

}
