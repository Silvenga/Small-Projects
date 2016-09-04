namespace SlightLibrary.Entities {

    public class Position {

        private Pos _pos;

        public int X {
            get {
                return _pos.X;
            }
            set {
                _pos.X = value;
            }
        }
        public int Y {
            get {
                return _pos.Y;
            }
            set {
                _pos.Y = value;
            }
        }

        public Position(int x = 0, int y = 0) {
            _pos.X = x;
            _pos.Y = y;
        }

        private bool Equals(Position other) {

            return Y == other.Y && X == other.X;
        }

        public override bool Equals(object obj) {

            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Position) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Y * 397) ^ X;
            }
        }

        public override string ToString() {

            return X + "," + Y;
        }

    }

    public struct Pos {

        public int X {
            get;
            set;
        }
        public int Y {
            get;
            set;
        }

        public Pos(int y = 0, int x = 0)
            : this() {
            Y = y;
            X = x;
        }

        public bool Equals(Pos other) {
            return Y == other.Y && X == other.X;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Pos && Equals((Pos) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Y * 397) ^ X;
            }
        }

        public override string ToString() {

            return X + "," + Y;
        }

    }
}
