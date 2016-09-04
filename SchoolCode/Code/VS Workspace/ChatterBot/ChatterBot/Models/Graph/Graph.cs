using System.Collections.Generic;
using System.Linq;

namespace ChatterBot.Models.Graph {

    public class PathLookup {

        public Node Start {
            get;
            set;
        }

        public Node End {
            get;
            set;
        }

        public PathLookup(Node start, Node end) {

            Start = start;
            End = end;
        }
    }

    public class Graph {

        private Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
        private Dictionary<Edge, Edge> _edges = new Dictionary<Edge, Edge>();

        public Dictionary<PathLookup, IEnumerable<IEnumerable<Edge>>> Paths {
            get;
            set;
        }

        public Dictionary<string, Node> Nodes {
            get {
                return _nodes;
            }
            set {
                _nodes = value;
            }
        }

        public Dictionary<Edge, Edge> Edges {
            get {
                return _edges;
            }
            set {
                _edges = value;
            }
        }

        public Node this[string a] {
            get {
                return Nodes[a];
            }
        }

        public bool MakesGraph(IEnumerable<Edge> edges, Node start, Node end) {

            var nodeList = Nodes.Keys.Concat(Nodes.Keys).ToList();

            foreach(var edge in edges) {

                nodeList.Remove(edge.Left.Name);
                nodeList.Remove(edge.Right.Name);
            }

            return nodeList.Count == 2 && nodeList.Contains(start.Name) && nodeList.Contains(end.Name);
        }

        public void Add(string left, string right, double weight = 1) {

            var leftNode = CreateOrReturnNode(left);
            var rightNode = CreateOrReturnNode(right);

            CreateOrReturnEdge(leftNode, rightNode, weight);
        }

        private Node CreateOrReturnNode(string name) {

            var node = new Node {
                Name = name
            };

            if(Nodes.ContainsKey(name)) {
                node = Nodes[name];
            } else {
                Nodes.Add(name, node);
            }

            return node;
        }

        private Edge CreateOrReturnEdge(Node left, Node right, double weight) {

            var edge = new Edge {
                Left = left,
                Right = right,
                Weight = weight
            };

            if(Edges.ContainsKey(edge)) {
                edge = Edges[edge];
            } else {
                Edges.Add(edge, edge);
            }

            return edge;
        }
    }
}
