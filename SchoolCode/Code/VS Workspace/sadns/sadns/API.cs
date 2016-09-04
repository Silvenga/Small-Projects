
using System.IO;
using System.Net;

namespace sadns {
    public static class API {

        private const string APIUrl = "http://update.lopezcloud.com";

        public static string ClientIP() {

            string ip;
            DoGet(APIUrl + "/ip", out ip);

            return ip;
        }

        public static bool DoFollow(string domain, out string response) {

            string requestUrl = APIUrl + "/" + domain;

            return DoGet(requestUrl, out response);
        }

        public static bool DoReset(string domain, out string response) {

            string requestUrl = APIUrl + "/" + domain + "?ip=" + "reset";

            return DoGet(requestUrl, out response);
        }

        public static bool DoCustom(string domain, string ipAdress, out string response) {

            string requestUrl = APIUrl + "/" + domain + "?ip=" + ipAdress;

            return DoGet(requestUrl, out response);
        }

        private static bool DoGet(string url, out string responseString) {

            responseString = default(string);

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            bool success = response.StatusCode == HttpStatusCode.OK;

            if(success) {

                Stream stream = response.GetResponseStream();

                if(stream != null) {
                    using(StreamReader streamReader = new StreamReader(stream)) {

                        responseString = streamReader.ReadToEnd();
                    }
                }

            } else {

                responseString = "Failed: " + response.StatusCode + " " + response.StatusDescription;
            }

            return success;
        }
    }
}
