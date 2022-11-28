using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HelloWorldCSharpApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSController : ControllerBase
    {
        private AmazonEC2Client awsEc2Client;
        private AmazonSimpleSystemsManagementClient awsSSMClient;
        private CustomAmazonSimpleSystemsManagementClient customAwsSSMClient;

        public AWSController()
        {
            string accessKey = "";
            string secretKey = "";
            Amazon.RegionEndpoint region = Amazon.RegionEndpoint.EUWest3;

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            this.awsEc2Client = new AmazonEC2Client(credentials, region);
            this.awsSSMClient = new AmazonSimpleSystemsManagementClient(credentials, region);
            this.customAwsSSMClient = new CustomAmazonSimpleSystemsManagementClient(credentials, region);
        }

        [HttpGet("createInstance", Name = "createInstance")]
        public async Task<List<Instance>> createInstance()
        {
            System.Diagnostics.Debug.WriteLine("Start createInstance...");

            RunInstancesRequest createEc2Request = new RunInstancesRequest {
                ImageId = "ami-02b01316e6e3496d9",
                MinCount = 1,
                MaxCount = 1,
                InstanceType = "t3.nano",
                SecurityGroupIds = new List<string>() { "sg-003eb38f17617c1ce" },
                SubnetId = "subnet-0c8782d18d92c563d"
            };

            RunInstancesResponse response = await this.awsEc2Client.RunInstancesAsync(createEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish createInstance.");

            return response.Reservation.Instances.ToList();
        }

        [HttpGet("describeAllInstances", Name = "describeAllInstances")]
        public async Task<List<Reservation>> describeAllInstances()
        {
            System.Diagnostics.Debug.WriteLine("Start describeAllInstances...");

            DescribeInstancesResponse response = await this.awsEc2Client.DescribeInstancesAsync();

            System.Diagnostics.Debug.WriteLine("Finish describeAllInstances.");

            return response.Reservations.ToList();
        }

        [HttpGet("describeInstances", Name = "describeInstances")]
        public async Task<List<Reservation>> describeInstances()
        {
            System.Diagnostics.Debug.WriteLine("Start describeInstances...");

            DescribeInstancesRequest describeEC2Request = new DescribeInstancesRequest
            {
                Filters = new List<Filter> {
                    new Filter {
                        Name = "instance-type",
                        Values = new List<string> {
                            "t2.micro"
                        }
                    }
                }
            };
            DescribeInstancesResponse response = await this.awsEc2Client.DescribeInstancesAsync(describeEC2Request);

            System.Diagnostics.Debug.WriteLine("Finish describeInstances.");

            return response.Reservations.ToList();
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
            System.Diagnostics.Debug.WriteLine("Start terminateInstance...");

            TerminateInstancesRequest stopEc2Request = new TerminateInstancesRequest
            {
                InstanceIds = new List<string>
                {
                    instanceId,
                }
            };

            TerminateInstancesResponse response = await this.awsEc2Client.TerminateInstancesAsync(stopEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish terminateInstance.");

            return response.TerminatingInstances.ToList();
        }


        [HttpGet("sendCommand1", Name = "sendCommand1")]
        public async Task<Command> sendCommand1(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start sendCommand1...");

            Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>>();
            parameters.Add("commands", new List<string> { "echo helleWorld !" });
            SendCommandRequest sendCommandRequest = new SendCommandRequest
            {
                DocumentName = "AWS-RunShellScript",
                InstanceIds = new List<string>
                {
                    instanceId,
                },
                Parameters = parameters
            };

            SendCommandResponse response = await this.awsSSMClient.SendCommandAsync(sendCommandRequest);

            System.Diagnostics.Debug.WriteLine("Finish sendCommand1.");

            return response.Command;
        }

        [HttpGet("sendCommand2", Name = "sendCommand2")]
        public async Task<Command> sendCommand2(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start sendCommand2...");

            Dictionary<string, string> parametersSourceInfo = new Dictionary<string, string>();
            parametersSourceInfo.Add("path", "https://follow-paris-s3-bucket.s3.eu-west-3.amazonaws.com/first_script.sh");

            Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>>();
            parameters.Add("sourceType", new List<string> { "S3" });
            parameters.Add("sourceInfo", new List<string> { JsonConvert.SerializeObject(parametersSourceInfo) });
            parameters.Add("commandLine", new List<string> { "first_script.sh" });
            parameters.Add("executionTimeout", new List<string> { "3600" });

            SendCommandRequest sendCommandRequest = new SendCommandRequest
            {
                DocumentName = "AWS-RunRemoteScript",
                InstanceIds = new List<string>
                {
                    instanceId,
                },
                Parameters = parameters
            };

            SendCommandResponse response = await this.awsSSMClient.SendCommandAsync(sendCommandRequest);

            System.Diagnostics.Debug.WriteLine("Finish sendCommand2.");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(parametersSourceInfo));
            
            return response.Command;
        }

        [HttpGet("getCommandInvocation", Name = "getCommandInvocation")]
        public async Task<GetCommandInvocationResponse> getCommandInvocation(string commandId, string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start getCommandInvocation...");

            GetCommandInvocationRequest sendCommandRequest = new GetCommandInvocationRequest
            {
                CommandId = commandId,
                InstanceId = instanceId,
            };

            GetCommandInvocationResponse response = await this.awsSSMClient.GetCommandInvocationAsync(sendCommandRequest);

            System.Diagnostics.Debug.WriteLine("Finish getCommandInvocation.");

            return response;
        }


    }
}
