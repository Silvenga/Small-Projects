using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class Preposition : Token {

        public Preposition(string word) {

            Value = word;
        }

        public static bool Propose(string word) {

            var words = new[] {
                "with",
                "to"
            };

            return words.Contains(word);
        }

    }

}