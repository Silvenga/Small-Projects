using System;
using System.Collections.Generic;
using System.Linq;
using SlightLibrary.Bases;
using SlightLibrary.Helpers;

namespace SlightLibrary.Extensions {

    /// <summary>
    /// Contains Enum Extensions
    /// </summary>
    public static class EnumExtensions {

        /// <summary>
        /// Get any EnumDerivable classes from an Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum data) where T : EnumDerivable {

            Type dataType = data.GetType();
            string name = Enum.GetName(dataType, data);
            return dataType.GetField(name).GetCustomAttributes(false).OfType<T>().SingleOrDefault();
        }

        public static T GetRandom<T>() {

            T[] values = (T[]) Enum.GetValues(typeof(T));
            return values[new Random().Next(0, values.Length)];
        }
    }
}