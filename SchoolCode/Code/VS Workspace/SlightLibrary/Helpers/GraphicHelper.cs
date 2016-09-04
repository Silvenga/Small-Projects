using System.IO;
using System.Windows.Media;


namespace SlightLibrary.Helpers {

    public static class GraphicHelper {

        /// <summary>
        /// Get system icon for given file
        /// TODO: Error handling for bad file name
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static System.Drawing.Icon GetIconOfFile(FileInfo filePath) {

            return System.Drawing.Icon.ExtractAssociatedIcon(filePath.FullName);
        }

        /// <summary>
        /// Parse Color from hex
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color ParseColor(string hex) {

            Color color = default(Color);

            var convertFromString = ColorConverter.ConvertFromString(hex);
            if (convertFromString != null)
                color = (Color) convertFromString;

            return color;
        }

        /// <summary>
        /// Create random color with given alfa
        /// </summary>
        /// <param name="alfa"></param>
        /// <returns></returns>
        public static Color GetRandomColor(byte alfa = 255) {

            return new Color {
                A = alfa,
                B = (byte) MathHelper.GetRandom(1, 255),
                G = (byte) MathHelper.GetRandom(1, 255),
                R = (byte) MathHelper.GetRandom(1, 255)
            };
        }
    }

}