using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.Route53;

namespace Slight.Route53Manager.Models {
    public class Credentials {

        public string AccessKey {
            get;
            set;
        }

        public string SecretAccessKey {
            get;
            set;
        }

        public IAmazonRoute53 CreateClient() {

            return AWSClientFactory.CreateAmazonRoute53Client(AccessKey, SecretAccessKey);
        }
    }
}
