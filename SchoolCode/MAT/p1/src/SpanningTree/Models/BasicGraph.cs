using System;
using System.Collections.Generic;

using QuickGraph;
using QuickGraph.Algorithms;

namespace SpanningTree.Models {

    public class BasicGraph {

        private HashSet<string> _nodes = new HashSet<string>();
        private Dictionary<UndirectedEdge<string>, double> _edges = new Dictionary<UndirectedEdge<string>, double>();

        public HashSet<string> Nodes {
            get {
                return _nodes;
            }
            set {
                _nodes = value;
            }
        }

        public Dictionary<UndirectedEdge<string>, double> Edges {
            get {
                return _edges;
            }
            set {
                _edges = value;
            }
        }

        public UndirectedGraph<string, UndirectedEdge<string>> CreateQuickGraph() {

            var backend = new UndirectedGraph<string, UndirectedEdge<string>>(false);

            foreach(var node in Nodes) {

                backend.AddVertex(node);
            }

            foreach(var edge in Edges) {

                backend.AddEdge(edge.Key);
            }

            return backend;
        }

        public void PrintEdgesWithWeight(IEnumerable<UndirectedEdge<string>> edges) {

            var totalWeight = 0d;

            foreach(var edge in edges) {

                var weight = Edges[edge];
                totalWeight += weight;
                var str = string.Format("{0}: {1}", edge, weight);

                Console.WriteLine(str);
            }

            Console.WriteLine("Total Weight: {0}",totalWeight);
        }

        public IEnumerable<UndirectedEdge<string>> Prim() {

            var graph = CreateQuickGraph();
            return graph.MinimumSpanningTreePrim(edge => Edges[edge]);
        }

        public IEnumerable<UndirectedEdge<string>> Kruskal() {

            var graph = CreateQuickGraph();
            return graph.MinimumSpanningTreeKruskal(edge => Edges[edge]);
        }

        public void AddEdge(string left, string right, double weight) {

            Nodes.Add(left);
            Nodes.Add(right);

            Edges.Add(new UndirectedEdge<string>(left, right), weight);
        }

        public double GetWeight(string left, string right) {

            return Edges[new UndirectedEdge<string>(left, right)];
        }

        private string NormalizeEdge(string left, string right) {

            if(String.Compare(left, right, StringComparison.Ordinal) < 0) {

                // Less than
                return left + right;
            }

            // Greater than
            return right + left;
        }
    }
}
