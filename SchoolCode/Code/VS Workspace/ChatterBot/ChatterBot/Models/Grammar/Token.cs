using System.Collections.Generic;

using ChatterBot.Actors.Grammar;

namespace ChatterBot.Models.Grammar {

    public abstract class Token {

        public bool IsWord {
            get {
                return Value != default(string);
            }
        }

        public virtual string Response {
            get {
                return Value;
            }
        }

        public string Value {
            get;
            protected set;
        }

        public List<Token> Children {
            get;
            protected set;
        }

        public override string ToString() {

            return string.Format(Value);
        }

        protected bool Equals(Token other) {

            return this.OrderedSearch().Equals(other.OrderedSearch());
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
            return Equals((Token) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Children != null ? Children.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

    }

}