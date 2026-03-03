using System;
using UnityEngine;


namespace Utils {

    [Obsolete]
    public struct OldTime {

        [Obsolete]
        public const int TPS = 60;

        public int Ticks;


        public float Seconds {
            get { return TicksToSeconds (Ticks); }
            set { Ticks = SecondsToTicks (value); }
        }


        public override string ToString () => Mathf.CeilToInt (Seconds).ToString ();


        public string ToString (int maxSecondsDisplayed) {
            int seconds = Mathf.CeilToInt (Seconds);
            return seconds > maxSecondsDisplayed ? "" : seconds.ToString ();
        }


        public static float TicksToSeconds (int ticks)     => (float) ticks / TPS;
        public static int   SecondsToTicks (int seconds)   => seconds * TPS;
        public static int   SecondsToTicks (float seconds) => Mathf.RoundToInt (seconds * TPS);


//        public static string SecondsToString (int seconds, int maxSecondsDisplayed) => ""; 
//        public static string SecondsToString (float seconds, int maxSecondsDisplayed) => "";


        public static string TicksToString (int ticks) {
            if (ticks >= 0) {
                return $"{ticks / TPS}:{ticks % TPS:00}";
            }
            ticks = System.Math.Abs (ticks);
            return $"-{ticks / TPS}:{ticks % TPS:00}";
        }


        public static bool operator == (OldTime a, OldTime b) => a.Ticks == b.Ticks;
        public static bool operator != (OldTime a, OldTime b) => a.Ticks != b.Ticks;
        public static bool operator <  (OldTime a, OldTime b) => a.Ticks <  b.Ticks;
        public static bool operator >  (OldTime a, OldTime b) => a.Ticks >  b.Ticks;
        public static bool operator <= (OldTime a, OldTime b) => a.Ticks <= b.Ticks;
        public static bool operator >= (OldTime a, OldTime b) => a.Ticks >= b.Ticks;

    }

}