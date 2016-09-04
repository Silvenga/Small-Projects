using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dnsmasq.genhosts {

    static class Program {

        static void Main(string[] args) {

            if(args.Length != 2) {

                throw new Exception("Missing args: <source> <target>");
            }

            var file = args.First().Replace("\"", "");

            var lines = File.ReadAllLines(file);

            var list = Convert(lines).ToList();

            File.WriteAllLines(args.Last().Replace("\"", ""), list);

        }

        static IEnumerable<string> Convert(IEnumerable<string> lines) {

            var index = 0;
            var results = new List<string>();

            foreach(var line in lines.Select(Explode)) {

                index++;

                try {

                    results.AddRange(ParseLine(line));

                } catch(Exception e) {

                    Console.WriteLine(e.Message, index);
                }

            }

            return results;
        }

        public static IEnumerable<string> ParseLine(string[] line) {

            if(!IsComment(line)) {

                string hostname;
                string address;
                string options;

                AssertLineValidity(line, out hostname, out address, out options);

                if(options.Contains("a")) {

                    // address=/n0/10.0.0.100
                    yield return string.Format("address=/{0}/{1}", hostname, address);
                }

                if(options.Contains("p")) {

                    // ptr-record=100.0.0.10.in-addr.arpa,n0
                    yield return string.Format("ptr-record={0},{1}", ToPtr(address), hostname);
                }
            }
        }

        public static void AssertLineValidity(string[] line, out string hostname, out string address, out string options) {

            if(line.Length != 3) {
                throw new Exception("Error at Line: {0} (parsing error), skipping...");
            }

            hostname = line[0];
            address = line[1];
            options = line[2];

            if(!IsValidHost(hostname)) {
                throw new Exception("Error at Line: {0} (hostname failed validation via RFC1123), skipping...");
            }

            if(!IsValidAddress(address)) {
                throw new Exception("Error at Line: {0} (address failed validation), skipping...");
            }
        }

        public static bool IsValidAddress(string address) {

            const string validIpAddress =
                @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

            return Regex.IsMatch(address, validIpAddress);
        }

        public static bool IsValidHost(string host) {

            const string validHostname =
                @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";

            return Regex.IsMatch(host, validHostname);
        }

        public static string[] Explode(string line) {

            return Regex.Split(line.Trim(), @"\s+").Select(x => x.Trim()).ToArray();
        }

        public static bool IsComment(string[] line) {

            return (line.First().StartsWith("#") || line.Length <= 1);
        }

        static string ToPtr(this string address) {

            var parts = address.Split('.');

            if(parts.Length != 4)
                throw new Exception("Bad address: " + address);

            return string.Join(".", parts.Reverse()) + ".in-addr.arpa";
        }
    }
}
