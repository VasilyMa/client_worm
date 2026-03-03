using System.Collections.Generic;
using UnityEngine;


namespace Core {

    public static class Colors {

        public static readonly Color
        White     = new Color32 (0xff, 0xff, 0xff, 0xff),
        PaleBlue  = new Color32 (0xdd, 0xff, 0xff, 0xff),
        Red       = new Color32 (0xff, 0x77, 0x66, 0xff),
        Orange    = new Color32 (0xff, 0xaa, 0x44, 0xff),
        Yellow    = new Color32 (0xff, 0xff, 0x77, 0xff),
        Green     = new Color32 (0x77, 0xff, 0x66, 0xff),
        DarkGreen = new Color32 (0x00, 0xcc, 0x88, 0xff),
        Cyan      = new Color32 (0x66, 0xff, 0xff, 0xff),
        Blue      = new Color32 (0x00, 0xbb, 0xff, 0xff),
        Violet    = new Color32 (0xaa, 0x99, 0xff, 0xff),
        Pink      = new Color32 (0xff, 0xbb, 0xff, 0xff),
        Brown     = new Color32 (0xbb, 0x99, 0x77, 0xff),
        Gray      = new Color32 (0xcc, 0xcc, 0xcc, 0xff),
        DarkGray  = new Color32 (0x88, 0x88, 0x88, 0xff),
        Black     = new Color32 (0x00, 0x00, 0x00, 0xff),
        Sky       = new Color32 (0x88, 0xcc, 0xee, 0xff),
        NightSky  = new Color32 (0x22, 0x44, 0x66, 0xff);

        
        public static readonly IReadOnlyList <Color> PlayerColors =
        new [] {Red, Green, Blue, Yellow, Violet, Brown, DarkGreen, Orange, Cyan, Pink, DarkGray, Black};
        // цветов 12, но автоматический выбор будет использовать только 10 первых


        public static readonly Color
        UINeutral  = PaleBlue,
        UIActive   = Yellow,
        UIInactive = Gray,
        UISelected = White,
        UIError    = Red;

    }

}
