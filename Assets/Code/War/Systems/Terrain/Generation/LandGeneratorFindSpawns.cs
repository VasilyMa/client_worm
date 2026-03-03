using Math;


namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public class FindSpawns : Algorithm {

            private const int EstimationCoeff = 30;

            private int _i;


            public override void OnAdd (LandGenerator gen) {
                int middle = gen._targetW / 2;
                _i = middle % 50;
                // расстояние 50 потому что у мины радиус активации 40

                if (gen._targetW == 0) return;

                int iters = (gen._targetW - 1 - _i) / 50 + 1;
                // 1-99    - 1 spawn
                // 100     - 2 spawns
                // 101-199 - 3 spawns

                gen._estimation += iters * gen._targetH * EstimationCoeff;
            }


            public override bool Apply (LandGenerator gen) {
//                if (_i >= gen._targetW) return true;
// если у карты нулевая ширина, на ней все равно нельзя будет играть
                ProcessX (gen, _i);
                _i += 50;
                return _i >= gen.Land.GetLength (0);
            }


            private void ProcessX (LandGenerator gen, int x) {
                // идем по вертикали вниз, где находим землю проверяем спавн
                var land = gen.Land;
                int h    = land.GetLength (1);
                // сначала ищем где земля
                int air = 30;
                for (int y = h - 1; y >= 0; y--) {
                    if (land [x, y] != 0) { // земля
                        if (air >= 30) {
                            int spawnY = CheckSpawn (land, x, y);
                            if (spawnY >= 0) {
                                gen.Spawns.Add (new XY (x, spawnY + 15));
                                y -= 40;
                            }
                        }
                        air = 0;
                    }
                    else {
                        air++;
                    }
                }
                gen._completed += h * EstimationCoeff;
            }


            private int CheckSpawn (byte [,] land, int x, int y) {
                int w        = land.GetLength (0);
                int h        = land.GetLength (1);
                int x0       = System.Math.Max (x - 8, 0);
                int x1       = System.Math.Min (x + 9, w);
                int airCount = 0;
                for (int i = 1; i <= 45; i++) {
                    int iy = i + y;
                    if (iy >= h) return iy - airCount;

                    bool air = true;
                    for (int ix = x0; ix < x1; ix++) {
                        if (land [ix, iy] == 0) continue;
                        air = false;
                    }
                    if (air) {
                        if (++airCount == 30) return iy - 29;
                    }
                    else {
                        if (i > 15) return -1;
                        airCount = 0;
                    }
                }
                return -1;
            }

        }

    }

}
