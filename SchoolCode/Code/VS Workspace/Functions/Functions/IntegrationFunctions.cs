using System;
using System.Collections.Generic;

using Functions.Models;

using MathNet.Symbolics;

namespace Functions {

    public static class IntegrationFunctions {

        public delegate Expression Sub(Expression x);

        public static double Gauss(int low, int high, List<double> lD, List<double> uD, Sub sub, bool convert = true) {

            const string uStr = "u";

            var l = lD.P();
            var u = uD.P();

            var e = Expression.Symbol(uStr);

            var expression = (convert) ? TrasformationDx(low, high) * sub(TrasformationX(low, high, e)) : sub(e);

            //expression.Print();

            //expression.Print();
            //TrasformationX(low, high, e).Print();
            //TrasformationDx(low, high).Print();

            Expression value = 0;

            for(var i = 0; i < l.Count; i++) {

                value += l[i] * expression.Subsitute(uStr, u[i]);
            }

            (string.Format("With {0} point Quadrature = {1}", lD.Count, value.ToReal())).Print();

            return 0;
        }

        public static Expression TrasformationX(int low, int high, Expression u) {

            Expression two = 2;

            return ((high - low) / two) * u + ((low + high) / two);
        }

        public static Expression TrasformationDx(int low, int high) {

            Expression two = 2;

            return ((high - low) / two);
        }

        public static double RombergTrapezoid(this Lookup expression, Expression low, Expression high, List<Expression> hList, bool convert = false) {

            var ts = new DyamicList<DyamicList<Expression>>();

            for(var i = 0; i < hList.Count; i++) {

                ts[i] = new DyamicList<Expression>();
            }

            var round = 0;

            Console.WriteLine("----------------------- " + round);

            {
                var index = 0;
                foreach(var ex in hList) {

                    var i = convert ? ((high - low) / ex).ToReal() : ex.ToReal();

                    var value = expression.RombergTrapezoidPart(low, high, (int) i).P();

                    ("T" + index + "0: Trapezoid(n=" + i + ") = " + value.ToReal()).Print();

                    ts[round][index++] = value;
                }
            }

            for(var i = 1; i < hList.Count; i++) {

                round++;
                Console.WriteLine("----------------------- " + round);

                for(var j = 0; j < hList.Count - round; j++) {

                    var constant = Expression.Pow(4, i);

                    var t = ts[i - 1];

                    var value = (constant * t[j + 1] - t[j]) / (constant - 1);

                    string.Format("T{3}{4}: ({0}({1}) - {2}) / ({0} - 1) = {5}", constant.ToReal(), t[j + 1].ToReal(), t[j].ToReal(), i, j, value.ToReal()).Print();

                    ts[i][j] = value;
                }
            }

            Console.WriteLine("----------------------- End");

            ts[hList.Count - 1][0].ToReal().Print();

            return ts[hList.Count - 1][0].ToReal();
        }

        public static double RombergTrapezoidPart(this Lookup expression, Expression low, Expression high, int n) {

            var str = "";

            var interval = (high - low) / n;

            var currentX = low + interval;

            var trap = expression.Subsitute(low) + expression.Subsitute(high);

            str += (expression.Subsitute(low).ToReal() + " + " + expression.Subsitute(high).ToReal());

            for(var i = 0; i < n - 1; i++) {

                var part = expression.Subsitute(currentX) * 2;

                str += (" + 2*" + (expression.Subsitute(currentX)).ToReal());

                trap += part;
                currentX += interval;
            }

            var area = (interval / 2) * trap;

            return area.ToReal();
        }

        public static double Trapezoid(this Lookup expression, Expression low, Expression high, int n) {

            var str = "";

            var interval = (high - low) / n;

            var currentX = low + interval;

            var trap = expression.Subsitute(low) + expression.Subsitute(high);

            str += (expression.Subsitute(low).ToReal() + " + " + expression.Subsitute(high).ToReal());

            for(var i = 0; i < n - 1; i++) {

                var part = expression.Subsitute(currentX) * 2;

                str += (" + 2*" + (expression.Subsitute(currentX)).ToReal());

                trap += part;
                currentX += interval;
            }

            var area = (interval / 2) * trap;

            ("Interval: " + interval.ToReal()).Print();
            ((interval / 2).ToReal() + " (" + str + ") = " + area.ToReal()).Print();

            return area.ToReal();
        }

