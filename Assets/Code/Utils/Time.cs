using Core;
using UnityEngine;


namespace Utils {

    public static class Time {

        public static int Seconds (int   seconds, int ticks = 0) => seconds * Settings.FPS + ticks;
        public static int Seconds (float seconds)                => Mathf.RoundToInt (seconds * Settings.FPS);


        // нет проверки на 0
        public static int ToSeconds     (int time) => time / Settings.FPS;
        public static int CeilToSeconds (int time) => (time + Settings.FPS - 1) / Settings.FPS;

    }

}
