namespace SlightLibrary.Extensions {

    public static class BooleanExtensions {

        public static bool Toggle(this bool boolean) {

            return !boolean;
        }

        public static int ToInt(this bool boolean) {

            return (boolean) ? 1 : 0;
        }
    }

}