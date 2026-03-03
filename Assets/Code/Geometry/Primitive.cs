using Math;


namespace Geometry {

    public struct Primitive {

        public enum PType { None, Circle, Left, Right, Top, Bottom }


        public static readonly Primitive None = new Primitive (PType.None, float.NaN, float.NaN, float.NaN);
        public PType Type;
        public float X, Y, R;


        private Primitive (PType type, float x, float y, float r) {
            Type = type;
            X = x;
            Y = y;
            R = r;
        }


        public bool IsEmpty => Type == PType.None;


        public static Primitive Circle (float x, float y, float r = 0) => new Primitive (PType.Circle, x,   y,   r  );
        public static Primitive Circle (Circle c)                      => new Primitive (PType.Circle, c.X, c.Y, c.R);
        public static Primitive Circle (XY p, float r = 0)             => new Primitive (PType.Circle, p.X, p.Y, r  );


        public static Primitive Left   (float x) => new Primitive (PType.Left,   x, float.NaN, float.NaN);
        public static Primitive Right  (float x) => new Primitive (PType.Right,  x, float.NaN, float.NaN);
        public static Primitive Top    (float y) => new Primitive (PType.Top,    float.NaN, y, float.NaN);
        public static Primitive Bottom (float y) => new Primitive (PType.Bottom, float.NaN, y, float.NaN);


        public override string ToString () => $"[ {Type} : {X:R}, {Y:R}, {R:R} ]";

    }

}
