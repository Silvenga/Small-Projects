using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class Pronoun : Token {

        public override string Response {
            get {

                var reponse = base.Response;

                if(Value.Equals("i")) {
                    reponse = "you";
                } else if(Value.Equals("me")) {
                    reponse = "you";
                }

                return reponse;
            }
        }

        public Pronoun(string word) {

            Value = word;
        }

        public static bool Propose(string word) {

            var words = new[] {
                "i",
                "me"
            };

            return words.Contains(word);
        }

    }

}