#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Symbolics;

using Evaluate = MathNet.Symbolics.Evaluate;

#endregion

namespace Functions.Models {

    internal class DifferenceTable {

        public enum KeyType {

            X,
            Y,
            DeltaY

        }

        private DifferenceTable() {

        }

        public DifferenceTable(List<Expression> x, List<Expression> y) {

            if(x.Count != y.Count)
                throw new Exception("X and Y must be the same size.");

            Rows = x.Count();

            Table = new DyamicList<DyamicList<Expression>>();

            for(var n = 0; n < Rows + 3; n++) {

                Table[n] = new DyamicList<Expression>();
            }

            for(var n = 0; n < Rows; n++) {

                Table[0][n] = x[n];
                Table[1][n] = y[n];
            }
        }

        public DyamicList<DyamicList<Expression>> Table {
            get;
            set;
        }

        public int Rows {
            get;
            set;
        }

        public Expression FindAtX(Expression value) {

            for(var i = 0; i < Rows; i++) {

                if(Equals(Table[0][i], value)) {

                    return Table[1][i];
                }
            }

            return Expression.Undefined;
        }

        public static void CreateAbstract() {

        }

        public DifferenceTable ForwardDifferences() {

            var oddset = 0;

            for(var col = 2; col <= Rows; col++) {

                for(var row = 0; row < Rows - oddset - 1; row++) {

                    var a = Table[col - 1][row + 1];
                    var b = Table[col - 1][row];

                    Table[col][row] = a - b;
                }
                oddset++;
            }

            this.Print();

            return new DifferenceTable {
                Table = Table,
                Rows = Rows
            };
        }

        public DifferenceTable BackwardDifferences() {

            var oddset = 0;

            for(var col = 2; col <= Rows; col++) {

                for(var row = Rows - 1; row >= oddset + 1; row--) {

                    Table[col][row] = Table[col - 1][row] - Table[col - 1][row - 1];
                }
                oddset++;
            }

            this.Print();

            return new DifferenceTable {
                Table = Table,
                Rows = Rows
            };
        }

