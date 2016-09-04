using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Amazon;
using Amazon.Route53;
using Amazon.Route53.Model;

namespace Slight.Route53Manager {
    class Program {
        static void Main(string[] args) {

            const string awsId = "AKIAJBEJGQLMJAGQV6YQ";
            const string awsKey = "lzM+9nJXLTNauQgz15SIyQD1QzWe/4+UcPzOEEPw";

            const string domainName = "www.example.org";

            //[1] Create an Amazon Route 53 client object
            var route53Client = AWSClientFactory.CreateAmazonRoute53Client(awsId, awsKey);

            //[2] Create a hosted zone
            var zoneRequest = new CreateHostedZoneRequest {
                Name = domainName,
                CallerReference = "my_change_request"
            };

            var zoneResponse = route53Client.CreateHostedZone(zoneRequest);

            //[3] Create a resource record set change batch
            var recordSet = new ResourceRecordSet {
                Name = domainName,
                TTL = 60,
                Type = RRType.A,
                ResourceRecords = new List<ResourceRecord> { new ResourceRecord { Value = "192.0.2.235" } }
            };

            var change1 = new Change {
                ResourceRecordSet = recordSet,
                Action = ChangeAction.CREATE
            };

            var changeBatch = new ChangeBatch {
                Changes = new List<Change> { change1 }
            };

            //[4] Update the zone's resource record sets
            var recordsetRequest = new ChangeResourceRecordSetsRequest {
                HostedZoneId = zoneResponse.HostedZone.Id,
                ChangeBatch = changeBatch
            };

            var recordsetResponse = route53Client.ChangeResourceRecordSets(recordsetRequest);

            //[5] Monitor the change status
            var changeRequest = new GetChangeRequest {
                Id = recordsetResponse.ChangeInfo.Id
            };

            while(route53Client.GetChange(changeRequest).ChangeInfo.Status == ChangeStatus.PENDING) {
                Console.WriteLine("Change is pending.");
                Thread.Sleep(15000);
            }

            Console.WriteLine("Change is complete.");
            Console.ReadKey();
        }
    }
}
