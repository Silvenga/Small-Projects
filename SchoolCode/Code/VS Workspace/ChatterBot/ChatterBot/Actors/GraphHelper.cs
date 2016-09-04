using System;
using System.Collections.Generic;
using System.Linq;

using ChatterBot.Models.Graph;

namespace ChatterBot.Actors {

    public static class GraphHelper {

        public static IEnumerable<T> ConsecutiveNotNullDistict<T>(this IEnumerable<T> enumerable) {

            var lastSeen = default(T);
            foreach(var t in enumerable) {

                if(t != null && !t.Equals(lastSeen)) {

                    yield return t;
                }

                lastSeen = t;
            }
        }

        public static IEnumerable<Edge> ShortestPath(this Graph graph, Node start, Node end) {

            var cache = new Dictionary<IEnumerable<Edge>, double>();
            var allPossiblePaths = graph.FindAllPaths(start, end);

            var bestweight = 0D;
            IEnumerable<Edge> bestPath = null;

            foreach(var path in allPossiblePaths.Select(x => x.ToList())) {

                double weight;
                cache.TryGetValue(path, out weight);

                if(weight.Equals(default(double))) {

                    weight = path.Sum(x => x.Weight);
                    cache.Add(path, weight);
                }

                if(weight > bestweight) {

                    bestweight = weight;
                    bestPath = path;
                }
            }

            return bestPath;
        }

        public static IEnumerable<IEnumerable<Edge>> FindAllPaths(this Graph graph, Node start, Node end) {

            IEnumerable<IEnumerable<Edge>> startEdges = graph.NextPossibles(start).Select(x => new List<Edge> { x });

            foreach(var possible in graph.FindPaths(startEdges, start, end, 1)) {

                yield return possible;
            }
        }

        public static IEnumerable<IEnumerable<Edge>> FindPaths(this Graph graph, IEnumerable<IEnumerable<Edge>> currentPaths, Node start, Node end, int round) {

            var foundPossibles = graph.FindPath(currentPaths, round).Distinct().ToList();

            var correct = foundPossibles.Where(x => graph.MakesGraph(x, start, end)).ToList();
            var notThere = foundPossibles.Where(x => !graph.MakesGraph(x, start, end));

            Console.WriteLine("{0} possible paths found.", foundPossibles.Count);

            if(correct.Any()) {

                Console.WriteLine("Found {0} correct paths found, checking for best path.", correct.Count);

                foreach(var possible in correct) {

                    yield return possible;
                }

            } else {

                Console.WriteLine("No correct paths found, branching again.");

                foreach(var path in graph.FindPaths(notThere, start, end, round + 1)) {

                    yield return path;
                }
            }
        }

        public static IEnumerable<List<Edge>> FindPath(this Graph graph, IEnumerable<IEnumerable<Edge>> currentSpans, int round) {

            foreach(var currentSpan in currentSpans) {

                var span = currentSpan.ToList();

                foreach(var possible in NextPossibles(graph, span.Last().Right)) {

                    var results = Combine(span, possible).ToList();
                    //if(results.Count > round)
                    yield return results;
                }

                foreach(var possible in NextPossibles(graph, span.Last().Left)) {

                    var results = Combine(span, possible).ToList();
                    //if(results.Count > round)
                    yield return results;
                }
            }
        }

        public static IEnumerable<Edge> Combine(this IEnumerable<Edge> edges, Edge last) {

            var hash = new List<Edge>();

            foreach(var edge in edges) {

                hash.Add(edge);
            }

            hash.Add(last);

            return hash;
        }

        public static IEnumerable<Edge> NextPossibles(this Graph graph, Node node) {

            return graph.Edges.Where(x => x.Value.Left.Equals(node) || x.Value.Right.Equals(node)).Select(x => x.Value);
        }
    }
}
