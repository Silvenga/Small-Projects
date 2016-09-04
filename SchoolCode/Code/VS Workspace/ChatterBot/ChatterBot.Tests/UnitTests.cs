using System;
using System.Collections.Generic;
using System.Linq;

using ChatterBot.Actors;
using ChatterBot.Actors.Grammar;
using ChatterBot.Models.Grammar;
using ChatterBot.Models.Graph;
using ChatterBot.Models.SM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatterBot.Tests {

    [TestClass]
    public class UnitTests {

        [TestMethod]
        public void GraphTest() {

            var graph = new Graph();

            // http://pages.cpsc.ucalgary.ca/~jacobs/Courses/cpsc331/W12/tutorials/Figures/weighted_graph_example2.gif

            graph.Add("0", "1", 1);
            graph.Add("0", "2", 6);
            graph.Add("1", "2", 4);
            graph.Add("1", "3", 3);
            graph.Add("2", "3", 1);
            graph.Add("3", "4", 1);

            var a = graph.ShortestPath(graph["0"], graph["4"]);
        }

        [TestMethod]
        public void Graph2Test() {

            var graph = new Graph();

            // http://pages.cpsc.ucalgary.ca/~jacobs/Courses/cpsc331/W12/tutorials/Figures/weighted_graph_example1.gif

            graph.Add("0", "1", 1);
            graph.Add("1", "3", 6);
            graph.Add("1", "2", 3);
            graph.Add("3", "2", 1);
            graph.Add("2", "4", 2);
            graph.Add("4", "6", 1);
            graph.Add("6", "5", 2);

            var a = graph.FindAllPaths(graph["0"], graph["5"]).Select(x => x.ToList()).ToList().OrderBy(x => x.Sum(c => c.Weight));
        }

      
        [TestMethod]
        public void GrammarTest() {

            var success = new List<string> {
                "I love a dog",
                "I love dogs",
                "I am a cat",
                "The dog chases the cat",
                "Cats love me",
                "A dog bites a cat",
                "I like to golf",
                "The cat with the cat with the fish chases the dog",
            };

            var failed = new List<string> {
                "",
                "  ",
                "\n",
                "dfsdf",
                "dog is love",
                "am is love",
                "dog cat fish",
            };

            foreach(var input in success) {

                Token head;
                var hasCorrectGrammer = StackParser.TryParseFirst(input, out head);

                Assert.IsTrue(hasCorrectGrammer);

                var str = head.OrderedSearch().TokensToString();

                Assert.AreEqual(input.ToLower().Trim(), str);
            }

            foreach(var input in failed) {

                Token head;
                var hasCorrectGrammer = StackParser.TryParseFirst(input, out head);

                Assert.IsFalse(hasCorrectGrammer);
            }
        }

        [TestMethod]
        public void ResponseTest() {

            //const string input = "I like to golf";
            const string input = "I am a cat";

            var stacks = StackParser.Parse(input);

            var primary = stacks.First().First().OrderedSearch();

            var str = primary.TokensToResponse();

            Assert.AreEqual("How do you feel about the fact that you are a cat?", str);
        }

        [TestMethod]
        public void HiTest() {

            Assert.IsTrue(HiStateMachine.Start("hi"));
            Assert.IsFalse(HiStateMachine.Start("hig"));
            Assert.IsFalse(HiStateMachine.Start("dhi"));
            Assert.IsFalse(HiStateMachine.Start("hg"));
        }

        [TestMethod]
        public void GreetingTest() {

            var success = new List<string> {
                "mike",
                "hi",
                "hei",
                "haye",
            };

            var fail = new List<string> {
                "amike",
                "mikea",
                "ike",
                "mik",
                "h",
                "he",
            };

            string temp;

            foreach(var s in success) {

                Assert.IsTrue(GreetingsSM.TryPropose(s, out temp));
                Assert.IsFalse(string.IsNullOrWhiteSpace(temp));
                Assert.IsTrue(temp.Contains(s));
                Console.WriteLine(temp);
            }

            foreach(var s in fail) {

                Assert.IsFalse(GreetingsSM.TryPropose(s, out temp));
                Assert.IsTrue(string.IsNullOrWhiteSpace(temp));
            }
        }

        [TestMethod]
        public void RegexTest() {

            var success = new List<string> {
                "Hi",
                "hi",
                "Hello",
                "hello",
                "Howdy",
                "howdy",
                "Aloha",
                "aloha",
                "Thanks",
                "thanks",
                "Thank you",
                "thank you",
            };

            var fail = new List<string> {
                "amike",
                "mikea",
                "ike",
                "mik",
                "h",
                "he",
            };

            string temp;

            foreach(var s in success) {

                Assert.IsTrue(RegexSM.TryPropose(s, out temp));
            }

            foreach(var s in fail) {

                Assert.IsFalse(RegexSM.TryPropose(s, out temp));
            }
        }

        [TestMethod]
        public void NonDeterministicTest() {

            Assert.IsTrue(NonDeterministicStateMachine.Start("111011010"));
            Assert.IsTrue(NonDeterministicStateMachine.Start("111011011011011010"));
            Assert.IsTrue(NonDeterministicStateMachine.Start("010"));

            Assert.IsFalse(NonDeterministicStateMachine.Start("111111"));
            Assert.IsFalse(NonDeterministicStateMachine.Start("00000"));
            Assert.IsFalse(NonDeterministicStateMachine.Start("11"));
        }

    }
}
