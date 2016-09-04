using System.Collections.Generic;
using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class NounPhrase : Token {

        public NounPhrase(IEnumerable<Token> children) {

            Children = children.ToList();
        }

        public static bool Propose(IEnumerable<Token> inParts) {

            var parts = inParts.ToArray();

            return (parts.Length == 2 && parts[0] is Article && parts[1] is Noun)
                   || (parts.Length == 2 && parts[0] is Preposition && parts[1] is Noun)
                   || (parts.Length == 1 && parts[0] is Noun)
                   || (parts.Length == 4
                       && parts[0] is Article
                       && parts[1] is Noun
                       && parts[2] is Preposition
                       && parts[3] is NounPhrase);
        }

    }

}