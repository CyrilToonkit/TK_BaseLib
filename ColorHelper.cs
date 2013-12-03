using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TK.BaseLib
{
    /// <summary>
    /// Tells if a color is named of defined by its Alpha, Red, Green and Blue Values
    /// </summary>
    public enum ColorFormat
    {
        NamedColor,
        ARGBColor
    }

    public class ColorHelper
    {
        public static Color BlendColors(Color color1, Color color2, double factor)
        {
            int r = Math.Max(0, Math.Min(255, (int)(color1.R * (1 - factor) + color2.R * factor)));
            int g = Math.Max(0, Math.Min(255, (int)(color1.G * (1 - factor) + color2.G * factor)));
            int b = Math.Max(0, Math.Min(255, (int)(color1.B * (1 - factor) + color2.B * factor)));

            return Color.FromArgb(r, g, b);
        }
    }
}
