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
using Amazon.S3;
using Amazon.S3.Model;
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
        private AmazonS3Client awsS3Client;


        public AWSController()
        {
            string accessKey = Environment.GetEnvironmentVariable("AWSAccessKey");
            string secretKey = Environment.GetEnvironmentVariable("AWSSecretKey");
            Amazon.RegionEndpoint region = Amazon.RegionEndpoint.EUWest3;

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            this.awsEc2Client = new AmazonEC2Client(credentials, region);
            this.awsSSMClient = new AmazonSimpleSystemsManagementClient(credentials, region);
            this.awsS3Client = new AmazonS3Client(credentials, region);

        }

        [HttpGet("createInstance", Name = "createInstance")]
        public async Task<List<Instance>> createInstance()
        {
            System.Diagnostics.Debug.WriteLine("Start createInstance...");

            IamInstanceProfileSpecification iamInstanceProfile = new IamInstanceProfileSpecification()
            {
                Arn = "arn:aws:iam::567618406205:instance-profile/ecsInstanceRole"
            };

            RunInstancesRequest createEc2Request = new RunInstancesRequest
            {
                ImageId = "ami-02b01316e6e3496d9",
                MinCount = 1,
                MaxCount = 1,
                InstanceType = "t3.nano",
                SecurityGroupIds = new List<string>() { "sg-003eb38f17617c1ce" },
                SubnetId = "subnet-0c8782d18d92c563d",
                IamInstanceProfile = iamInstanceProfile
            };

            RunInstancesResponse response = await this.awsEc2Client.RunInstancesAsync(createEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish createInstance.");

            return response.Reservation.Instances.ToList();
        }

        [HttpGet("startInstance", Name = "startInstance")]
        public async Task<List<InstanceStateChange>> startInstance(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start createInstance...");

            StartInstancesRequest startEc2Request = new StartInstancesRequest
            {
                InstanceIds = new List<string>
                {
                    instanceId,
                }
            };

            StartInstancesResponse response = await this.awsEc2Client.StartInstancesAsync(startEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish createInstance.");

            return response.StartingInstances.ToList();
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
        public async Task<List<Reservation>> describeInstances(string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start describeInstances...");

            DescribeInstancesRequest describeEC2Request = new DescribeInstancesRequest
            {
                // Filters = new List<Filter> {
                //     new Filter {
                //         Name = "instance-type",
                //         Values = new List<string> {
                //             "t2.micro"
                //         }
                //     }
                // },
                InstanceIds = new List<string> { instanceId }
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

        [HttpGet("getScriptFromS3", Name = "getScriptFromS3")]
        public async Task<string> getScriptFromS3()
        {
            System.Diagnostics.Debug.WriteLine("Start getScriptFromS3...");

            GetObjectRequest getObjectRequest = new GetObjectRequest()
            {
                BucketName = "ansys-gateway-development-private",
                Key = "first_script.sh"
            };

            GetObjectResponse getObjectResponse = await this.awsS3Client.GetObjectAsync(getObjectRequest);

            StreamReader reader = new StreamReader(getObjectResponse.ResponseStream);

            string contents = await reader.ReadToEndAsync();

            //DisassociateIamInstanceProfileRequest removeRoleFromEc2Request = new DisassociateIamInstanceProfileRequest
            //{
            //    AssociationId = associationId
            //};

            //DisassociateIamInstanceProfileResponse response = await this.awsEc2Client.DisassociateIamInstanceProfileAsync(removeRoleFromEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish getScriptFromS3.");

            //return response.IamInstanceProfileAssociation;
            return contents;
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
                Parameters = parameters,
                OutputS3BucketName = "ansys-gateway-development",
                OutputS3KeyPrefix = "runCommandsOutput"
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
                Parameters = parameters,
                OutputS3BucketName = "ansys-gateway-development",
                OutputS3KeyPrefix = "runCommandsOutput"
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

        [HttpGet("addRoleToEC2", Name = "addRoleToEC2")]
        public async Task<IamInstanceProfileAssociation> addRoleToEC2(string iamInstanceProfileArn, string iamInstanceProfileName, string instanceId)
        {
            System.Diagnostics.Debug.WriteLine("Start addRoleToEC2...");

            IamInstanceProfileSpecification iamInstanceProfile = new IamInstanceProfileSpecification()
            {
                Arn = iamInstanceProfileArn,
                Name = iamInstanceProfileName
            };

            AssociateIamInstanceProfileRequest addRoleToEc2Request = new AssociateIamInstanceProfileRequest
            {
                IamInstanceProfile = iamInstanceProfile,
                InstanceId = instanceId
            };

            AssociateIamInstanceProfileResponse response = await this.awsEc2Client.AssociateIamInstanceProfileAsync(addRoleToEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish addRoleToEC2.");

            return response.IamInstanceProfileAssociation;
        }
        [HttpGet("DescribeAssociation", Name = "DescribeAssociation")]
        public async Task<List<IamInstanceProfileAssociation>> DescribeAssociation(string associationId)
        {
            System.Diagnostics.Debug.WriteLine("Start DescribeAssociation...");

            DescribeIamInstanceProfileAssociationsRequest DescribeAssociationRequest = new DescribeIamInstanceProfileAssociationsRequest
            {
                AssociationIds = new List<string> { associationId }
            };

            DescribeIamInstanceProfileAssociationsResponse response = await this.awsEc2Client.DescribeIamInstanceProfileAssociationsAsync(DescribeAssociationRequest);

            System.Diagnostics.Debug.WriteLine("Finish DescribeAssociation.");

            return response.IamInstanceProfileAssociations.ToList();
        }

        [HttpGet("removeRoleFromEC2", Name = "removeRoleFromEC2")]
        public async Task<IamInstanceProfileAssociation> removeRoleFromEC2(string associationId)
        {
            System.Diagnostics.Debug.WriteLine("Start removeRoleFromEC2...");

            DisassociateIamInstanceProfileRequest removeRoleFromEc2Request = new DisassociateIamInstanceProfileRequest
            {
                AssociationId = associationId
            };

            DisassociateIamInstanceProfileResponse response = await this.awsEc2Client.DisassociateIamInstanceProfileAsync(removeRoleFromEc2Request);

            System.Diagnostics.Debug.WriteLine("Finish removeRoleFromEC2.");

            return response.IamInstanceProfileAssociation;
        }
    }
}
