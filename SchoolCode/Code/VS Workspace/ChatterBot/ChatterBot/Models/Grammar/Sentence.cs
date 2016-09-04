using System;
using System.Collections.Generic;
using System.Linq;

using ChatterBot.Actors.Grammar;

namespace ChatterBot.Models.Grammar {

    public class Sentence : Token {

        public Sentence(IEnumerable<Token> children) {

            Children = children.ToList();
        }

        public static bool Propose(IEnumerable<Token> inParts) {

            var parts = inParts.ToArray();

            return (parts.Length == 2 && parts[0] is VerbPhrase && parts[1] is NounPhrase)
                   || (parts.Length == 2 && parts[0] is NounPhrase && parts[1] is VerbPhrase);
        }
    }

}