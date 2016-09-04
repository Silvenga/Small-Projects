using System;
using System.Collections.Generic;
using System.Linq;

using ChatterBot.Models.Grammar;

namespace ChatterBot.Actors.Grammar {

    public static class GrammarHelper {

        public static IEnumerable<Token> TokenizeSentence(this string sentance) {

            return sentance.Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ParseWord())
                .Reverse();
        }

        public static Token ParseWord(this string word) {

            word = word.Trim().ToLower();

            if(Verb.Propose(word)) {
                return new Verb(word);
            }
            if(Noun.Propose(word)) {
                return new Noun(word);
            }
            if(Article.Propose(word)) {
                return new Article(word);
            }
            if(Preposition.Propose(word)) {
                return new Preposition(word);
            }
            if(Pronoun.Propose(word)) {
                return new Pronoun(word);
            }

            throw new Exception(String.Format("Can't parse {0}", word));
        }

        public static IEnumerable<Stack<Token>> ReductionOptions(this IEnumerable<Token> proposedStack) {

            var stack = proposedStack.ToArray();

            for(var j = 1; j <= stack.Length && j <= 4; j++) {

                var head = stack.Subarray(0, j);
                var tail = stack.Subarray(j, stack.Length - j);

                if(Sentence.Propose(head)) {

                    var possibleStack = new List<Token>();
                    possibleStack.Add(new Sentence(head));
                    possibleStack.AddRange(tail);

                    yield return possibleStack.ToStack();
                }
                if(VerbPhrase.Propose(head)) {

                    var possibleStack = new List<Token>();
                    possibleStack.Add(new VerbPhrase(head));
                    possibleStack.AddRange(tail);

                    yield return possibleStack.ToStack();
                }
                if(NounPhrase.Propose(head)) {

                    var possibleStack = new List<Token>();
                    possibleStack.Add(new NounPhrase(head));
                    possibleStack.AddRange(tail);

                    yield return possibleStack.ToStack();
                }
            }
        }

    }

}


