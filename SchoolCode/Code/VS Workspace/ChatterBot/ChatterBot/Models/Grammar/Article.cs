using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class Article : Token {

        public Article(string word) {

            Value = word;
        }

        public static bool Propose(string word) {

            var words = new[] {
                "a",
                "the",
            };

            return words.Contains(word);
        }
    }

}