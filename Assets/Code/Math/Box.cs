using static UnityEngine.Mathf;


namespace Math {

    public struct Box {

        public float X0, Y0, X1, Y1;


        public Box (float x0, float x1, float y0, float y1) {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
        }


        public static Box operator * (float f, Box b) => new Box (b.X0 * f, b.X1 * f, b.Y0 * f, b.Y1 * f);
        public static Box operator * (Box b, float f) => new Box (b.X0 * f, b.X1 * f, b.Y0 * f, b.Y1 * f);
        public static Box operator / (Box b, float f) => new Box (b.X0 / f, b.X1 / f, b.Y0 / f, b.Y1 / f);


        // уже подготовленный для обычных циклов for объект
        public IntBox ToTiles (float size) =>
        new IntBox (
            CeilToInt  (X0 / size) - 1,
            FloorToInt (X1 / size) + 1,
            CeilToInt  (Y0 / size) - 1,
            FloorToInt (Y1 / size) + 1
        );


        public void Expand (XY v) {
            if (v.X < 0) X0 += v.X;
            else         X1 += v.X;
            
            if (v.Y < 0) Y0 += v.Y;
            else         Y1 += v.Y;
        }


        public Box Expanded (XY v) {
            var result = this;
            result.Expand (v);
            return result;
        }


        public static bool Overlap (Box a, Box b) =>
        a.X0 < b.X1 && b.X0 < a.X1 &&
        a.Y0 < b.Y1 && b.Y0 < a.Y1;

    }

}
