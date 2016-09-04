using System;
using SlightLibrary.Entities;
using SlightLibrary.Exceptions;

namespace SlightLibrary.Helpers {

    public static class MathHelper {

        private static readonly Random Random = new Random();

        /// <summary>
        /// Get the greatest common facter of two numbers
        /// Throws 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int GetGFC(int m, int n) {

            int retVal;
            if (m < 1 || n < 1)
                throw new InputDataException("Both numbers given must be greater than 1");

            if (m == n) {
                retVal = n;
            } else {

                if (m < n) {

                    int temp = m;
                    m = n;
                    n = temp;
                }
                retVal = m % n == 0 ? n : GetGFC(n, m % n);
            }
            return retVal;
        }

        public static double DoMathOperation(double one, double two, MathOperation operation) {

            switch (operation) {
                case MathOperation.Addition:
                    return one + two;
                case MathOperation.Multiplication:
                    return one * two;
                default:
                    throw new ArgumentOutOfRangeException("operation");
            }
        }

        /// <summary>
        /// Get random int between given min/max ints
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int min, int max) {

            return Random.Next(min, max + 1);
        }

        /// <summary>
        /// Get random boolean with a weighted chance.
        /// </summary>
        /// <param name="chanceOfTrue"></param>
        /// <returns></returns>
        public static bool GetRandomBoolean(int chanceOfTrue = 50) {

            return (GetRandom(1, 100) <= chanceOfTrue);
        }

        /// <summary>
        /// Get random decimal bettween given min/max with given amount of significant decimal places 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="significantDecimal"></param>
        /// <returns></returns>
        public static Decimal GetRandom(Decimal min, Decimal max, int significantDecimal) {

            Decimal newNumber = 0;

            while (!(newNumber <= max && newNumber >= min)) {

                newNumber = Decimal.Round(Decimal.Parse(GetRandom(Decimal.ToInt32(min), Decimal.ToInt32(max)) + "." + Random.Next()), significantDecimal);
            }
            return newNumber;
        }
    }

}