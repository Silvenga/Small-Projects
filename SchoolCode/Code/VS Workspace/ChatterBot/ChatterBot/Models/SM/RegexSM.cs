using System.Collections.Generic;
using System.Linq;

using ChatterBot.Models.Abstract;

namespace ChatterBot.Models.SM {
    public static class RegexSM {

        public static bool TryPropose(string input, out string response) {

            var chain = ProposeChain(input);

            var state = chain.Last();

            response = state.Response;
            return state.HasAcceptance;
        }

        public static IEnumerable<State<string>> ProposeChain(string input) {

            State<string> state = new A();

            foreach(var c in input.ToLower()) {

                state = state.Propose(c);
                yield return state;
            }
        } 

        class A : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('H')) {
                    return new B();
                }
                if(args.Equals('h')) {
                    return new B();
                }

                if(args.Equals('A')) {
                    return new H();
                }
                if(args.Equals('a')) {
                    return new H();
                }

                if(args.Equals('T')) {
                    return new M();
                }
                if(args.Equals('t')) {
                    return new M();
                }

                return new Fail();
            }
        }

        class B : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('i')) {
                    return new C();
                }

                if(args.Equals('e')) {
                    return new D();
                }

                if(args.Equals('o')) {
                    return new I();
                }

                return new Fail();
            }
        }

        class C : State<string> {

            public override string Response {
                get {
                    return "";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }

        class D : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('l')) {
                    return new E();
                }

                return new Fail();
            }
        }

        class E : State<string> {

            public override State<string> Propose(dynamic args) {


                if(args.Equals('l')) {
                    return new F();
                }

                return new Fail();
            }
        }

        class F : State<string> {


            public override State<string> Propose(dynamic args) {

                if(args.Equals('o')) {
                    return new G();
                }

                return new Fail();
            }
        }

        class G : State<string> {

            public override string Response {
                get {
                    return "";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }
        class H : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('l')) {
                    return new N();
                }

                return new Fail();
            }
        }
        class I : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('w')) {
                    return new J();
                }

                return new Fail();
            }
        }
        class J : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('d')) {
                    return new K();
                }

                return new Fail();
            }
        }
        class K : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('y')) {
                    return new L();
                }

                return new Fail();
            }
        }
        class L : State<string> {

            public override string Response {
                get {
                    return "";
                }
            }

            public override State<string> Propose(dynamic args) {
                return new Fail();
            }
        }
        class M : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('h')) {
                    return new R();
                }

                return new Fail();
            }
        }
        class N : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('o')) {
                    return new O();
                }

                return new Fail();
            }
        }
        class O : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('h')) {
                    return new P();
                }


                return new Fail();
            }
        }
        class P : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('a')) {
                    return new Q();
                }

                return new Fail();
            }
        }
        class Q : State<string> {

            public override string Response {
                get {
                    return "";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }

        class R : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('a')) {
                    return new S();
                }

                return new Fail();
            }
        }
        class S : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('n')) {
                    return new T();
                }

                return new Fail();
            }
        }
        class T : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('k')) {
                    return new U();
                }

                return new Fail();
            }
        }
        class U : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals(' ')) {
                    return new V();
                }

                if(args.Equals('s')) {
                    return new W();
                }

                return new Fail();
            }
        }
        class V : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals(' ')) {
                    return new V();
                }

                if(args.Equals('y')) {
                    return new X();
                }

                return new Fail();
            }
        }
        class W : State<string> {

            public override string Response {
                get {
                    return "";
                }
            }

            public override State<string> Propose(dynamic args) {

                return new Fail();
            }
        }
        class X : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('o')) {
                    return new Y();
                }


                return new Fail();
            }
        }
        class Y : State<string> {

            public override State<string> Propose(dynamic args) {

                if(args.Equals('u')) {
                    return new Z();
                }

                return new Fail();
            }
        }

        class Z : State<string> {

            public override string Response {
                get {
                    return "";
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
