#region Usings

using System;
using System.Collections.Generic;

using MathNet.Symbolics;

#endregion

namespace Functions {

    public static class FiniteFunctions {

        public static Expression Choose(Expression n, int k, int sign = -1) {

            if(k < 1)
                return 1;

            return (n.Factorial(k - 1, sign)) / (k.Factorial());
        }

        public static Expression ChooseWithDiff(Expression h, int k, int times, int sign) {

            if(k < 1)
                return 0;

            var u = Expression.Symbol("u");

            var d = (u.Factorial(k - 1, sign));

            for(var i = 0; i < times; i++) {

                d = Calculus.Differentiate(u, d);
            }

            var result = d / k.Factorial();


            var floatingPoint = Evaluate.Evaluate(
                new Dictionary<string, FloatingPoint> {
                    {
                        "u", FloatingPoint.NewReal(h.ToReal())
                    }
                },
                result);

            var frac = (Expression) floatingPoint.RealValue.ToFraction();// k.Factorial();


            return frac;
        }

        public static Expression Factorial(this Expression value, int start, int sign) {

            if(start < 1) {
                return value;
            }
            return (value + (start * sign)) * Factorial(value, start - 1, sign);
        }

        public static int Factorial(this int value) {

            if(value <= 1)
                return 1;
            return value * Factorial(value - 1);
        }

    }

}