
using System.Threading.Tasks;

using Amazon.Route53.Model;

using Slight.Route53Manager.Models;

namespace Slight.Route53Manager.Actor {
    public static class Zone {

        public static async Task<HostedZone> CreateAsync(Credentials credentials, string domainName) {

            var client = credentials.CreateClient();
            var reference = AwsHelper.GenerateReferenceToken();

            var zoneRequest = new CreateHostedZoneRequest {
                Name = domainName,
                CallerReference = reference
            };

            var response = await client.CreateHostedZoneAsync(zoneRequest);

            await response.ChangeInfo.WaitForStatusAync(credentials);

            return response.HostedZone;
        }
    }
}
