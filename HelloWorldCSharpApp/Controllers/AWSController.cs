using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldCSharpApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSController : ControllerBase
    {
        private AmazonEC2Client awsEc2Client;

        public AWSController()
        {
            string accessKey = "";
            string secretKey = "";
            Amazon.RegionEndpoint region = Amazon.RegionEndpoint.EUWest3;

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            this.awsEc2Client = new AmazonEC2Client(credentials, region);
        }


        [HttpGet("runInstance", Name = "runInstance")]
        public async Task<List<Instance>> runInstance()
        {
            System.Diagnostics.Debug.WriteLine("Start runInstance...");

            RunInstancesRequest startEc2Request = new RunInstancesRequest {
                ImageId = "ami-02b01316e6e3496d9",
                MinCount = 1,
                MaxCount = 1,
                InstanceType = "t3.nano",
                SecurityGroupIds = new List<string>() { "sg-003eb38f17617c1ce" },
                SubnetId = "subnet-0c8782d18d92c563d"
            };

            RunInstancesResponse response = await this.awsEc2Client.RunInstancesAsync(startEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish runInstance.");

            return response.Reservation.Instances.ToList();
        }

        [HttpGet("stopInstance", Name = "stopInstance")]
        public async Task<List<InstanceStateChange>> stopInstance(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start stopInstance...");

            StopInstancesRequest stopEc2Request = new StopInstancesRequest
            {
                InstanceIds = new List<string>
                {
                    instanceId,
                }
            };

            StopInstancesResponse response = await this.awsEc2Client.StopInstancesAsync(stopEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish stopInstance.");

            return response.StoppingInstances.ToList();
        }

        [HttpGet("terminateInstance", Name = "terminateInstance")]
        public async Task<List<InstanceStateChange>> terminateInstance(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start stopInstance...");

            TerminateInstancesRequest stopEc2Request = new TerminateInstancesRequest
            {
                InstanceIds = new List<string>
                {
                    instanceId,
                }
            };

            TerminateInstancesResponse response = await this.awsEc2Client.TerminateInstancesAsync(stopEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish stopInstance.");

            return response.TerminatingInstances.ToList();
        }
    }
}
