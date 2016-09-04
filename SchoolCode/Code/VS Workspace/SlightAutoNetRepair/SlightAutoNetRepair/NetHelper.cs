using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SlightAutoNetRepair {
    class NetHelper {

        public static Task<bool> AsyncPing(string address, int attempts = 4) {

            var ping = new Ping();

            return Task.Run(delegate {

                for(var i = 0; i < attempts; i++) {

                    var result = ping.Send(address);

                    if(result != null && result.Status == IPStatus.Success)
                        return true;
                }

                return false;
            });
        }
    }
}
