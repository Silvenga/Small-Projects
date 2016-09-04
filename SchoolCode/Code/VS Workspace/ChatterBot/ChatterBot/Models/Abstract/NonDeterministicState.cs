using System.Collections.Generic;

namespace ChatterBot.Models.Abstract {
    public abstract class NonDeterministicState {

        public virtual string Name {
            get {

                return GetType().Name;
            }
        }

        public virtual bool HasAcceptance {
            get {
                return false;
            }
        }

        public abstract IEnumerable<NonDeterministicState> Propose(dynamic args);
    }
}
