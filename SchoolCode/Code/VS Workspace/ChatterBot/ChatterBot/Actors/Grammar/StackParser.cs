using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ChatterBot.Models.Grammar;
using ChatterBot.Models.Graph;

namespace ChatterBot.Actors.Grammar {

    public static class StackParser {

        private static List<Token> _tokens = new List<Token>();

        public static List<Token> Tokens {
            get {
                return _tokens ?? (_tokens = new List<Token>());
            }
            set {
                _tokens = value;
            }
        }

        public static void Main() {

            while(true) {

                var line = Console.ReadLine();

                Token output;

                if(TryParseFirst(line, out output)) {

                    _tokens.Add(output);

                    var response = output.OrderedSearch().TokensToResponse();

                    Console.WriteLine(response);

                } else {

                    if(line != null && File.Exists(line)) {

                        var lines = File.ReadLines(line);
                        var graph = new Graph();

                        foreach(var line1 in lines) {

                            var tokens = line1.Split(',').Select(x => x.Trim()).ToArray();

                            graph.Add(tokens[0], tokens[1], double.Parse(tokens[2]));
                        }

                        var path = graph.FindAllPaths(graph["start"], graph["end"]).First().ToList();

                        foreach(var pa in path) {

                            Console.WriteLine(pa);
                        }
                        Console.WriteLine("Total weight: {0}", path.Sum(x => x.Weight));
                    }

                    if(_tokens.Any()) {
                        var anotherReposnse = _tokens.NextMod().OrderedSearch().TokensToResponse();
                        Console.WriteLine(anotherReposnse);
                    } else {
                        Console.WriteLine("Wat?");
                    }
                }
            }
        }


        public static bool TryParseFirst(string sentence, out Token token) {

            try {

                var head = Parse(sentence).First();

                token = head.First();
                return true;

            } catch(Exception) {

                token = default(Token);
                return false;
            }
        }

        public static IEnumerable<Stack<Token>> Parse(string sentence) {

            IEnumerable<Stack<Token>> stacks = new List<Stack<Token>> {
                new Stack<Token>()
            };

            foreach(var token in sentence.TokenizeSentence()) {

                stacks = ShiftToken(stacks, token);
            }

            return stacks
                .Distinct(new TokenListComparer())
                .Where(x => x.Count() == 1)
                .Where(x => x.First() is Sentence)
                .Select(x => x.ToStack());
        }

        public static IEnumerable<Stack<Token>> ShiftToken(IEnumerable<Stack<Token>> stacks, Token token) {

            foreach(var stack in stacks) {

                stack.Push(token);

                foreach(var possibleStack in RecusiveReduce(stack)) {

                    yield return possibleStack;
                }
            }
        }

        public static IEnumerable<Stack<Token>> RecusiveReduce(Stack<Token> stack) {

            return RecusiveReduce(
                new List<Stack<Token>> {
                    stack
                });
        }

        public static IEnumerable<Stack<Token>> RecusiveReduce(IEnumerable<Stack<Token>> stacks) {

            foreach(var stack in stacks) {

                var posposed = stack.ReductionOptions();
                foreach(var proposed in RecusiveReduce(posposed)) {

                    yield return proposed;
                }

                yield return stack;
            }
        }

    }
}
