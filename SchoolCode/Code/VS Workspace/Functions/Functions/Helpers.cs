
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

using Jint;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Symbolics;

using Evaluate = MathNet.Symbolics.Evaluate;

namespace Functions {

    public static class Helpers {

        public static bool IsNegative(this double d) {

            return d < 0;
        }

        public static Expression Subsitute(this Expression function, string symbol, Expression value) {

            return Evaluate.Evaluate(
                   new Dictionary<string, FloatingPoint> {
                    {
                        symbol, value.ToReal()
                    }
                },
                   function).RealValue.ToFraction();
        }

        public static Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(List<TKey> keys, List<TValue> values) {

            if(values.Count() != keys.Count()) {
                throw new ArgumentException("Keys and Values must be the same size.");
            }

            return keys.Zip(
                values,
                (k, v) => new {
                    k,
                    v
                })
                .ToDictionary(x => x.k, x => x.v);
        }

        public static double ToReal(this Expression expression, int places = 5) {

            var value = double.NaN;

            if(expression.IsNumber) {

                var empty = new Dictionary<string, FloatingPoint>();

                value = Evaluate.Evaluate(empty, expression).RealValue;
            }

            return value.Round(places);
        }

        public static double Round(this double d, int places = 3) {

            return Math.Round(d, places);
        }

        public static Expression F(this string str) {

            var s = str.Split('/');
            var top = int.Parse(s.First());
            var bot = int.Parse(s.Last());

            return BigRational.FromIntFraction(top, bot);
        }

        public static Expression P(this object str) {

            return Infix.ParseOrUndefined(str.ToString());
        }

        public static List<Expression> P(this IEnumerable<double> list) {

            return list.Select(d => d.P()).ToList();
        }

        public static Dictionary<Expression, Expression> P(this Dictionary<double, double> list) {

            return list.ToDictionary(x => x.Key.P(), x => x.Value.P());
        }

        public static Expression P(this double value) {

            //var str = "" + value;
            //var decimals = str.Split('.').Last().Count();

            //var dem = Math.Pow(10, decimals);

            //value *= dem;

            //return BigRational.FromIntFraction((int) value, (int) dem);
            return value.ToFraction();
        }

        public static BigRational ToFraction(this double x, double error = 0.0000000001) {

            int num;
            int den;

            var n = (int) Math.Floor(x);
            x -= n;

            if(x < error) {
                num = n;
                den = 1;
                return BigRational.FromIntFraction(num, den);
            }

            if(1 - error < x) {
                num = n + 1;
                den = 1;
                return BigRational.FromIntFraction(num, den);
            }

            var lowerN = 0;
            var lowerD = 1;

            var upperN = 1;
            var upperD = 1;

            while(true) {

                var middleN = lowerN + upperN;
                var middleD = lowerD + upperD;

                if(middleD * (x + error) < middleN) {

                    upperN = middleN;
                    upperD = middleD;

                } else if(middleN < (x - error) * middleD) {

                    lowerN = middleN;
                    lowerD = middleD;

                } else {

                    num = n * middleD + middleN;
                    den = middleD;
                    return BigRational.FromIntFraction(num, den);
                }
            }

            //BigRational frac;
            //if(x % 1 == 0)	// if whole number
            //        {
            //    frac = BigRational.FromIntFraction((int) x, 1);
            //} else {
            //    double dTemp = x;
            //    int iMultiple = 1;
            //    string strTemp = x.ToString();
            //    while(strTemp.IndexOf("E") > 0)	// if in the form like 12E-9
            //            {
            //        dTemp *= 10;
            //        iMultiple *= 10;
            //        strTemp = dTemp.ToString();
            //    }
            //    int i = 0;
            //    while(strTemp[i] != '.')
            //        i++;
            //    int iDigitsAfterDecimal = strTemp.Length - i - 1;
            //    while(iDigitsAfterDecimal > 0) {
            //        dTemp *= 10;
            //        iMultiple *= 10;
            //        iDigitsAfterDecimal--;
            //    }
            //    frac = BigRational.FromIntFraction((int) Math.Round(dTemp), iMultiple);
            //}
            //return frac;
        }

        public static double Abs(this double value) {

            return Math.Abs(value);
        }

        public static void Round(this Matrix<double> matrix, int places = 5) {

            for(var col = 0; col < matrix.ColumnCount; col++) {

                for(var row = 0; row < matrix.RowCount; row++) {

                    var currentValue = matrix.At(row, col);

                    currentValue = Math.Round(currentValue, places);

                    matrix.At(row, col, currentValue);
                }
            }
        }

        public static void Print(this object obj) {

            Console.WriteLine(obj);
        }


        public static void Print(this Expression obj) {

            Infix.Print(obj).Print();
        }

        public static Vector<double> ToVector(this IEnumerable<double> list) {

            return DenseVector.OfEnumerable(list);
        }

        public static Expression Solve(this Expression equation, Expression result) {

            if(equation.IsNumber)
                return Expression.Undefined;

            if(!result.IsNumber)
                throw new ArgumentException("Result must be a number.");

            if(equation.IsIdentifier)
                return result;

            if(equation.IsSum) {

                var tmp = (Expression.Sum) equation;

                var num = tmp.Item.First(x => x.IsNumber);

                equation -= num;
                result -= num;

            } else if(equation.IsProduct) {

                var tmp = (Expression.Product) equation;

                var num = tmp.Item.First(x => x.IsNumber);

                equation /= num;
                result /= num;
            } else if(equation.IsPower) {

                throw new Exception("Cannot not handle Powers, yet.");
            }

            return Solve(equation, result);
        }

        public static Expression FindN(Expression low, Expression high, Expression h) {

            var n = Expression.Symbol("n");

            return (h * n).Solve((high - low));
        }

    }

    public class Func {

        public string Equation {
            get;
            set;
        }

        public string Libs {
            get;
            set;
        }

        public Engine Engine {
            get;
            set;
        }

        public Func(string equation) {

            Equation = equation;

            Libs = File.ReadAllText("math.js");

            Engine = new Engine().Execute(Libs);
        }

        public double Invoke(double x) {

            var str = "math.eval('" + Equation + "', { x: x });";

            var result = Engine
                .SetValue("x", x)
                .Execute(str)
                .GetCompletionValue()
                .ToObject();

            return double.Parse(result.ToString());
        }



    }
}
