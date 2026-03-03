using System;


namespace Math {

    public struct IntXY : IEquatable <IntXY> {

        public int X, Y;


        public IntXY (int x, int y) {
            X = x;
            Y = y;
        }


        public static IntXY operator + (IntXY a)          => new IntXY (+a.X,      +a.Y);
        public static IntXY operator - (IntXY a)          => new IntXY (-a.X,      -a.Y);
        public static IntXY operator + (IntXY a, IntXY b) => new IntXY (a.X + b.X, a.Y + b.Y);
        public static IntXY operator - (IntXY a, IntXY b) => new IntXY (a.X - b.X, a.Y - b.Y);
        public static IntXY operator * (int d, IntXY a)   => new IntXY (a.X * d,   a.Y * d);
        public static IntXY operator * (IntXY a, int d)   => new IntXY (a.X * d,   a.Y * d);
        public static IntXY operator / (IntXY a, int d)   => new IntXY (a.X / d,   a.Y / d);

        public static bool operator != (IntXY a, IntXY b) => !a.Equals (b);
        public static bool operator == (IntXY a, IntXY b) =>  a.Equals (b);

        public static explicit operator XY (IntXY p) => new XY (p.X, p.Y);


        public static IntXY Zero  => new IntXY ( 0,  0);
        public static IntXY One   => new IntXY ( 1,  1);
        public static IntXY Down  => new IntXY ( 0,  1);
        public static IntXY Left  => new IntXY (-1,  0);
        public static IntXY Up    => new IntXY ( 0, -1);
        public static IntXY Right => new IntXY ( 1,  0);


        public          bool Equals (IntXY other) => X == other.X && Y == other.Y;
        public override bool Equals (object obj)  => obj is IntXY && Equals ((IntXY) obj);


        public override int GetHashCode () {
            unchecked {
                return X * 397 ^ Y;
            }
        }

    }

}
