using System;
using System.Collections.Generic;

namespace SlightLibrary.Extensions {

    /// <summary>
    /// Contains Exstentions for any Numeric objects
    /// </summary>
    public static class NumericExtensions {

        /// <summary>
        /// thisInt ^ power
        /// </summary>
        /// <param name="baseInt"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static int ToPower(this int baseInt, int power) {

            int newValue = baseInt;
            for (int i = 1; i < power; ++i)
                newValue *= baseInt;

            return newValue;
        }

        /// <summary>
        /// Get the factors of thisInt
        /// </summary>
        /// <param name="givenNumber"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetFactors(this int givenNumber) {

            List<int> factors = new List<int>();
            int max = (int) Math.Sqrt(givenNumber);

            for (int i = 1; i <= max; ++i) {
                if (givenNumber % i != 0)
                    continue;

                factors.Add(i);

                if (i != givenNumber / i)
                    factors.Add(givenNumber / i);
            }
            return factors;
        }
    }
}
