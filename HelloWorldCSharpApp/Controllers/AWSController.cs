using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.EC2;
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
        public void runInstance()
        {
            System.Diagnostics.Debug.WriteLine("runInstance...");
        }

        [HttpGet("stopInstance", Name = "stopInstance")]
        public void stopInstance()
        {
            System.Diagnostics.Debug.WriteLine("stopInstance...");
        }
    }
}
