using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace SlightLibrary.Helpers {

    public static class NetHelper {

        private const string CheckIPWebAdress = "http://checkip.dyndns.org";

        /// <summary>
        /// Get the user's computer public/external IPv4 adress
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetPublicIP() {

            IPAddress returnIP = null;
            WebRequest request = WebRequest.CreateHttp(CheckIPWebAdress);
            WebResponse requestRespose = request.GetResponse();
            Stream genericStream = requestRespose.GetResponseStream();

            if (genericStream != null) {

                StreamReader stream = new StreamReader(genericStream);
                string plainRespose = stream.ReadToEnd();
                plainRespose = plainRespose.Split(':')[1];
                plainRespose = plainRespose.Split('<')[0];
                returnIP = IPAddress.Parse(plainRespose.Trim());
            }

            return returnIP;
        }

        /// <summary>
        /// Returns a hash code for the current state of the network 
        /// Included is the DHCP servers and number of Interfaces
        /// </summary>
        /// <returns></returns>
        public static int GetLocalNetworkState() {

            return NetworkInterface.GetAllNetworkInterfaces().Select(i => i.GetIPProperties().DhcpServerAddresses).Aggregate(1, (current1, tempGate) => tempGate.Aggregate(current1, (current, j) => current * j.MapToIPv4().GetHashCode()));
        }
    }
}