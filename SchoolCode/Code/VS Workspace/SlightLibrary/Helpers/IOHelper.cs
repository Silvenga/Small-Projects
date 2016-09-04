using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Newtonsoft.Json;

using SlightLibrary.Extensions;

namespace SlightLibrary.Helpers {

    public static class IOHelper {

        /// <summary>
        /// If any errors are found, give the user this message
        /// </summary>
        public static string ErrorMessage {
            get;
            set;
        }

        /// <summary>
        /// Sets the default values
        /// </summary>
        static IOHelper() {

            ErrorMessage = "Bad Input.";
        }

        /// <summary>
        /// Save object to file
        /// TODO: Create better error handling
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public static void SaveObject(Object obj, string fileName) {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        /// <summary>
        /// Read object from file
        /// TODO: Create better error handling
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Object ReadObject(string fileName) {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            object obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        /// <summary>
        /// Read object from file and preform basic casting
        /// TODO: Create better error handling
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T ReadObject<T>(string fileName) {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            T obj = (T) formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        /// <summary>
        /// Pause program, clone of CMD PAUSE function
        /// </summary>
        /// <param name="message"></param>
        public static void Pause(string message = "Press any key to continue... ") {

            Console.WriteLine(message);
            Console.ReadKey();
        }

        /// <summary>
        /// Attempts to read a line from the command line, then attempt to parse. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="export"></param>
        /// <returns></returns>
        public static bool TryReadLine<T>(out T export) {

            return DataHelper.TryTypeConvert(Console.ReadLine(), out export);
        }

        /// <summary>
        /// Prompt for the given type, display message to user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T PromptForInput<T>(string message) {

            Console.WriteLine(message);
            T returnData;
            while(!TryReadLine(out returnData))
                Console.WriteLine(ErrorMessage);

            return returnData;
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T PromptForInput<T>(string message, T min, T max) where T : IComparable {

            Console.WriteLine(message);
            T returnData;
            while(!(TryReadLine(out returnData) && returnData.IsGreaterThanEqual(min) && returnData.IsLessThanEqual(max)))
                Console.WriteLine(ErrorMessage);

            return returnData;
        }

        /// <summary>
        /// Prompt for String while displaying message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string PromptForInputString(string message) {

            return PromptForInput<string>(message);
        }

        /// <summary>
        /// Prompt or a bool vaule
        /// </summary>
        /// <param name="message">Message to give to user</param>
        /// <param name="answerTrue">What answer should be considered true</param>
        /// <param name="answerFalse">What answer should be considered false</param>
        /// <returns></returns>
        public static bool PromptForInputBool(string message, string answerTrue, string answerFalse) {

            Console.WriteLine(message);
            string returnData;
            while(!((returnData = Console.ReadLine()) == answerFalse || returnData == answerTrue)) {
                Console.WriteLine(ErrorMessage);
            }
            return returnData == answerTrue;
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int PromptForInputInt(string message, int min = int.MinValue, int max = int.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double PromptForInputDouble(string message, double min = double.MinValue, double max = double.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static short PromptForInputShort(string message, short min = short.MinValue, short max = short.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float PromptForInputFloat(string message, float min = float.MinValue, float max = float.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static decimal PromptForInputDecimal(string message, decimal min = decimal.MinValue, decimal max = decimal.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static long PromptForInputLong(string message, long min = long.MinValue, long max = long.MaxValue) {

            return PromptForInput(message, min, max);
        }

        /// <summary>
        /// The Min, Max range is inclusive.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static char PromptForInputChar(string message, char min = char.MinValue, char max = char.MaxValue) {

            return PromptForInput(message, min, max);
        }

        public static void Save<T>(this T obj, string file) {

            using(var stream = File.OpenWrite(file)) {

                using(var writer = new StreamWriter(stream)) {

                    var serializer = new JsonSerializer();

                    serializer.Serialize(writer, obj);
                }
            }
        }

        public static T Load<T>(string file) {

            using(var stream = File.OpenRead(file)) {

                using(var reader = new StreamReader(stream)) {

                    var serializer = new JsonSerializer();

                    return (T) serializer.Deserialize(reader, typeof(T));
                }
            }
        }

    }

}