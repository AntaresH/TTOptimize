using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI
{
    public static class ColorHelper
    {
        public static readonly RgbColor DefaultConsoleForeground = new RgbColor(204, 204, 204);
        public static readonly RgbColor DefaultConsoleBackground = new RgbColor(0, 0, 0);

        public static readonly RgbColor Orange = new RgbColor(204, 85, 0);
        public static readonly RgbColor Turqoise = new RgbColor(0, 128, 128);
        public static readonly RgbColor DarkMagenta = new RgbColor(160, 0, 80);
        public static readonly RgbColor WarmWhite = new RgbColor(255, 250, 220);
        public static readonly RgbColor FullWhite = new RgbColor(255, 255, 255);
        public static readonly RgbColor DarkRed = new RgbColor(80, 0, 0);

        public static readonly RgbColor LightSetYellow = new RgbColor(208, 208, 0);
        public static readonly RgbColor LightSetBlue = new RgbColor(80, 145, 255);
        public static readonly RgbColor LightSetGreen = new RgbColor(0, 220, 50);
        public static readonly RgbColor LightSetBrown = new RgbColor(145, 92, 42);
        public static readonly RgbColor LightSetRed = new RgbColor(220, 80, 80);
        public static readonly RgbColor LightSetDefault = new RgbColor(192, 192, 192);
        public static readonly RgbColor DarkSetYellow = new RgbColor(138, 116, 0);
        public static readonly RgbColor DarkSetBlue = new RgbColor(30, 60, 165);
        public static readonly RgbColor DarkSetGreen = new RgbColor(0, 125, 30);
        public static readonly RgbColor DarkSetBrown = new RgbColor(100, 45, 0);
        public static readonly RgbColor DarkSetRed = new RgbColor(180, 10, 20);
        public static readonly RgbColor DarkSetDefault = new RgbColor(96, 96, 96);

        public static RgbColor DarkSetColor(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => DarkSetYellow,
                DevelopmentColor.Blue => DarkSetBlue,
                DevelopmentColor.Green => DarkSetGreen,
                DevelopmentColor.Brown => DarkSetBrown,
                DevelopmentColor.Red => DarkSetRed,
                _ => DarkSetDefault
            };
        }

        public static RgbColor LightSetColor(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => LightSetYellow,
                DevelopmentColor.Blue => LightSetBlue,
                DevelopmentColor.Green => LightSetGreen,
                DevelopmentColor.Brown => LightSetBrown,
                DevelopmentColor.Red => LightSetRed,
                _ => LightSetDefault
            };
        }

        public static string GetRgbColorString(RgbColor color)
        {
            return $"{color.R};{color.G};{color.B}";
        }
    }
}
