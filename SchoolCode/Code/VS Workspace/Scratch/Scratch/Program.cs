using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Scratch {
    class Program {
        static void Main(string[] args) {


            //var all = File.ReadAllLines("log.txt");
            //var changes = all.Where(x => x.Contains("insertion") || x.Contains("deletion"));

            //var creations = 0;
            //var commits = 0;

            //foreach(var change in changes) {

            //    int temp;
            //    var numbers = change.Split(' ').Select(x => x.Trim()).Where(x => int.TryParse(x, out temp)).ToArray();

            //    if(numbers.Length == 2) {

            //        Console.WriteLine("{0}", numbers[1]);
            //        creations += int.Parse(numbers[1]);

            //        commits++;
            //    } else if(numbers.Length == 3) {
            //        Console.WriteLine("{0} {1}", numbers[1], numbers[2]);
            //        creations += int.Parse(numbers[1]) + int.Parse(numbers[2]);
            //        commits++;
            //    }

            //}

            //Console.WriteLine("{1} commits with {0} total", creations, commits);




            var a = new[] {
                "us-midwest.privateinternetaccess.com          ",
                "us-east.privateinternetaccess.com             ",
                "us-texas.privateinternetaccess.com            ",
                "us-west.privateinternetaccess.com             ",
                "us-california.privateinternetaccess.com       ",
                "us-seattle.privateinternetaccess.com          ",
                "us-florida.privateinternetaccess.com          ",
                "ca.privateinternetaccess.com                  ",
                "ca-toronto.privateinternetaccess.com          ",
                "uk-london.privateinternetaccess.com           ",
                "uk-southampton.privateinternetaccess.com      ",
                "swiss.privateinternetaccess.com               ",
                "nl.privateinternetaccess.com                  ",
                "sweden.privateinternetaccess.com              ",
                "france.privateinternetaccess.com              ",
                "germany.privateinternetaccess.com             ",
                "romania.privateinternetaccess.com             ",
                "hk.privateinternetaccess.com                  ",
                "israel.privateinternetaccess.com              ",
                "aus.privateinternetaccess.com                 ",
                "japan.privateinternetaccess.com               ",
            }.Select(x => x.Trim());


            var b = a.Select(PingHost).ToList();

            while(b.Count > 0) {

                var task = Task.WhenAny(b).Result;
                b.Remove(task);

                var result = task.Result;

                var replys = result.PingReplies;
                var success = replys.Count(x => x.Status == IPStatus.Success);

                var average = (success < 1) ? -1 : replys.Where(x => x.Status == IPStatus.Success).Sum(x => x.RoundtripTime) / success;

                Console.WriteLine("{0}/{1} obtainable; {2}ms ping; {3}", success, replys.Count, average, result.Host);
            }

            Console.ReadLine();
        }


        public static async Task<Reponse> PingHost(string host) {

            var pinger = new Ping();

            var b = new List<PingReply>();

            foreach(var ipAddress in GetAllAddresses(host).Distinct()) {

                var reply = await pinger.SendPingAsync(ipAddress);
                b.Add(reply);
            }

            return new Reponse {
                Host = host,
                PingReplies = b
            };
        }

        public static IEnumerable<IPAddress> GetAllAddresses(string host) {

            for(var i = 0; i < 100; i++) {

                foreach(var ipAddress in Dns.GetHostEntry(host).AddressList) {

                    yield return ipAddress;
                }
            }
        }

        internal class Reponse {

            public string Host {
                get;
                set;
            }

            public List<PingReply> PingReplies {
                get;
                set;
            }
        }

        public static int Mod(int x, int m) {
            var r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
