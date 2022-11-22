using System;
using System.Collections.Generic;
using System.Linq;
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

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            this.awsEc2Client = new AmazonEC2Client(credentials, Amazon.RegionEndpoint.EUWest3);
        }


        [HttpGet("runInstance", Name = "runInstance")]
        public async Task<string> runInstance()
        {
            System.Diagnostics.Debug.WriteLine("Start runInstance...");

            RunInstancesRequest startEc2Request = new RunInstancesRequest {
                ImageId = "ami-02b01316e6e3496d9",
                MinCount = 1,
                MaxCount = 1,
                InstanceType = "t3.nano",
                SecurityGroupIds = new List<string>() { "sg-903004f8" },
                SubnetId = "subnet-6e7f829e"
            };

            RunInstancesResponse response = await this.awsEc2Client.RunInstancesAsync(startEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish runInstance.");

            return response.HttpStatusCode;
        }

        [HttpGet("stopInstance", Name = "stopInstance")]
        public void stopInstance()
        {
            System.Diagnostics.Debug.WriteLine("Start stopInstance...");
            System.Diagnostics.Debug.WriteLine("Finish stopInstance.");
        }
    }
}
