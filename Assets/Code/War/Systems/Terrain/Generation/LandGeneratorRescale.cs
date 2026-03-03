namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public class Rescale : Algorithm {

            private const int EstimationCoeff = 1;

            private int _w;
            private int _h;


            public Rescale (int w, int h) {
                _w = w;
                _h = h;
            }


            public override void OnAdd (LandGenerator gen) {
                gen._estimation += _w * _h * EstimationCoeff;
                gen._targetW    =  _w;
                gen._targetH    =  _h;
            }


            public override bool Apply (LandGenerator gen) {
                var a  = gen.Land;
                var b  = new byte[_w, _h];
                int aw = a.GetLength (0);
                int ah = a.GetLength (1);

                for (int x = 0; x < _w; x++)
                for (int y = 0; y < _h; y++) {
                    b [x, y] = a [x * aw / _w, y * ah / _h];
                }
                gen.Land       =  b;
                gen._completed += _w * _h * EstimationCoeff;
                return true;
            }

        }

    }

}
