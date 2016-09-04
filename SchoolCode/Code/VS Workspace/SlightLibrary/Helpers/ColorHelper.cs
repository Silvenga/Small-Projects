using System;
using SlightLibrary.Entities;

namespace SlightLibrary.Helpers {

    public static class ColorHelper {

        // http://stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv

        public static HSVColor ColorToHSV(System.Windows.Media.Color color) {

            return ColorToHSV(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public static HSVColor ColorToHSV(System.Drawing.Color color) {

            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            double hue = color.GetHue();
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HSVColor {
                Hue = hue,
                Saturation = saturation,
                Value = value
            };
        }

        public static System.Windows.Media.Color ColorFromHSV(HSVColor hsvColor) {

            return ColorFromHSV(hsvColor.Hue, hsvColor.Saturation, hsvColor.Value).FromColor();
        }

        public static System.Drawing.Color ColorFromHSV(double hue, double saturation, double value) {

            if (Double.IsInfinity(hue) || Double.IsNaN(hue))
                return default(System.Drawing.Color);

            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi) {
                case 0:
                    return System.Drawing.Color.FromArgb(255, v, t, p);
                case 1:
                    return System.Drawing.Color.FromArgb(255, q, v, p);
                case 2:
                    return System.Drawing.Color.FromArgb(255, p, v, t);
                case 3:
                    return System.Drawing.Color.FromArgb(255, p, q, v);
                case 4:
                    return System.Drawing.Color.FromArgb(255, t, p, v);
                default:
                    return System.Drawing.Color.FromArgb(255, v, p, q);
            }
        }

        public static System.Windows.Media.Color FromColor(this System.Drawing.Color otherColor) {

            return new System.Windows.Media.Color {
                A = otherColor.A,
                B = otherColor.B,
                G = otherColor.G,
                R = otherColor.R
            };
        }

        public static System.Drawing.Color FromColor(this System.Windows.Media.Color otherColor) {

            return System.Drawing.Color.FromArgb(otherColor.A, otherColor.R, otherColor.G, otherColor.B);
        }
    }
}
