using SlightLibrary.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SlightLibrary.Extensions {

    /// <summary>
    /// Contains Extensions for all generic types 
    /// </summary>
    public static class GenericExtensions {

        /// <summary>
        /// Prints Object that implemnt IEnumerable
        /// Using Generics for speed improvemnts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void PrintCollection<T>(this T obj) where T : IEnumerable {

            string stringToPrint = obj.Cast<object>().Aggregate("", (current, o) => current + (o + ", "));

            Console.WriteLine(stringToPrint.Substring(0, stringToPrint.Length - 2));
        }

        /// <summary>
        /// Prints This Object to the command line
        /// </summary>
        /// <param name="obj">The object to print using ToString</param>
        public static void ToConsole<T>(this T obj) {

            Console.WriteLine(obj);
        }

        /// <summary>
        /// Converts this object to of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T TypeConvert<T>(this object data) {

            return (T) Convert.ChangeType(data, typeof(T));
        }

        /// <summary>
        /// Converts this object to of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T HandleTypeConvert<T>(this object data) {

            try {
                return (T) Convert.ChangeType(data, typeof(T));
            } catch(Exception) {
                Console.WriteLine("Handled TypeConvert Exception.");
            }
            return default(T);
        }

        /// <summary>
        /// Compares two comparible generics 
        /// Orgin: David Morton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsGreaterThan<T>(this T data, T other) where T : IComparable {

            return data.CompareTo(other) > 0;
        }

        /// <summary>
        /// Compares two comparible generics 
        /// Orgin: David Morton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsLessThan<T>(this T data, T other) where T : IComparable {

            return data.CompareTo(other) < 0;
        }

        /// <summary>
        /// Compares two comparible generics 
        /// Orgin: David Morton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsGreaterThanEqual<T>(this T data, T other) where T : IComparable {

            return data.CompareTo(other) >= 0;
        }

        /// <summary>
        /// Compares two comparible generics 
        /// Orgin: David Morton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsLessThanEqual<T>(this T data, T other) where T : IComparable {
            return data.CompareTo(other) <= 0;
        }

        public static TItem RandomItem<TList, TItem>(this TList list) where TList : List<TItem> {

            return list[MathHelper.GetRandom(0, list.Count - 1)];
        }
    }

}