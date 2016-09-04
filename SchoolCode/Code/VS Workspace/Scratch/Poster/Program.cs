using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using HtmlAgilityPack;

namespace Poster {
    class Program {
        static void Main(string[] args) {

            var perfectCubes = new HashSet<int> {
                1,8,27,64,125,216,343,512,729,1000
            };
            
            foreach(var i in perfectCubes)
                foreach(var f in perfectCubes)
                    if(perfectCubes.Contains(i + f))
                        Console.WriteLine("Found {0}, {1}", i, f);


            return;

            const string data = "CharityId=24";

            //  FindLinks("http://www.oceanfinance.co.uk/win-the-tin/");

            while(true) {
                Console.WriteLine(DateTime.Now);
                try {

                    var request = (HttpWebRequest) WebRequest.Create("http://www.oceanfinance.co.uk/win-the-tin/blog/vote");

                    request.Headers.Clear();
                    request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                    request.Referer = "http://www.oceanfinance.co.uk/win-the-tin/";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Method = "POST";
                    request.Headers.Add("Origin", "http://www.oceanfinance.co.uk");
                    request.ContentLength = data.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AllowAutoRedirect = false;

                    using(var stream = request.GetRequestStream()) {
                        using(var myWriter = new StreamWriter(stream)) {
                            myWriter.Write(data);
                        }
                    }

                    using(var objResponse = (HttpWebResponse) request.GetResponse()) {
                        Console.WriteLine(objResponse.StatusCode);
                    }

                } catch(Exception) {

                    Console.WriteLine("Error");
                }
            }
        }

        public static IEnumerable<Link> FindLinks(string page) {

            var hw = new HtmlWeb();
            var doc = hw.Load(page);

            var nodes = doc.DocumentNode.SelectNodes("//form");

            return null;
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
