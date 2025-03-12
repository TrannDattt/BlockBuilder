using BuilderTool.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderTool.Helpers
{
    public static class ColorMapper
    {
        public static Dictionary<EColor, Color> ColorDict { get; private set; } = new Dictionary<EColor, Color>
        {
            { EColor.Black, Color.clear },
            { EColor.LightGreen, Color.green},
            { EColor.Cyan, Color.cyan },
            { EColor.Yellow, Color.yellow },
            { EColor.Selected, new Color(0, 1, 0, .5f) }
        };

        public static Color GetColor(EColor color)
        {
            return ColorDict[color];
        }
    }
}
