#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Functions.Models;

using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Symbolics;
#endregion

namespace Functions {

    internal class Program {

        public delegate Expression Cal(Expression x);

        private static void Main() {

            var x = Expression.Symbol("x");

            // 1
            {
                // var func = new Func("x^2 - 5x - 24");
                // var high = 6.6;
                // var low = 9.1;
                // string.Format("Using: {0} and {1}", low, high).Print();
                // func.BolzanoBisection(low, high);
                // func.RegulaFalsi(low, high);
                // func.Muller(high, low);

                // high = -5;
                // low = -2;
                // string.Format("Using: {0} and {1}", low, high).Print();
                //func.BolzanoBisection(low, high);
                // func.RegulaFalsi(low, high);
                // func.Muller(-10, -2);
            }
            // 2
            {
                //var func = new Func("cos(x) - x^2 - 2");
                //var funcPrime = new Func("-sin(x) - 2x");
                //// func.BolzanoBisection(0, 2);
                ////func.Newton(funcPrime, -1);
                //func.Muller(-1, 0);
            }
            // 3
            {
                //var m = DenseMatrix.OfArray(
                //    new double[,] {
                //        {5,-1,2,24},
                //        {-1,5,-2,8},
                //        {2,1,3,22}
                //    });

                //m.GaussElimination();
                //m.GaussJordanElimination();
            }
            // 4
            {
                //var fuc = Expression.Cos(x) * Expression.Pow(x, 3);

                //new Lookup(fuc, "x").Trapezoid(0, 1, 4);
            }
            // 5
            {
                //var fuc = Expression.Pow(x, 4);

                //var h = (int) Helpers.FindN(1, 2, 0.05.P()).ToReal();
                //new Lookup(fuc, "x").Simpson1_3(1, 2, h);


                //h = (int) Helpers.FindN(1, 2, 0.25.P()).ToReal();
                //new Lookup(fuc, "x").Simpson1_3(1, 2, h);
            }
            // 6
            {
                //var m = DenseMatrix.OfArray(
                //    new double[,] {
                //        {49,-34,-14},
                //        {-34,46,20},
                //        {-14,20,22}
                //    });

                //"High".Print();
                //m.PowerMethod();
                //"Low".Print();
                //m.InversePowerMethod();
                //m.JoardanInversion();
            }
            // 7
            {
                //var m = DenseMatrix.OfArray(
                //    new double[,] {
                //        {16,12,8,4},
                //        {12,13,8,5},
                //        {8,8,6,4},
                //        {4,5,4,4}
                //    });

                //var b = DenseMatrix.OfArray(
                //new double[,] {
                //        {4},
                //        {-3},
                //        {-2},
                //        {-5}
                //    });

                //m.Doolittle(b);
            }
            // 8
            {
                //IntegrationFunctions.Sub sub = delegate(Expression u) {
                //    return Expression.Pow(Math.E.P(), (-1) * u);
                //};

                //var n = 3;
                //IntegrationFunctions.Gauss(-1, 1, QuadratureDictionary.L(n), QuadratureDictionary.U(n), sub, false);

                //n = 4;
                //IntegrationFunctions.Gauss(-1, 1, QuadratureDictionary.L(n), QuadratureDictionary.U(n), sub, false);

                //n = 5;
                //IntegrationFunctions.Gauss(-1, 1, QuadratureDictionary.L(n), QuadratureDictionary.U(n), sub, false);

            }
            // 9
            {

                //IntegrationFunctions.Sub sub = delegate(Expression u) {
                //    return Expression.Pow(u, 5) - (3 * Expression.Pow(u, 2)) + 4;
                //};

                //var a = new List<double> {
                //    -2,
                //    -1,
                //    0,
                //    1,
                //    2
                //}.P();

                //var b = a.Select(u => sub(x).Subsitute("x", u)).ToList();

                //Console.WriteLine("Tables");
                //new DifferenceTable(a, b).ForwardDifferences();
                //new DifferenceTable(a, b).BackwardDifferences();

                //Console.WriteLine("At 0.75");
                //var n = 0.75.P();
                //new DifferenceTable(a, b).ForwardDifferences().NewtonForward(n, 1);
                //new DifferenceTable(a, b).BackwardDifferences().NewtonBackward(n, 1);

                //Console.WriteLine("Prime at -0.5");
                //n = (-0.5).P();
                //new DifferenceTable(a, b).ForwardDifferences().NewtonForwardDiff(n, 1, 1);
                //new DifferenceTable(a, b).BackwardDifferences().NewtonBackwardDiff(n, 1, 1);

                //Console.WriteLine("Double Prime at -0.5");
                //new DifferenceTable(a, b).ForwardDifferences().NewtonForwardDiff(n, 1, 2);
                //new DifferenceTable(a, b).BackwardDifferences().NewtonBackwardDiff(n, 1, 2);
            }
            // 10
            {
                //var func = Expression.Pow(Math.E.P(), (-1) * x);

                //new Lookup(func, "x").RombergTrapezoid(
                //    0,
                //    1,
                //    new List<double> {
                //        (double) 1 / 2,
                //        (double) 1 / 4,
                //        (double) 1 / 8
                //    }.P(), true);

                //new Lookup(func, "x").Simpson1_3(0, 1, 2);
                //new Lookup(func, "x").Trapezoid(0, 1, 10);
            }
            // 11
            {

                //var func = Expression.Sin(x) * Expression.Pow(4, x);
                //new Lookup(func, "x").Simpson3_8(5, 7, 15);
            }

        }

    }

}