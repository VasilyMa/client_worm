using System;
using System.Collections.Generic;
using System.Linq;
using Math;
using War.Components;
using War.Entities;


namespace War.Systems.Worms {

    public class WormsSystem {

        // просто класс для всяких червяко-специфичных взаимодействий объектов.
        // здесь не должно быть метода GetWorms или чего-то такого, так как мы
        // должны делать все операции с червяками в этом классе.


        private List <Worm> _worms = new List <Worm> ();

        public List<Worm> Worms => _worms;

        public void Add (Worm worm) {
            _worms.Add (worm);
        }


        public bool TryDealPoisonDamage () {
            _worms.RemoveAll (w => w.Despawned);
            bool success = false;
            for (int i = 0; i < _worms.Count; i++) {
                if (_worms [i].HP > 0) continue;
                _worms [i].TakePoisonDamage ();
                success = true;
            }
            return success;
        }


        public bool TryReap () {
            _worms.RemoveAll (w => w.Despawned);
            bool success = false;
            for (int i = 0; i < _worms.Count; i++) {
                if (_worms [i].HP > 0) continue;
                _worms [i].Die ();
                success = true;
            }
            return success;
        }


        public IEnumerable <Worm> FindUltraWaveTargets (Collider c, XY v) {
            var excluded = new HashSet <Worm> (_worms.Where (w => w.Colliders.Any(c.Overlaps)));
            return _worms.Where (w => !excluded.Contains(w) && w.Colliders.Any (wc => c.FlyInto (wc, v) != null));
        }

    }

}