        public void Replace(Dictionary<string, FloatingPoint> symbols) {

            foreach(var b in Table.Backend) {

                foreach(var i in b.Value.Backend) {

                    Evaluate.Evaluate(symbols, i.Value);
                }
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="x">Delta</param>
        /// <param name="y">Base 0</param>
        /// <param name="start">The start of x</param>
        /// <returns></returns>
        public Expression Solve(int x, int start) {

            Console.WriteLine();

            var value = 0.P();

            var str = "SUM ";

            for(var i = 0; i <= Rows; i++) {

                var part = Part(i, x, start);

                str += Infix.Print(part) + " ";

                value += part;
            }

            str += "= " + Infix.Print(value);

            str.Print();
            value.Print();

            return value;
        }

        public Expression Part(int index, int high, int delta) {

            var c = FiniteFunctions.Choose(high, index);

            var y = Expression.Pow(-1, index);

            var x = FindAtX(high - index + delta);

            return c * y * x;
        }

        public Expression NewtonForward(Expression x, Expression h) {

            var x0 = Table[0][0];

            var u = (x - x0) / h;

            var value = 0.P();

            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            for(var i = 0; i < Rows; i++) {

                var part = NewtonForwardPart(i, u);

                str += " " + part.ToReal();

                value += part;
            }

            (str + " = " + value.ToReal()).Print();

            return Rational.Simplify("x", value);
        }

        private Expression NewtonForwardPart(int delta, Expression u) {

            var c = FiniteFunctions.Choose(u, delta);

            var y = Table[delta + (1)][0];

            return c * y;
        }

        public Expression NewtonForwardDiff(Expression x, Expression h, int times) {

            var x0 = Table[0][0];

            var u = (x - x0) / h;

            var uPrime = 1 / Expression.Pow(h, times);

            var value = 0.P();

            ("H: " + h.ToReal()).Print();
            //("HPrime: " + uPrime.ToReal()).Print();
            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            for(var i = 0; i < Rows; i++) {

                var part = NewtonForwardDiffPart(i, u, times);

                str += " " + part.ToReal();

                value += part;
            }

            value *= uPrime;

            (str + " = " + value.ToReal()).Print();

            return Rational.Simplify("x", value);
        }

        private Expression NewtonForwardDiffPart(int delta, Expression u, int times) {

            var c = FiniteFunctions.ChooseWithDiff(u, delta, times, -1);

            var y = Table[delta + (1)][0];

            // (y.ToReal() + " * " + c.ToReal() + " = " + (y * c).ToReal()).Print();

            return y * c;
        }


        public Expression NewtonBackwardDiff(Expression x, Expression h, int times) {

            var xN = Table[0][Rows - 1];

            var u = (x - xN) / h;
            var uPrime = 1 / Expression.Pow(h, times);

            var value = 0.P();

            ("H: " + h.ToReal()).Print();
            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            for(var i = 0; i < Rows; i++) {

                var part = NewtonBackwardDiffPart(i, u, times);

                str += " " + part.ToReal();

                value += part;
            }

            value *= uPrime;


            (str + " = " + (Rational.Simplify("x", value).ToReal())).Print();

            return Rational.Simplify("x", value);
        }

        private Expression NewtonBackwardDiffPart(int delta, Expression u, int times) {

            var c = FiniteFunctions.ChooseWithDiff(u, delta, times, 1);

            var y = Table[delta + (1)][Rows - 1];

            return y * c;
        }


        public Expression NewtonBackward(Expression x, Expression h) {

            var xN = Table[0][Rows - 1];

            var u = (x - xN) / h;

            var value = 0.P();

            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            for(var i = 0; i < Rows; i++) {

                var part = NewtonBackwardPart(i, u);

                str += " " + part.ToReal();

                value += part;
            }

            (str + " = " + Rational.Simplify("x", value).ToReal()).Print();

            return Rational.Simplify("x", value);
        }

        private Expression NewtonBackwardPart(int delta, Expression u) {

            var c = FiniteFunctions.Choose(u, delta, 1);

            var y = Table[delta + (1)][Rows - 1];

            return c * y;
        }

        private int IndexOfX(Expression expression) {

            foreach(var a in Table[0].Backend) {

                var value = a.Value;

                if(expression.Equals(value)) {

                    return a.Key;
                }
            }

            return -1;
        }

        public Expression GalsForward(Expression x, Expression h, Expression low) {

            var x0Index = IndexOfX(low);

            var x0 = Table[0][x0Index];

            var u = (x - x0) / h;

            var value = 0.P();

            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            var index = 0;

            for(var i = 0; i < Rows; i++) {

                var old = index;

                if(i != 0 && i % 2 == 0)
                    index++;

                var part = GalsForwardPart(i, x0Index - index, u, old);

                if(part == null) {
                    Console.WriteLine("null");
                    break;
                }

                str += " " + part.ToReal();

                value += part;
            }

            (str + " = " + Rational.Simplify("x", value).ToReal()).Print();

            return Rational.Simplify("x", value);
        }

        private Expression GalsForwardPart(int delta, int index, Expression u, int i) {

            var c = FiniteFunctions.Choose(u + i, delta);

            var y = Table[delta + (1)][index];

            Console.WriteLine((y ?? -1).ToReal());

            return (y == null) ? null : c * y;
        }

        public Expression GalsBackward(Expression x, Expression h, Expression high) {

            var x0Index = IndexOfX(high);

            Console.WriteLine(x0Index);

            var x0 = Table[0][x0Index];

            var u = (x - x0) / h;

            var value = 0.P();

            ("U: " + u.ToReal()).Print();

            var str = x.ToReal() + " = SUM";

            var index = 0;

            for(var i = 0; i < Rows; i++) {

                var old = index;

                if(i != 0 && i % 2 == 0)
                    index++;

                var part = GalsBackwardPart(i, x0Index + index, u, old);

                if(part == null)
                    break;

                str += " " + part.ToReal();

                value += part;
            }

            (str + " = " + Rational.Simplify("x", value).ToReal()).Print();

            return Rational.Simplify("x", value);
        }

        private Expression GalsBackwardPart(int delta, int index, Expression u, int i) {

            var c = FiniteFunctions.Choose(u - i, delta, 1);

            var y = Table[delta + (1)][index];

            return (y == null) ? null : c * y;
        }

        public string Format(Expression expression) {

            var x = Expression.Symbol("x");

            var real = Exponential.Simplify(Rational.Simplify(x, expression)).ToReal();

            return (double.IsNaN(real) ? "" : "" + real);
        }

        public Expression FindAtX(Expression value, int delta) {

            for(var i = 0; i < Rows; i++) {

                if(Equals(Table[0][i], value)) {

                    return Table[delta + 1][i];
                }
            }

            return Expression.Undefined;
        }

        public Expression Equation() {

            var value = 0.P();

            for(var i = 0; i < Rows - 1; i++) {

                var part = Part(i);

                value += part;
            }

            Rational.Simplify("x", value).Print();

            return Rational.Simplify("x", value);
        }

        public Expression Part(int delta) {

            var c = FiniteFunctions.Choose("x", delta);

            var y = Table[delta + (1)][0];

            return c * y;
        }

        public override string ToString() {

            var strings = new List<string>();

            var head = "| ";

            var delta = -1;

            for(var col = 0; col < Rows + 1; col++) {

                var index = 0;

                var b = Table[col];

                var padding = 0;
                if(b.Backend.Values.Any()) {

                    padding =
                        b.Backend.Select(x => Format(x.Value)).OrderByDescending(x => x.Length).First().Count();

                    if(padding == 1)
                        padding = 2;

                    var val = "y" + delta;

                    if(delta == -1)
                        val = "x";

                    head += val.PadLeft(padding) + " | ";

                    delta++;
                }

                for(var row = 0; row < Rows; row++) {

                    if(strings.Count <= index)
                        strings.Add("| ");

                    var val = b[row] ?? "";

                    strings[index] += "" + Format(val).PadLeft(padding) + " | ";
                    index++;
                }
            }

            return head + "\n" + string.Join("\n", strings) + "\n";
        }

    }

}