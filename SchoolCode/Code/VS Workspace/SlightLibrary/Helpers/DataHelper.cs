using System;

namespace SlightLibrary.Helpers {

    public static class DataHelper {

        /// <summary>
        /// Atempts to convert data to given data type, 
        /// Returns true if successful while changing T export to converted data
        /// If failed, returns false while changing export to default of given data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="export"></param>
        /// <returns></returns>
        public static bool TryTypeConvert<T>(Object data, out T export) {

            bool succesful = false;
            try {
                if (!string.IsNullOrEmpty(data.ToString())) {
                    export = (T) Convert.ChangeType(data, typeof(T));
                    succesful = true;
                } else {
                    throw new FormatException();
                }
            } catch (Exception) {
                export = default(T);
            }
            return succesful;
        }
        /// <summary>
        /// Get a uri from compiled resources relitive to root directory.
        /// /subfolder/image.png
        /// </summary>
        /// <param name="pathRelativeToRoot"></param>
        /// <returns></returns>
        public static Uri UriFromRoot(string pathRelativeToRoot) {

            return new Uri("pack://application:,,," + pathRelativeToRoot);
        }
    }

}