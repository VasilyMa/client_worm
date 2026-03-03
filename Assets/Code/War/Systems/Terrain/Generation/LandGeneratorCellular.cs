using System.Runtime.CompilerServices;


namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public class Cellular : Algorithm {

            private const int EstimationCoeff = 2;
            private static readonly uint [] Init = {1, 1 << 16};

            private readonly uint _rules;
            private          int  _iterations;


            public Cellular (uint rules, int iterations) {
                _rules = rules;
                _iterations = iterations;
            }


            public override void OnAdd (LandGenerator gen) {
                gen._estimation += gen._targetW * gen._targetH * _iterations * EstimationCoeff;
            }


            public override bool Apply (LandGenerator gen) {
                var a = gen.Land;
                int w = a.GetLength(0);
                int h = a.GetLength(1);
                var b = new byte [w, h];
                
                uint temp;

                for (int x = 1; x < w - 1; ++x)
                for (int y = 1; y < h - 1; ++y) {
                    temp = Init [a [x, y]] <<           +a[x-1,y+1] +a[x,y+1] +a[x+1,y+1]
                                                        +a[x-1,y  ]           +a[x+1,y  ]
                                                        +a[x-1,y-1] +a[x,y-1] +a[x+1,y-1];
                    b [x, y] = Test (temp);
                }

                for (int x = 1; x < w - 1; ++x) {
                    // в y=0 вода
                    // чтобы земли около воды было больше добавляем 2
                    temp = Init [a [x, 0]] << 2         +a[x-1,1] +a[x,1] +a[x+1,1]
                                                        +a[x-1,0]         +a[x+1,0];
                    b [x, 0] = Test (temp);
                    
                    
                    temp = Init [a [x, h-1]] <<         +a[x-1,h-1]           +a[x+1,h-1]
                                                        +a[x-1,h-2] +a[x,h-2] +a[x+1,h-2];
                    b [x, h-1] = Test (temp);
                }

                for (int y = 1; y < h - 1; ++y) {
                    temp = Init [a [0, y]] <<           +a[0,y+1] +a[1,y+1]
                                                                  +a[1,y  ]
                                                        +a[0,y-1] +a[1,y-1];
                    b [0, y] = Test (temp);

                    
                    temp = Init [a [w-1, y]] <<         +a[w-2,y+1] +a[w-1,y+1]
                                                        +a[w-2,y  ]
                                                        +a[w-2,y-1] +a[w-1,y-1];
                    b [w-1, y] = Test (temp);
                }

                temp = Init [a [0, 0]] <<               +a[0,1] +a[1,1]
                                                                +a[1,0];
                b [0, 0] = Test (temp);

                
                temp = Init [a [0, h-1]] <<                       +a[1,h-1]
                                                        +a[0,h-2] +a[1,h-2];
                b [0, h-1] = Test (temp);

                
                temp = Init [a [w-1, h-1]] <<           +a[w-2,h-1]          
                                                        +a[w-2,h-2] +a[w-1,h-2];
                b [w-1, h-1] = Test (temp);

                
                temp = Init [a [w-1, 0]] <<             +a[w-2,1] +a[w-1,1]
                                                        +a[w-2,0];
                b [w-1, 0] = Test (temp);

                
                gen.Land = b;

                gen._completed += w * h * EstimationCoeff;
                return --_iterations == 0;
            }

            
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            private byte Test (uint temp) {
                return (byte) ((_rules & temp) != 0 ? 1 : 0);
            }

        }

    }

}