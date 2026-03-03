using System;


namespace Utils {

    public class LootTable <T> {

        private T   [] _table;
        private int [] _sums;


        public LootTable (params Tuple <T, int> [] table) {
            _table = new T  [table.Length];
            _sums  = new int[table.Length];
            for (int i = 0, sum = 0; i < table.Length; i++) {
                _table [i] =        table [i].Item1;
                _sums  [i] = sum += table [i].Item2;
            }
        }


        public T GetLoot (Random random) {
            int n = random.Int (_sums [_sums.Length - 1]);

            int l = 0;
            int h = _sums.Length;
            while (l != h) {
                int m = (l + h) / 2;
                
                if (_sums [m] > n) h = m;
                else               l = m + 1;
            }

            return _table [h];
        }

    }

}
