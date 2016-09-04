using ChatterBot.Models.Abstract;

namespace ChatterBot.Models {

    public static class HiStateMachine {

        public static bool Start(string str) {

            State state = new A();

            foreach(var c in str) {

                state = state.Propose(c);
            }

            return state.IsAcceptance;
        }

        class A : State {

            public override State Propose(dynamic nextChar) {

                if(nextChar.Equals('h')) {
                    return new B();
                }

                return new D();
            }
        }

        class B : State {

            public override State Propose(dynamic nextChar) {

                if(nextChar.Equals('i')) {
                    return new C();
                }

                return new D();
            }
        }

        class C : State {

            public override bool IsAcceptance {
                get {
                    return true;
                }
            }

            public override State Propose(dynamic nextChar) {

                return new A();
            }
        }

        class D : State {

            public override State Propose(dynamic nextChar) {

                return new D();
            }
        }
    }
}
