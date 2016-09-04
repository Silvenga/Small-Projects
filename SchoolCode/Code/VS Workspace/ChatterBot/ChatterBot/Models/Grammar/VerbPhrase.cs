using System.Collections.Generic;
using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class VerbPhrase : Token {

        public VerbPhrase(IEnumerable<Token> children) {

            Children = children.ToList();
        }

        public static bool Propose(IEnumerable<Token> inParts) {

            var parts = inParts.ToArray();

            return (parts.Length == 1 && parts[0] is Verb)
                   || (parts.Length == 2 && parts[0] is Pronoun && parts[1] is Verb)
                   || (parts.Length == 2 && parts[0] is Verb && parts[1] is Pronoun)
                   || (parts.Length == 2 && parts[0] is Verb && parts[1] is NounPhrase);
        }
    }

}