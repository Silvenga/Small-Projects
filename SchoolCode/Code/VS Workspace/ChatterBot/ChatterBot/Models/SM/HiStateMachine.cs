using ChatterBot.Models.Abstract;

namespace ChatterBot.Models.SM {

    public static class HiStateMachine {

        public static bool Start(string str) {

            State<object> state = new LookingForH();

            foreach(var c in str) {

                state = state.Propose(c);
            }

            return state.HasAcceptance;
        }

        class LookingForH : State<object> {

            public override State<object> Propose(dynamic nextChar) {

                if(nextChar.Equals('h')) {
                    return new LookingForI();
                }

                return new Fail();
            }
        }

        class LookingForI : State<object> {

            public override State<object> Propose(dynamic nextChar) {

                if(nextChar.Equals('i')) {
                    return new Acceptance();
                }

                return new Fail();
            }
        }

        class Acceptance : State<object> {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override State<object> Propose(dynamic nextChar) {

                return new LookingForH();
            }
        }

        class Fail : State<object> {

            public override State<object> Propose(dynamic nextChar) {

                return new Fail();
            }
        }
    }
}
