using System;
using System.Collections.Generic;
using Math;


namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        private readonly Random _rng;

        public byte [,]  Land;
        public List <XY> Spawns = new List <XY> ();
        
        private LinkedList <Algorithm> _queue = new LinkedList <Algorithm> ();

        private long _estimation;
        private long _completed;
        private int _targetW, _targetH;


        public int   ProgressPercents => (int) (_completed * 100 / _estimation);
        public float Progress         => _completed / (float) _estimation;


        public LandGenerator (Random rng) {
            _rng = rng;
            Land = new byte [,] {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 1, 0, 0},
                {0, 1, 0, 1, 0, 1, 0},
                {0, 1, 1, 0, 1, 1, 0},
                {0, 1, 0, 1, 0, 1, 0},
                {0, 1, 1, 0, 1, 1, 0}
            };
            _targetW = Land.GetLength (0);
            _targetH = Land.GetLength (1);

            AddAlgorithms (
                new SwitchDimensions (),
                new Expand (6),
                new Cellular (0x01e801d0, 10),
                new Cellular (0x01f001e0, 10),
                new Expand (3),
                new Rescale (2000, 1000),
                new Cellular (0x01e801d0, 40),
                new Cellular (0x01f001e0, 10),
                new FindSpawns ()
            );
        }


        public LandGenerator (Random rng, object _) {
            _rng = rng;
            Land = new byte [,] {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 1, 0, 0},
                {0, 1, 0, 1, 0, 1, 0},
                {0, 1, 1, 0, 1, 1, 0},
                {0, 1, 0, 1, 0, 1, 0},
                {0, 1, 1, 0, 1, 1, 0}
            };
            _targetW = Land.GetLength (0);
            _targetH = Land.GetLength (1);

            AddAlgorithms (
                new SwitchDimensions (),
                new Expand (6),
                new Cellular (0x01e801d0, 40),
                new Cellular (0x01f001e0, 10),
                new Expand (2),
                new Rescale (1000, 500),
                new Cellular (0x01e801d0, 40),
                new Cellular (0x01f001e0, 10),
                new FindSpawns ()
            );
        }


        private void AddAlgorithm (Algorithm algo) {
            algo.OnAdd (this);
            _queue.AddLast (algo);
        }


        private void AddAlgorithms (params Algorithm [] algos) {
            foreach (var algo in algos) {
                AddAlgorithm (algo);
            }
        }


        public bool Update () { // true if complete
            if (_queue.Count == 0) {
                return true;
            }
            var algo = _queue.First.Value;
            if (algo.Apply (this)) {
                _queue.RemoveFirst ();
            }
            return _queue.Count == 0;
        }


        public IEnumerable <float> Generate () {
            foreach (var algo in _queue) {
                // todo foreach in algo.Apply (this)...
                
                while (!algo.Apply (this)) yield return Progress;
                yield return Progress;
            }
        }

    }

}