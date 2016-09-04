using System;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Functions {
    public static class MatrixFunctions {

        private static double _error = 0.0005;

        public static double Error {
            get {
                return _error;
            }
            set {
                _error = value;
            }
        }

        public static Matrix Doolittle(this Matrix a, Matrix b) {

            Console.WriteLine("a:");
            Console.WriteLine(a);
            Console.WriteLine("b:");
            Console.WriteLine(b);

            var rows = a.RowCount;
            var cols = a.ColumnCount;

            Matrix<double> l = DenseMatrix.Create(rows, cols, double.NaN);
            l.SetUpperTriangle(0);
            l.SetDiagonal(DenseVector.Create(rows, 1));

            Matrix<double> u = DenseMatrix.Create(rows, cols, double.NaN);
            u.SetLowerTriangle(0);

            // first row is known
            u.SetRow(0, a.Row(0));

            for(var row = 1; row < rows; row++) {

                for(var col = 0; col < cols; col++) {

                    if(double.IsNaN(l[row, col])) {

                        l[row, col] = DoolittleMultiplcation(row, col, l, u, a);

                    } else if(double.IsNaN(u[row, col])) {

                        u[row, col] = DoolittleMultiplcation(row, col, l, u, a);
                    }
                }
            }

            Console.WriteLine("l:");
            Console.WriteLine(l);
            Console.WriteLine("u:");
            Console.WriteLine(u);


            Matrix<double> y = DenseMatrix.Create(rows, 1, double.NaN);
            var yValue = new DyamicList<double>(double.NaN);

            for(var row = 0; row < rows; row++) {


                var value = DoolittleMultiplcation(row, 0, l, y, b);
                yValue[row] = value;
                y[row, 0] = value;
            }

            Console.WriteLine(yValue.ToString("y"));

            Matrix<double> x = DenseMatrix.Create(rows, 1, double.NaN);
            var xValue = new DyamicList<double>(double.NaN);

            for(var row = rows - 1; row >= 0; row--) {

                var valueType = DoolittleMultiplcation(row, 0, u, x, y);

                x[row, 0] = valueType;
                xValue[row] = valueType;
            }

            Console.WriteLine(xValue.ToString("x"));

            return null;
        }

        public static double DoolittleMultiplcation(int uRow, int uCol, Matrix<double> l, Matrix<double> u, Matrix<double> a) {

            var value = a[uRow, uCol];

            var uCo = 1d;

            var size = a.RowCount;

            var rVector = l.Row(uRow);
            var cVector = u.Column(uCol);

            for(var i = 0; i < size; i++) {

                var rValue = rVector[i];
                var cValue = cVector[i];

                if(i == uRow && !double.IsNaN(rValue) && double.IsNaN(cValue)) {

                    uCo = rValue;

                } else if(i == uCol && !double.IsNaN(cValue) && double.IsNaN(rValue)) {

                    uCo = cValue;

                } else {

                    // NaN * 0 = NaN
                    value -= (rValue.Equals(0) || cValue.Equals(0)) ? 0 : rValue * cValue;
                }
            }

            value /= uCo;

            return value;
        }

        public static void SetUpperTriangle(this Matrix<double> matrix, double value) {

            var size = matrix.ColumnCount;

            var leftPadding = 1;

            for(var row = 0; row < size; row++) {

                for(var col = leftPadding; col < size; col++) {

                    matrix.At(row, col, value);
                }

                leftPadding++;
            }
        }

        public static void SetLowerTriangle(this Matrix<double> matrix, double value) {

            var size = matrix.ColumnCount - 1;

            var rightPadding = 1;

            for(var row = size; row > 0; row--) {

                for(var col = size - rightPadding; col >= 0; col--) {

                    matrix.At(row, col, value);
                }

                rightPadding++;
            }
        }

        public static DyamicList<double> GaussElimination(this Matrix<double> m) {

            var equations = DenseMatrix.OfMatrix(m);

            var x = new DyamicList<double>(double.NaN);

            Console.WriteLine(equations);

            var numZeros = 0;

            for(var row = 0; row < equations.RowCount; row++) {

                for(var zeros = 0; zeros < numZeros; zeros++) {

                    var topVector = equations.Row(row - (numZeros - zeros));
                    var currentVector = equations.Row(row);

                    var top = topVector.At(zeros);
                    var bot = currentVector.At(zeros);

                    var normal = bot / top;

                    currentVector -= topVector * normal;

                    equations.SetRow(row, currentVector);
                }

                numZeros++;
            }

            equations.Round();
            Console.WriteLine(equations);

            for(var row = equations.RowCount - 1; row >= 0; row--) {

                var rowVector = equations.Row(row);
                var value = rowVector.At(rowVector.Count - 1);

                for(var xNum = rowVector.Count - 2; xNum >= row; xNum--) {

                    if(x[xNum].Equals(double.NaN)) {

                        value /= rowVector.At(xNum);
                        break;
                    }

                    value -= rowVector.At(xNum) * x[xNum];
                }

                x[row] = value;
            }

            Console.WriteLine("Result: ");
            Console.WriteLine(x.ToString("x", true));

            return x;
        }

        public static DyamicList<double> GaussJordanElimination(this Matrix<double> m) {

            var equations = DenseMatrix.OfMatrix(m);

            Console.WriteLine(equations);

            var numZeros = 0;

            for(var row = 0; row < equations.RowCount; row++) {

                for(var zeros = 0; zeros < numZeros; zeros++) {

                    var topVector = equations.Row(row - (numZeros - zeros));
                    var currentVector = equations.Row(row);

                    var top = topVector.At(zeros);
                    var bot = currentVector.At(zeros);

                    var normal = bot / top;

                    currentVector -= topVector * normal;

                    equations.SetRow(row, currentVector);
                }

                numZeros++;
            }

            equations.Round();
            Console.WriteLine(equations);

            var lastRow = equations.RowCount - 1;
            var lastCol = equations.ColumnCount - 1;

            for(var row = lastRow; row >= 0; row--) {

                var rowVector = equations.Row(row);

                for(var xNum = lastCol - 1; xNum >= row; xNum--) {

                    if(xNum == row) {

                        rowVector /= rowVector.At(xNum);
                        continue;
                    }

                    var helperVector = equations.Row(xNum);
                    var normal = rowVector.At(xNum);

                    rowVector -= helperVector * normal;
                }

                equations.SetRow(row, rowVector);
            }

            equations.Round();

            var values = equations.Column(lastCol).ToArray();

            var x = DyamicList<double>.Create(values, double.NaN);

            Console.WriteLine("Result: ");
            Console.WriteLine(x.ToString("x", true));

            return x;
        }

        public static Matrix<double> JoardanInversion(this Matrix<double> matrix) {

            var size = matrix.ColumnCount;

            var vectors = matrix.EnumerateColumns();
            vectors = vectors.Concat(DenseMatrix.CreateIdentity(size).EnumerateColumns());

            Matrix<double> workspace = DenseMatrix.OfColumnVectors(vectors);

            Console.WriteLine(workspace);

            var numZeros = 0;

            for(var row = 0; row < size; row++) {

                for(var zeros = 0; zeros < numZeros; zeros++) {

                    var topVector = workspace.Row(row - (numZeros - zeros));
                    var currentVector = workspace.Row(row);

                    var top = topVector.At(zeros);
                    var bot = currentVector.At(zeros);

                    var normal = bot / top;

                    currentVector -= topVector * normal;

                    workspace.SetRow(row, currentVector);
                }

                numZeros++;
            }

            workspace.Round();
            Console.WriteLine(workspace);

            for(var row = size - 1; row >= 0; row--) {

                var rowVector = workspace.Row(row);

                for(var xNum = size - 1; xNum >= row; xNum--) {

                    if(xNum == row) {

                        rowVector /= rowVector.At(xNum);
                        continue;
                    }

                    var helperVector = workspace.Row(xNum);
                    var normal = rowVector.At(xNum);

                    rowVector -= helperVector * normal;
                }

                workspace.SetRow(row, rowVector);
            }

            workspace.Round();
            Console.WriteLine(workspace);

            var result = workspace.SubMatrix(0, size, size, size);

            result.Round();

            Console.WriteLine("Result:");
            Console.WriteLine(result);

            return result;
        }

        public static double PowerMethod(this Matrix<double> matrix) {

            var lambda = new DyamicList<double>(double.NaN);

            Console.WriteLine(matrix);

            var eigenVector = (Vector<double>) DenseVector.Create(matrix.ColumnCount, i => (i) + 3);

            var index = -1;

            do {
                index++;

                eigenVector = matrix * eigenVector;

                lambda[index] = eigenVector.HighestValue();

                eigenVector /= lambda[index];

            } while(lambda[index].Diff(lambda[index - 1]) > Error);

            Console.WriteLine(lambda.ToString("lambda", true));
            Console.WriteLine(eigenVector);

            string.Format("Result: {0}", Math.Round(lambda[index], 5)).Print();

            return Math.Round(lambda[index], 5);
        }

        public static double HighestValue(this Vector<double> vector) {

            var largest = 0d;

            for(var i = 0; i < vector.Count; i++) {

                if(vector[i].Abs() == largest.Abs()) {

                    largest = (vector[i] > largest) ? vector[i] : largest;

                } else {

                    largest = (vector[i].Abs() > largest.Abs()) ? vector[i] : largest;

                }

            }

            return largest;
        }

        public static double InversePowerMethod(this Matrix<double> matrix) {

            var value = matrix.Inverse().PowerMethod();
            value = 1 / value;

            string.Format("Result: {0}", Math.Round(value, 5)).Print();

            return Math.Round(value, 5);
        }

    }
}