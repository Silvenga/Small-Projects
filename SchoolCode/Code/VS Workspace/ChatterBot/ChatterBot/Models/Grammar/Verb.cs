using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class Verb : Token {

        public override string Response {
            get {
                var reponse = base.Response;

                if(Value.Equals("am")) {
                    reponse = "are";
                }

                return reponse;
            }
        }

        public Verb(string word) {

            Value = word;
        }

        public static bool Propose(string word) {

            var words = new[] {
                "bites",
                "chases",
                "am",
                "is",
                "love",
                "are",
                "like",
                "run",
                "add",
                "allow",
                "bake",
                "bang",
                "call",
                "chase",
                "damage	",
                "drop",
                "end",
                "escape",
                "fasten",
                "fix",
                "gather",
                "grab",
                "hang",
                "hug",
                "imagine",
                "itch",
                "jog",
                "jump",
                "kick",
                "knit",
                "land",
                "lock",
                "march",
                "mix",
                "name",
                "notice",
                "obey",
                "open",
                "pass",
                "promise",
                "question",
                "reach",
                "rinse",
                "scatter",
                "stay",
                "talk",
                "turn",
                "untie",
                "use",
                "vanish",
                "visit",
                "walk",
                "work",
                "yawn",
                "yell",
                "zip",
                "zoom",
            };

            return words.Contains(word);
        }
    }

}