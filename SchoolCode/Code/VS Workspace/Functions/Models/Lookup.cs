using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Symbolics;

namespace Functions.Models {
    public class Lookup {

        public Dictionary<Expression, Expression> Table {
            get;
            set;
        }

        public Expression Function {
            get;
            set;
        }

        public string Symbol {
            get;
            set;
        }

        public Lookup(Dictionary<Expression, Expression> table) {

            Table = table;
        }

        public Lookup(List<Expression> x, List<Expression> y) {

            Table = Helpers.CreateDictionary(x, y);
        }

        public Lookup(List<double> x, List<double> y) {

            Table = Helpers.CreateDictionary(x.P(), y.P());
            Function = null;
        }

        public Lookup(Expression expression, string symbol) {

            Function = expression;
            Symbol = symbol;
            Table = new Dictionary<Expression, Expression>();
        }

        public Expression Subsitute(Expression x) {

            if(!Table.ContainsKey(x)) {

                if(string.IsNullOrWhiteSpace(Symbol) || Function == null)
                    throw new ArgumentException("Value cannot be found in table and symbol has no value.");

                return Function.Subsitute(Symbol, x);
            }

            return Table[x];
        }
    }
}
