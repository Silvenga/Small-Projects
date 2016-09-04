using System.Collections.Generic;
using System.Linq;

using ChatterBot.Models.Abstract;

namespace ChatterBot.Models.SM {

    public static class NonDeterministicStateMachine {

        public static bool Start(string str) {

            List<NonDeterministicState> states = new List<NonDeterministicState> {
                new A()
            };

            foreach(var c in str) {

                var newStates = new List<NonDeterministicState>();

                foreach(var state in states) {

                    var newState = state.Propose(c);

                    newStates.AddRange(newState);
                }

                states = newStates;
            }

            return states.Any(x => x.HasAcceptance);
        }

        class A : NonDeterministicState {

            public override IEnumerable<NonDeterministicState> Propose(dynamic args) {

                if(args.Equals('0')) {
                    yield return new B();
                }

                yield return new A();
            }

        }

        class B : NonDeterministicState {

            public override IEnumerable<NonDeterministicState> Propose(dynamic args) {

                if(args.Equals('1')) {
                    yield return new C();
                }
            }

        }

        class C : NonDeterministicState {

            public override IEnumerable<NonDeterministicState> Propose(dynamic args) {

                if(args.Equals('0')) {
                    yield return new D();
                }
            }

        }

        class D : NonDeterministicState {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override IEnumerable<NonDeterministicState> Propose(dynamic args) {

                yield return new D();
            }

        }
    }
}
