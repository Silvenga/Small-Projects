using System;
using System.Linq;

using SpanningTree.Actors;
using SpanningTree.Models;

namespace SpanningTree {

    public class Program {

        static void Main(string[] args) {

            try {

                var graph = Parser.Do("graph.csv");

                var prims = graph.Prim();
                var kruskal = graph.Kruskal();
                Console.WriteLine("Prims -------------------");
                graph.PrintEdgesWithWeight(prims);
                Console.WriteLine("Kruskal -----------------");
                graph.PrintEdgesWithWeight(kruskal);

            } catch(Exception e) {

                Console.WriteLine("Error: {0}", e.Message);

            } finally {

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

    }
}
