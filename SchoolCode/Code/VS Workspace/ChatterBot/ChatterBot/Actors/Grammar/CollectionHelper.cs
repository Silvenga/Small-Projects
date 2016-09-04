using System;
using System.Collections.Generic;
using System.Linq;

using ChatterBot.Models.Grammar;

namespace ChatterBot.Actors.Grammar {

    public static class CollectionHelper {

        public static Random Random {
            get {
                return _random ?? (_random = new Random());
            }
            set {
                _random = value;
            }
        }

        public static IEnumerable<Token> OrderedSearch(this Token token) {

            if(token.IsWord) {
                yield return token;
            } else {
                foreach(var child in token.Children) {
                    foreach(var result in OrderedSearch(child)) {
                        yield return result;
                    }
                }
            }
        }

        public static string TokensToString(this IEnumerable<Token> tokens) {

            return string.Join(" ", tokens);
        }

        private static readonly List<string> _prefixes = new List<string> {
            "How do you feel about the fact that " ,
            "Does it make you sad when ",
            "Do you like it when "
        };

        private static Random _random;

        public static T NextMod<T>(this IEnumerable<T> enumerable) {

            var list = enumerable.ToList();

            return list.ElementAt(Random.Next(list.Count));
        }

        public static string TokensToResponse(this IEnumerable<Token> tokens) {

            return _prefixes.NextMod() + string.Join(" ", tokens.Select(x => x.Response)) + "?";
        }

        public static T[] Subarray<T>(this T[] data, int index, int length) {

            var result = new T[length];
            if(length != 0) {
                Array.Copy(data, index, result, 0, length);
            }
            return result;
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable) {

            var list = new Stack<T>();

            foreach(var i in enumerable.Reverse()) {

                list.Push(i);
            }

            return list;
        }

        public static bool HasSameElementsAs<T>(this IEnumerable<T> first, IEnumerable<T> second) {

            var firstMap = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var secondMap = second
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return
                firstMap.Keys.All(x =>
                    secondMap.Keys.Contains(x) && firstMap[x] == secondMap[x]
                ) &&
                secondMap.Keys.All(x =>
                    firstMap.Keys.Contains(x) && secondMap[x] == firstMap[x]
                );
        }
    }

    public class TokenListComparer : IEqualityComparer<IEnumerable<Token>> {

        public bool Equals(IEnumerable<Token> a, IEnumerable<Token> b) {

            return a.HasSameElementsAs(b);
        }

        public int GetHashCode(IEnumerable<Token> obj) {

            // Don't really know why this is required... but it is...
            return obj.Count();
        }

    }
}
