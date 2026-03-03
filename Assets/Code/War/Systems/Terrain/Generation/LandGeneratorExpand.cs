using Utils;


namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public class Expand : Algorithm {

            private const int EstimationCoeff = 1;

            private int _iterations;


            public Expand (int iterations = 1) { _iterations = iterations; }


            public override void OnAdd (LandGenerator gen) {
                for (int i = 0; i < _iterations; i++) {
                    gen._targetW = gen._targetW * 2 - 1;
                    gen._targetH = gen._targetH * 2 - 1;
                    gen._estimation += gen._targetW * gen._targetH * EstimationCoeff;
                }
            }


            public override bool Apply (LandGenerator gen) {
                var a  = gen.Land;
                int aw = a.GetLength (0);
                int ah = a.GetLength (1);
                int bw = aw * 2 - 1;
                int bh = ah * 2 - 1;
                var b  = new byte[bw, bh];

                // X-X
                // ---
                // X-X
                for (int x = 0; x < aw; x++)
                for (int y = 0; y < ah; y++) {
                    b [x * 2, y * 2] = a [x, y];
                }

                // #-#
                // -X-
                // #-#
                for (int x = 1; x < bw; x += 2)
                for (int y = 1; y < bh; y += 2) {
                    b [x, y] = (byte) (gen._rng.Bool (
                        b [x - 1, y - 1] +
                        b [x + 1, y - 1] +
                        b [x + 1, y + 1] +
                        b [x - 1, y + 1],
                        4
                    ) ? 1 : 0);
                }

                // |#-  -#|
                // |X#  #X|
                // |#-  -#|
                for (int y = 1; y < bh; y += 2) {
                    b [0, y] = b [bw - 1, y] = 0;
                }

                for (int x = 1; x < bw; x += 2) {
                    // -#-
                    // #X#
                    // ~~~
                    b [x, 0] = (byte) (gen._rng.Bool (
                        b [x - 1, 0] +
                        b [x + 1, 0] +
                        b [x, 1] + 1,
                        4
                    ) ? 1 : 0);
                    // '''
                    // #X#
                    // -#-
                    b [x, bh - 1] = 0;
                }

                // -#-
                // #X#
                // -#-
                for (int x = 1; x < bw - 1; ++x)
                for (int y = (x & 1) != 0 ? 2 : 1; y < bh - 1; y += 2) {
                    b [x, y] = (byte) (gen._rng.Bool (
                        b [x - 1, y] +
                        b [x + 1, y] +
                        b [x, y - 1] +
                        b [x, y + 1],
                        4
                    ) ? 1 : 0);
                }

                gen.Land = b;
                gen._completed += bw * bh * EstimationCoeff;
                return --_iterations == 0;
            }

        }

    }

}