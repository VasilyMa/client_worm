using System;
using Math;
using UnityEngine;
using Collision = War.Systems.Collisions.Collision;


namespace Geometry {

    public static class Geom {

        // возвращает относительную величину
        // вместо x можно y и норм
        public static float RayTo1D (float oX, float dirX, float targX) => (targX - oX) / dirX;


        // возвращает расстояние, пройденное лучом, может вернуть отрицательное или nan
        // абсолютное значение
        public static float RayToCircle (XY o, XY v, XY cCenter, float r) {
            var oc = cCenter - o;

            // если луч направлен в другую сторону
            if (XY.Dot (oc, v) <= 0) return float.NaN;

            // квадрат расстояния от центра окружности до луча
            float h2 = XY.Cross (oc, v);
            h2 *= h2 / v.SqrLength;

            if (h2 >= r * r) return float.NaN;
            return Mathf.Sqrt (oc.SqrLength - h2) - Mathf.Sqrt (r * r - h2);
        }


        public static XY Bounce (XY velocity, XY normal, float tangentialBounce, float normalBounce) {
            var tangent           = normal.Rotated90CW ();
            var convertedVelocity = ConvertToBasis (velocity, tangent, normal);

            return tangent * tangentialBounce * convertedVelocity.X
                 - normal  * normalBounce     * convertedVelocity.Y;
        }


        public static XY ConvertToBasis (XY v, XY x, XY y) {
            // коэффициенты системы уравнений
            float
            a0 = x.X, b0 = y.X, c0 = v.X,
            a1 = x.Y, b1 = y.Y, c1 = v.Y;

            // определитель матрицы при решении системы методом Крамера
            float d = a0 * b1 - a1 * b0;

            // null - прямые параллельны или совпадают
            return d == 0 ? XY.NaN : new XY (c0 * b1 - c1 * b0, a0 * c1 - a1 * c0) / d;
        }


        public static float Distance    (Box b, XY p)     => Mathf.Sqrt (SqrDistance (b, p));
        public static float SqrDistance (Box b, XY p)     => XY.SqrDistance (p, p.Clamped (b));
        
        public static bool  Overlap     (Circle c, Box b) => SqrDistance (b, c.O) < c.R * c.R;

    }

}
