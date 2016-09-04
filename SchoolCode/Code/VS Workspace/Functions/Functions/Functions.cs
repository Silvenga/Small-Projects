
using System;

namespace Functions {

    public static class Functions {

        private static double _error = 0.005;

        public static double Error {
            get {
                return _error;
            }
            set {
                _error = value;
            }
        }

        public static double Newton(this Func equation, Func derivative, double start = 1d) {

            var currentX = start;
            var currentError = double.MaxValue;

            var lowest = double.PositiveInfinity;

            while(currentError > Error) {

                var lastX = currentX;
                currentX = NewtonMethod(currentX, equation, derivative);

                currentError = currentX.Diff(lastX);

                if(currentError < lowest) {
                    //Console.WriteLine("E: {0}", currentError);
                    //Console.WriteLine(currentX);
                    lowest = currentError;
                }

                if(lastX.Equals(currentX)) {
                    Console.WriteLine("Cannot converge. Repeating x of " + currentX + " found.");
                    return double.NaN;
                }

                Console.WriteLine(currentX);
            }

            Console.WriteLine("Result: " + currentX);

            return currentX;
        }

        private static double NewtonMethod(double x, Func equation, Func derivative) {

            var f = equation.Invoke(x);
            var d = derivative.Invoke(x);

            return x - (f / d);
        }

        public static double Muller(this Func equation, double a, double b) {

            var x = new DyamicList<double>(double.NaN);

            x[0] = a;
            x[1] = (a + b) / 2;
            x[2] = b;

            var f = new DyamicList<double>(double.NaN);

            f[0] = equation.Invoke(x[0]);
            f[1] = equation.Invoke(x[1]);
            f[2] = equation.Invoke(x[2]);

            var d = new DyamicList<double>(double.NaN);

            d[0] = x[1] - x[0];
            d[1] = x[2] - x[1];

            var g = new DyamicList<double>(double.NaN);

            g[0] = (f[1] - f[0]) / d[0];
            g[1] = (f[2] - f[1]) / d[1];

            var i = 0;

            var h = new DyamicList<double>(double.NaN);
            var c = new DyamicList<double>(double.NaN);

            while(true) {

                h[i] = (g[i + 1] - g[i]) / (d[i + 1] + d[i]);

                c[i] = g[i + 1] + (d[i + 1] * h[i]);

                var sqr = Math.Pow(c[i], 2) - (4 * f[i + 2] * h[i]);

                if(sqr < 0) {
                    Console.WriteLine("Cannot converge with interval.");
                    return Double.NaN;
                }

                d[i + 2] = (-2 * f[i + 2]) / (c[i] + (Math.Sign(c[i]) * Math.Sqrt(sqr)));

                x[i + 3] = x[i + 2] + d[i + 2];

                f[i + 3] = equation.Invoke(x[i + 3]);

                if(Equals(f[i + 3], 0d))
                    f[i + 3] = 0.0000000000000001;

                g[i + 2] = (f[i + 3] - f[i + 2]) / (d[i + 2]);

                Console.WriteLine(x[i + 1]);

                if(x[i + 1].Diff(x[i]) < Error)
                    break;

                i++;
            }

            Console.WriteLine(x.ToString("x", true));
            Console.WriteLine(d.ToString("d", true));
            Console.WriteLine(c.ToString("c", true));
            Console.WriteLine(g.ToString("g", true));
            Console.WriteLine(h.ToString("h", true));
            Console.WriteLine("Result: " + x[i + 1]);

            return x[i + 1];
        }



        public static double RegulaFalsi(this Func equation, double a, double b) {

            var currentX = double.MinValue;
            var currentError = double.MaxValue;

            while(currentError > Error) {

                var lastX = currentX;
                currentX = RegulaFalsiFunction(a, b, equation);
                a = currentX;

                var lastError = currentError;
                currentError = currentX.Diff(lastX);

                if(lastError < currentError) {

                    var tmp = b;
                    b = a;
                    a = tmp;
                }


                Console.WriteLine(currentX);
            }

            Console.WriteLine("Result: " + currentX);

            return currentX;
        }

        private static double RegulaFalsiFunction(double a, double b, Func equation) {

            var fA = equation.Invoke(a);
            var fB = equation.Invoke(b);

            var top = (a * fB) - (b * fA);
            var bot = (fB - fA);

            return top / bot;
        }

        public static double BolzanoBisection(this Func equation, double a, double b) {

            var fA = equation.Invoke(a);
            var fB = equation.Invoke(b);

            // Check for 0
            if(Equals(fA, 0d) || Equals(fB, 0d))
                throw new Exception("fA, fB cannot == 0");

            // Should both be different
            if(fA.IsNegative() == fB.IsNegative())
                throw new Exception("fA, fB cannot both be negative or possitive.");

            var high = (fA.IsNegative()) ? b : a;
            var low = (fA.IsNegative()) ? a : b;
            var lastMiddle = double.PositiveInfinity;
            var currentMiddle = double.NegativeInfinity;

            while(Math.Abs(lastMiddle - currentMiddle) > Error) {

                lastMiddle = currentMiddle;
                currentMiddle = CalculateMiddle(high, low);

                var fC = equation.Invoke(currentMiddle);

                if(fC.IsNegative())
                    low = currentMiddle;
                else
                    high = currentMiddle;

                Console.WriteLine(currentMiddle);
            }

            Console.WriteLine("Result: " + currentMiddle);

            return currentMiddle;
        }

        public static double CalculateMiddle(double high, double low) {

            return (high + low) / 2;
        }

        public static double Diff(this double a, double b) {

            var value = Math.Abs(a - b);

            if(double.IsNaN(value))
                return double.PositiveInfinity;

            return value;
        }
    }
}
