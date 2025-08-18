using System.Collections.Generic;
using UnityEngine;

public static class ColorData
{
    public static Dictionary<PixelColorType, Color> ColorMap = new Dictionary<PixelColorType, Color>
{
    { PixelColorType.Red,        new Color(0.90f, 0.22f, 0.27f) }, // #E63946 🔴
    { PixelColorType.Orange,     new Color(0.96f, 0.64f, 0.38f) }, // #F4A261 🟠
    { PixelColorType.LightYellow,new Color(0.99f, 0.80f, 0.43f) }, // #FDCB6E 🟡
    { PixelColorType.Green,      new Color(0.16f, 0.62f, 0.56f) }, // #2A9D8F 🟢
    { PixelColorType.Blue,       new Color(0.27f, 0.48f, 0.61f) }, // #457B9D 🔵
    { PixelColorType.Violet,     new Color(0.62f, 0.31f, 0.87f) }, // #9D4EDD 🟣
    { PixelColorType.White,      new Color(1.00f, 1.00f, 1.00f) }, // #FFFFFF ⚪
    { PixelColorType.Black,      new Color(0.00f, 0.00f, 0.00f) }, // #000000 ⚫
    { PixelColorType.Brown,      new Color(0.55f, 0.43f, 0.39f) }, // #8D6E63 🟤
    { PixelColorType.Purple,     new Color(0.49f, 0.34f, 0.76f) }  // #7E57C2 🟪
};
}