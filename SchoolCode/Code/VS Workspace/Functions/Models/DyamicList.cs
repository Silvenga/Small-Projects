using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using MathNet.Symbolics;

namespace Functions {

    public class DyamicList<T> {

        public Dictionary<int, T> Backend {
            get;
            set;
        }

        public T Default {
            get;
            set;
        }

        public DyamicList(T defaultReturn = default(T)) {

            Backend = new Dictionary<int, T>();
            Default = defaultReturn;
        }

        public static DyamicList<T> Create(T[] array, T defaultReturn) {

            var list = new DyamicList<T>(defaultReturn);

            for(var i = 0; i < array.Length; i++) {

                list[i] = array[i];
            }

            return list;
        }

        public T this[int i] {
            get {
                if(Backend.ContainsKey(i))
                    return Backend[i];

                return Default;
            }
            set {

                if(!Backend.ContainsKey(i))
                    Backend.Add(i, value);
                else
                    Backend[i] = value;
            }
        }

        public override string ToString() {

            return ToString("x");
        }

        public string ToString(string name, bool useNewLines = false, bool round = false) {

            var separator = ((useNewLines) ? "\n" : ", ");

            var values = Backend.ToList().OrderBy(x => x.Key).Select(x => name + "[" + x.Key + "]=" + String(x.Value));
            return string.Join(separator, values);
        }

        protected bool Equals(DyamicList<T> other) {

            return Equals(Backend, other.Backend);
        }

        public override bool Equals(object obj) {

            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((DyamicList<T>) obj);
        }

        public override int GetHashCode() {

            return (Backend != null ? Backend.GetHashCode() : 0);
        }

        public string String(object o) {

            if(o as Expression == null)
                return o.ToString();
            else {
                return Infix.Print(o as Expression);
            }
        }

    }
}
