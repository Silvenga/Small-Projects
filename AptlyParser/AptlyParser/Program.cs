using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RestSharp;

namespace AptlyParser
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            ReplaceVersions(args[0], args[1], args[2], args[3]);
        }

        public static void ReplaceVersions(string aptlyUrl, string repoName, string file, string lookup)
        {
            var client = new RestClient(aptlyUrl);
            var request = new RestRequest("api/repos/{repo}/packages", Method.GET);
            request.AddUrlSegment("repo", repoName);
            var list = client.Execute<List<string>>(request).Data;

            var packages = list.Select(x => x.Split())
                                .GroupBy(x => x[1])
                                .Select(g => new
                                {
                                    Name = g.Key,
                                    Versions = g.Select(x => x[2]).OrderBy(x => x, new VersionComparer()).Distinct().Reverse(),
                                    Archs = g.Select(x => x[0].Substring(1)).Distinct().OrderBy(x => x)
                                })
                                .OrderBy(x => x.Name);
            var str = "";

            foreach (var package in packages)
            {
                str += Environment.NewLine;
                str += $"{package.Name} ({string.Join(", ", package.Archs)})" + Environment.NewLine;
                foreach (var version in package.Versions)
                {
                    str += $"\t{version}" + Environment.NewLine;
                }
            }

            var fileStr = File.ReadAllText(file).Replace(lookup, str);
            File.WriteAllText(file, fileStr);
        }
    }
}