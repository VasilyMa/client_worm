namespace Math {

    public struct Circle {

        public float X, Y;
        public float R;


        public Circle (float x, float y, float r) {
            X = x;
            Y = y;
            R = r;
        }


        public Circle (XY o, float r) : this (o.X, o.Y, r) {}


        public XY O {
            get { return new XY (X, Y); }
            set {
                X = value.X;
                Y = value.Y;
            }
        }


        public static bool Overlap (Circle a, Circle b) {
            float d = a.R + b.R;
            return (a.O - b.O).SqrLength < d * d;
        }

    }

}
