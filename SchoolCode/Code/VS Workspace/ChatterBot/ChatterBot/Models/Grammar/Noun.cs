using System.Linq;

namespace ChatterBot.Models.Grammar {

    public class Noun : Token {

        public Noun(string word) {

            Value = word;
        }

        public static bool Propose(string word) {

            var words = new[] {
                "dogs",
                "dog",
                "cats",
                "cat",
                "fishes",
                "fish",
                "golf",
                "ball",
                "bat",
                "bed",
                "book",
                "boy",
                "bun",
                "can",
                "cake",
                "cap",
                "car",
                "cat",
                "cow",
                "cub",
                "cup",
                "dad",
                "day",
                "dog",
                "doll",
                "dust",
                "fan",
                "feet",
                "girl",
                "gun",
                "hall",
                "hat",
                "hen",
                "jar",
                "kite",
                "man",
                "map",
                "men",
                "mom",
                "pan",
                "pet",
                "pie",
                "pig",
                "pot",
                "rat",
                "son",
                "sun",
                "toe",
                "tub",
                "van",
            };

            return words.Contains(word);
        }
    }

}