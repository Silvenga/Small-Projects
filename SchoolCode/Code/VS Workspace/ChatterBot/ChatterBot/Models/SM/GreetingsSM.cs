using ChatterBot.Models.Abstract;

namespace ChatterBot.Models.SM {
    public static class GreetingsSM {

        public static bool TryPropose(string input, out string response) {

            State<string> state = new Bgjn();

            foreach(var c in input.ToLower()) {

                state = state.Propose(c);
            }

            response = state.Response;
            return state.HasAcceptance;
        }

        class Bgjn : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('m')) {
                    return new C();
                }

                if(args.Equals('h')) {
                    return new Hko();
                }

                return new Fail();
            }
        }

        class C : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('i')) {
                    return new D();
                }

                return new Fail();
            }
        }

        class D : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('k')) {
                    return new E();
                }

                return new Fail();
            }
        }

        class E : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('e')) {
                    return new F();
                }

                return new Fail();
            }
        }

        class F : State<string> {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override string Response {
                get {
                    return "You said mike";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }

        class Hko : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('i')) {
                    return new I();
                }
                if(args.Equals('e')) {
                    return new L();
                }
                if(args.Equals('a')) {
                    return new P();
                }

                return new Fail();
            }
        }

        class I : State<string> {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override string Response {
                get {
                    return "You said hi.";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }

        private class L : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('i')) {
                    return new M();
                }

                return new Fail();
            }

        }

        class M : State<string> {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override string Response {
                get {
                    return "You said hei.";
                }
            }

            public override State<string> Propose(dynamic args) {

                if(args.Equals('m')) {
                    return new C();
                }

                return new Fail();
            }
        }

        class P : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('y')) {
                    return new Q();
                }

                return new Fail();
            }
        }

        class Q : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('e')) {
                    return new R();
                }

                return new Fail();
            }
        }

        class R : State<string> {

            public override bool HasAcceptance {
                get {
                    return true;
                }
            }

            public override string Response {
                get {
                    return "You said haye.";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }

        class Fail : State<string> {

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }
    }
}
