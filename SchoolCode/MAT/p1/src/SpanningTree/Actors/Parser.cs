using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpanningTree.Models;

namespace SpanningTree.Actors {
    public class Parser {

        public static BasicGraph Do(string filePath) {

            Console.WriteLine("Reading file {0}", filePath);

            if(!File.Exists(filePath)) {

                throw new Exception(string.Format("File {0} does not exist. Will not continue.", filePath));
            }

            var lines = File.ReadAllLines(filePath);

            var index = 0;
            var graph = new BasicGraph();

            foreach(var line in lines) {

                index++;

                if(string.IsNullOrWhiteSpace(line)) {
                    continue;
                }

                Console.WriteLine("Parsing line {0}.", index);

                var triplet = line.Split(',').Select(x => x.Trim()).ToList();

                if(triplet.Count() != 3) {
                    Console.WriteLine(" Invalid format, skipping.");
                    continue;
                }

                var left = triplet[0];
                var right = triplet[1];
                double weight;

                if(!Double.TryParse(triplet[2], out weight)) {
                    Console.WriteLine(" Could not parse weight, skipping.");
                    continue;
                }

                Console.WriteLine(" Parsed Edge {0}<->{1} with a weight of {2}", left, right, weight);
                graph.AddEdge(left, right, weight);
            }

            Console.WriteLine("Parsing complete.");
            return graph;
        }
    }
}
