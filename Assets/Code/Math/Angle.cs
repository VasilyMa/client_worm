using UnityEngine;


namespace Math {

    public static class Angle {
        
        public static float Delta (float from, float to, float pi = Mathf.PI) {
            float _2pi = 2 * pi;
            float num = Mathf.Repeat(to - from, _2pi);
            if (num > pi) num -= _2pi;
            return num;
        }


        public static float MoveTowards (float from, float to, float step, float pi = Mathf.PI) {
            float delta = Delta (from, to, pi);
            if (-step < delta && delta < step) return to;
            return Mathf.MoveTowards (from, from + delta, step);
        }

    }

}
