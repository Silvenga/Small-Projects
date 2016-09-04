using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.Route53;
using Amazon.Route53.Model;

using Slight.Route53Manager.Models;

namespace Slight.Route53Manager.Actor {
    public static class Record {

        public static async Task Create(this HostedZone zone, Credentials credentials) {

            var client = credentials.CreateClient();

            //[3] Create a resource record set change batch
            var recordSet = new ResourceRecordSet {
                Name = zone.Name,
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
                HostedZoneId = zone.Id,
                ChangeBatch = changeBatch
            };

            var recordsetResponse = client.ChangeResourceRecordSets(recordsetRequest);

            var status = await recordsetResponse.ChangeInfo.WaitForStatusAync(credentials);
        }
    }
}
