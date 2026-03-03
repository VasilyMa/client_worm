using static System.Math;


namespace Math {

    public struct IntBox {

        public int X0, X1, Y0, Y1;
        
        
        public IntBox (int x0, int x1, int y0, int y1) {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
        }


        public static IntBox Intersection (IntBox a, IntBox b) {
            return new IntBox (
                Max (a.X0, b.X0),
                Min (a.X1, b.X1),
                Max (a.Y0, b.Y0),
                Min (a.Y1, b.Y1)
            );
        }

    }

}
