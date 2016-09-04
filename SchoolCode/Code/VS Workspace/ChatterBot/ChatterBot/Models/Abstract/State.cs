namespace ChatterBot.Models.Abstract {

    public abstract class State<T> {

        public virtual string Name {
            get {
                return GetType().Name;
            }
        }

        public virtual bool HasAcceptance {
            get {
                return !Equals(Response, default(T));
            }
        }

        public virtual T Response {
            get {
                return default(T);
            }
        }

        public abstract State<T> Propose(dynamic args);

    }
}
