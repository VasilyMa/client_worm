using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Math;
using War.Entities;
using War.Entities.Liquids;
using War.Systems.Terrain;


namespace War.Systems.Fire {

    public class FireSystem {

        // ответственность класса:
        // хранение огоньков и работа с ними
        // 


        private List <Flame>           _flames      = new List <Flame>           ();
        private List <IFireDamageable> _damageables = new List <IFireDamageable> ();


        public void Reset () {
            foreach (var o in _damageables) o.ResetFire ();
        }


        // уменьшает длительность горения огней на 1 ход
        public void OnTurnEnded () {
            Reset ();
            var result = new HashSet <LandTile> ();
            _.War.Land.BeginRaw ();
            foreach (var flame in _flames) flame.OnTurnEnded (result);
            foreach (var tile in result) tile.Recalculate (_.War.Land);
            _.War.Land.EndRaw ();
            _flames.RemoveAll (f => f.Despawned);
        }


        public IEnumerable <Flame> GetFires (Box box) {
            return null;
        }


        public void Add (Flame flame) {
            _flames.Add (flame);
        }


        public void Add (IFireDamageable damageable) {
            _damageables.Add (damageable);
        }


        public void Update () {
            // на первое время не делаем оптимизацию по тайлам
            
            // var set = new HashSet <Flame> (); // которые сработали
            
            foreach (var damageable in _damageables.Where (o => o.Alive)) {
                var list = new List <Flame> ();
                foreach (var flame in _flames.Where (flame => damageable.IsNear (flame))) {
                    list.Add (flame);
                    // set.Add (flame);
                }
                if (list.Count > 0) damageable.ApplyFire (list);
            }

            // foreach (var flame in set) flame.OnTrigger ();
            _flames.RemoveAll (f => f.Despawned);
        }


        public void ExtinguishFire (XY o, float r) {
            foreach (var flame in _flames.Where (f => XY.SqrDistance (f.Position, o) < r * r)) flame.Extinguish ();
            _flames.RemoveAll (f => f.Despawned);
        }

    }


    public interface IFireDamageable {

        bool Alive { get; }
        bool IsNear (Flame flame); // true если заденет
        void ApplyFire (IList <Flame> flames);
        void ResetFire ();

    }

}
