using System;
using System.Linq;

using MathNet.Symbolics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests {
    [TestClass]
    public class UnitTest {

        [TestMethod]
        public void TestMethod1() {

            var x = Expression.Symbol("x");

            var expression = x * 2 + x;

            var result = Solve(expression, 3);

            var a = expression.IsProduct;
        }

        [TestMethod]
        public void ToRealTest() {

            var x = Infix.ParseOrUndefined("1/3");

            var d = x.ToReal().Round(2);

            Assert.AreEqual(0.33, d);
        }

        [TestMethod]
        public void ToFractionTest() {

            var x = 1 / 3d;

            var f = x.ToFraction(0.001);

            Assert.AreEqual(3, f.Denominator);
            Assert.AreEqual(1, f.Numerator);
        }

        public static Expression Solve(Expression equation, Expression result) {

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
    }
}
