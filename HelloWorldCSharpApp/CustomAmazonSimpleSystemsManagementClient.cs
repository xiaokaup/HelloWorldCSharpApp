using System;
using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement.Model.Internal.MarshallTransformations;

namespace HelloWorldCSharpApp
{
	public class CustomAmazonSimpleSystemsManagementClient : AmazonSimpleSystemsManagementClient
    {
        public CustomAmazonSimpleSystemsManagementClient()
        {
        }

        public CustomAmazonSimpleSystemsManagementClient(AWSCredentials credentials, RegionEndpoint region) : base(credentials, region)
        {
        }

        //public virtual Task<SendCommandResponse> CustomeSendCommandAsync(SendCommandRequest request) 
        //{
        //    var options = new InvokeOptions();
        //    options.RequestMarshaller = SendCommandRequestMarshaller.Instance;
        //    options.ResponseUnmarshaller = SendCommandResponseUnmarshaller.Instance;

        //    return InvokeAsync<SendCommandResponse>(request, options, cancellationToken);
        //}
    }
}