        public static double Simpson1_3(this Lookup lookup, Expression low, Expression high, int n) {

            var str = "";

            var interval = (high - low) / n;

            var currentX = low + interval;

            Expression trap = 0;

            for(var i = 0; i < n / 2; i++) {

                var part = lookup.Subsitute(currentX - interval) +
                           (4 * lookup.Subsitute(currentX)) +
                           lookup.Subsitute(currentX + interval);

                str += "(" + (lookup.Subsitute(currentX - interval).ToReal() + " + " +
                              "4*" + (lookup.Subsitute(currentX)).ToReal() + " + " +
                              lookup.Subsitute(currentX + interval).ToReal()) + ") ";

                trap += part;
                currentX += interval * 2;
            }

            var area = (interval / 3) * trap;

            ("Interval: " + interval.ToReal()).Print();

            str = (interval / 3).ToReal() + " * SUM(" + str + ") = " + area.ToReal();

            str.Print();

            return area.ToReal();
        }

        public static double Simpson3_8(this Lookup lookup, Expression low, Expression high, int n) {

            var str = "";

            var interval = (high - low) / n;

            var currentX = low + (interval * 2);

            Expression trap = 0;

            for(var i = 0; i < n / 3; i++) {

                var part = lookup.Subsitute(currentX - interval - interval) +
                           (3 * lookup.Subsitute(currentX - interval))
                           + (3 * lookup.Subsitute(currentX)) +
                           lookup.Subsitute(currentX + interval);

                str += "(" + (lookup.Subsitute(currentX - interval - interval).ToReal() + " + " +
                              "3*" + (lookup.Subsitute(currentX - interval)).ToReal() + " + "
                              + "3*" + (lookup.Subsitute(currentX)).ToReal() + " + " +
                              lookup.Subsitute(currentX + interval).ToReal()) + ") ";

                trap += part;
                currentX += interval * 3;
            }

            var area = (interval * 3 / 8) * trap;

            ("Interval: " + interval.ToReal()).Print();

            str = (interval / 3).ToReal() + " * SUM(" + str + ") = " + area.ToReal();

            str.Print();

            return area.ToReal();
        }


    }

    public static class QuadratureDictionary {

        public static List<QuadratureValue> Values = new List<QuadratureValue> {
            new QuadratureValue(),
            new QuadratureValue(),
            new QuadratureValue {
                L = new List<double> {
                    1,
                    1
                },
                U = new List<double> {
                    -0.57735,
                    0.57735
                }
            },
            new QuadratureValue {
                L = new List<double> {
                    0.55556,
                    0.88889,
                    0.55556,
                },
                U = new List<double> {
                    -0.77460,
                    0,
                    0.77460
                },
            },
            new QuadratureValue {
                L = new List<double> {
                    0.34785,
                    0.65215,
                    0.65215,
                    0.34785
                },
                U = new List<double> {
                    -0.86114,
                    -0.33998,
                    0.33998,
                    0.86114
                }
            },
            new QuadratureValue {
                L = new List<double> {
                    0.23693,
                    0.47863,
                    0.56889,
                    0.47863,
                    0.23693,
                },
                U = new List<double> {
                    -0.90618,
                    -0.53847,
                    0,
                    0.53847,
                    0.90618
                }
            },
            new QuadratureValue {
                L = new List<double> {
                    0.17132,
                    0.36076,
                    0.46791,
                    0.46791,
                    0.36076,
                    0.17132,
                },
                U = new List<double> {
                    -0.93247,
                    0.66121,
                    -0.23862,
                    0.23862,
                    -0.66121,
                    0.93247
                }
            }
        };

        public static List<double> L(int i) {

            return Values[i].L;
        }

        public static List<double> U(int i) {

            return Values[i].U;
        }
    }

    public class QuadratureValue {

        public List<double> L {
            get;
            set;
        }

        public List<double> U {
            get;
            set;
        }
    }
}
