using Core;


namespace Geometry {

    public static class PrimitiveUtils {

//        private delegate void OffsetGetter (Primitive a, Primitive b, ref XY offset);
//        private delegate bool OverlapDetector (Primitive a, Primitive b, XY offset);
//        private delegate XY NormalGetter (Primitive a, Primitive b);


        public static void GetOffsetNormal (Primitive a, Primitive b, ref Math.XY offset, out Math.XY normal) {
            if (Overlap(a, b, offset)) {
                float lo = 0;
                float hi = 1;
                float len = offset.Length;
                for (
                    int i = 0;
                    i < Settings.NumericMethodsIterations && (hi - lo) * len > Settings.NumericMethodsPrecision;
                    i++
                ) {
                    float mid = 0.5f * (lo + hi);
                    bool overlap = Overlap (a, b, mid * offset);
                    if (overlap) hi = mid;
                    else         lo = mid;
                }
                offset *= lo;
            }
            normal = GetNormal(a, b, offset);
        }


        private static bool Overlap (Primitive a, Primitive b, Math.XY offset) {
            switch (a.Type) {
                case Primitive.PType.Circle:
                    switch (b.Type) {
                        case Primitive.PType.Circle:
                            float rr = a.R + b.R;
                            float d2 = Math.XY.SqrDistance (new Math.XY (a.X, a.Y) + offset, new Math.XY (b.X, b.Y));
                            return d2 < rr * rr;
                        case Primitive.PType.Left:   return a.X + a.R + offset.X > b.X;
                        case Primitive.PType.Right:  return a.X - a.R + offset.X < b.X;
                        case Primitive.PType.Top:    return a.Y - a.R + offset.Y < b.Y;
                        case Primitive.PType.Bottom: return a.Y + a.R + offset.Y > b.Y;
                        default:                     return false;
                    }
                case Primitive.PType.Left:
                    switch (b.Type) {
                        case Primitive.PType.Circle: return a.X + offset.X < b.X + b.R;
                        case Primitive.PType.Right:  return a.X + offset.X < b.X;
                        default:                     return false;
                    }
                case Primitive.PType.Right:
                    switch (b.Type) {
                        case Primitive.PType.Circle: return a.X + offset.X > b.X - b.R;
                        case Primitive.PType.Left:   return a.X + offset.X > b.X;
                        default:                     return false;
                    }
                case Primitive.PType.Top:
                    switch (b.Type) {
                        case Primitive.PType.Circle: return a.Y + offset.Y > b.Y - b.R;
                        case Primitive.PType.Bottom: return a.Y + offset.Y > b.Y;
                        default:                     return false;
                    }
                case Primitive.PType.Bottom:
                    switch (b.Type) {
                        case Primitive.PType.Circle: return a.Y + offset.Y < b.Y + b.R;
                        case Primitive.PType.Top:    return a.Y + offset.Y < b.Y;
                        default:                     return false;
                    }
                default: return false;
            }
        }


        private static Math.XY GetNormal (Primitive a, Primitive b, Math.XY offset) {
            switch (a.Type) {
                case Primitive.PType.Circle:
                    switch (b.Type) {
                        case Primitive.PType.Circle: return new Math.XY (a.X - b.X, a.Y - b.Y) + offset;
                        case Primitive.PType.Left:   return Math.XY.Left;
                        case Primitive.PType.Right:  return Math.XY.Right;
                        case Primitive.PType.Top:    return Math.XY.Up;
                        case Primitive.PType.Bottom: return Math.XY.Down;
                        default:                     return Math.XY.NaN;
                    }
                case Primitive.PType.Left:   return Math.XY.Right;
                case Primitive.PType.Right:  return Math.XY.Left;
                case Primitive.PType.Top:    return Math.XY.Down;
                case Primitive.PType.Bottom: return Math.XY.Up;
                default:                     return Math.XY.NaN;
            }
        }

    }

}
