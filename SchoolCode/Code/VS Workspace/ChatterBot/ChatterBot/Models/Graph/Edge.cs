using System.Collections.Generic;

namespace ChatterBot.Models.Graph {
    public class Edge {

        public Node Left {
            get;
            set;
        }

        public Node Right {
            get;
            set;
        }

        public double Weight {
            get;
            set;
        }

        public override string ToString() {
            return string.Format("Weight: {0}: {1} <-> {2}", Weight, Left, Right);
        }

        public bool Contains(Node node) {

            return Left.Equals(node) || Right.Equals(node);
        }

        protected bool Equals(Edge other) {

            return (Equals(Right, other.Right) && Equals(Left, other.Left)) || Equals(Right, other.Left) && Equals(Left, other.Right);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }
            if(ReferenceEquals(this, obj)) {
                return true;
            }
            if(obj.GetType() != GetType()) {
                return false;
            }
            return Equals((Edge) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Right != null ? Right.GetHashCode() : 0) + (Left != null ? Left.GetHashCode() : 0)) * 397;
            }
        }

        private sealed class WeightEqualityComparer : IEqualityComparer<Edge> {

            public bool Equals(Edge x, Edge y) {
                if(ReferenceEquals(x, y)) {
                    return true;
                }
                if(ReferenceEquals(x, null)) {
                    return false;
                }
                if(ReferenceEquals(y, null)) {
                    return false;
                }
                if(x.GetType() != y.GetType()) {
                    return false;
                }
                return x.Weight.Equals(y.Weight);
            }

            public int GetHashCode(Edge obj) {
                return obj.Weight.GetHashCode();
            }

        }

        private static readonly IEqualityComparer<Edge> _weightComparerInstance = new WeightEqualityComparer();

        public static IEqualityComparer<Edge> WeightComparer {
            get {
                return _weightComparerInstance;
            }
        }

    }
}
