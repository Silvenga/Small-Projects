using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace PlexDownloader {

    class Program {

        static void Main(string[] args) {

            foreach(var link in FindLinks("https://plex.tv/downloads").Where(x => x.Href.EndsWith(".deb") && !x.Name.StartsWith("Intel"))) {

                Console.WriteLine(link.Name + ": " + link.Href);

                var sourceFile = new FileInfo(link.Href.Split('/').Last());

                var request = (HttpWebRequest) WebRequest.Create(link.Href);
                request.Method = "HEAD";
                var response = (HttpWebResponse) request.GetResponse();

                if(response.LastModified > sourceFile.LastWriteTime) {

                    new WebClient().DownloadFile(link.Href, sourceFile.Name);
                }
            }
        }

        public static IEnumerable<Link> FindLinks(string page) {

            var hw = new HtmlWeb();
            var doc = hw.Load(page);

            return doc.DocumentNode.SelectNodes("//a[@href]").Select(link => new Link {
                Href = link.Attributes["href"].Value,
                Name = link.InnerText
            }).Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Href) && x.Href.StartsWith("http"));
        }

    }

    public class Link {

        public string Href {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

    }
}
