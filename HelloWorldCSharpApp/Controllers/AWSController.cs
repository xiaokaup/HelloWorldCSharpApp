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
        private AWSCredentials credentials;
        private AmazonEC2Client awsEc2Client;

        public AWSController(AWSCredentials credentials, AmazonEC2Client awsEc2Client)
        {
            string accessKey = "";
            string secretKey = "";
            
            this.credentials = new BasicAWSCredentials(accessKey, secretKey);
            this.awsEc2Client = new AmazonEC2Client(credentials, Amazon.RegionEndpoint.EUWest3);
        }
    }
}
