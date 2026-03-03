namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public class SwitchDimensions : Algorithm {

            private const int EstimationCoeff = 1;


            public override void OnAdd (LandGenerator gen) {
                gen._estimation += gen._targetW * gen._targetH * EstimationCoeff;
                int temp     = gen._targetH;
                gen._targetH = gen._targetW;
                gen._targetW = temp;
            }


            public override bool Apply (LandGenerator gen) {
                var a = gen.Land;
                int w = a.GetLength (0);
                int h = a.GetLength (1);
                var b = new byte [h, w];

                for (int x = 0; x < w; ++x) {
                    int ix = w - x - 1;
                    for (int y = 0; y < h; ++y) {
                        b [y, x] = a [ix, y];
                    }
                }
                gen.Land = b;
                gen._completed += w * h * EstimationCoeff;
                return true;
            }

        }

    }

}