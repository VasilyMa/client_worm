using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Math;
using UnityEngine;
using Random = System.Random;


namespace Utils {

    public static class RandomExtension {

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int Int (this Random r) => r.Next ();


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int Int (this Random r, int max) => r.Next (max);


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int Int (this Random r, int min, int max) => r.Next (min, max);


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Float (this Random r) => (float) r.NextDouble ();


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Float (this Random r, float max) => r.Float () * max;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Float (this Random r, float min, float max) => Mathf.LerpUnclamped (min, max, r.Float ());


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float SignedFloat (this Random r) => r.Float () - 0.5f;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Angle (this Random r) => r.Float () * 2 * Mathf.PI;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool Bool (this Random r) => r.Next (2) == 0;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool Bool (this Random r, float chance) => r.Float () < chance;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool Bool (this Random r, int chance, int outOf) => r.Next (outOf) < chance;


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Round (this Random r, float value) => Mathf.Floor (value + r.Float ());


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt (this Random r, float value) => Mathf.FloorToInt (value + r.Float ());


        public static XY Point (this Random r, Box box) =>
        new XY (
            r.Float (box.X0, box.X1),
            r.Float (box.Y0, box.Y1)
        );


        public static void Shuffle <T> (this Random r, IList <T> list) {
            for (int i = list.Count; i > 1;) {
                int j = r.Next (i);
                var t = list [--i];
                list [i] = list [j];
                list [j] = t;
            }
        }


        public static void Shuffle <T> (this Random r, IList <T> list, int elementsToShuffle) {
            for (int i = 0, end = System.Math.Min (list.Count - 1, elementsToShuffle); i < end; i++) {
                int j = r.Next (i, list.Count);
                var t = list [i];
                list [i] = list [j];
                list [j] = t;
            }
        }

    }

}
