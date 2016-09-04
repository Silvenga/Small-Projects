using System;
using System.Threading.Tasks;

using Amazon.Route53;
using Amazon.Route53.Model;

using Slight.Route53Manager.Models;

namespace Slight.Route53Manager.Actor {
    public static class AwsHelper {

        public static string GenerateReferenceToken() {

            var token = Guid.NewGuid().ToString();

            return token;
        }

        public static async Task<ChangeStatus> WaitForStatusAync(this ChangeInfo info, Credentials credentials) {

            var client = credentials.CreateClient();

            var changeRequest = new GetChangeRequest {
                Id = info.Id
            };

            ChangeStatus currentStatus;

            while((currentStatus = (await client.GetChangeAsync(changeRequest)).ChangeInfo.Status) == ChangeStatus.PENDING) {
                Console.WriteLine("Change is pending.");
                await Task.Delay(500);
            }

            return currentStatus;
        }
    }
}
