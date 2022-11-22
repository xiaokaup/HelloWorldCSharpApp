using System;
using Amazon.SimpleSystemsManagement.Model;

namespace HelloWorldCSharpApp
{
	public class CustomSendCommandRequest : SendCommandRequest
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public CustomSendCommandRequest()
		{
		}

        /// <summary>
        /// Gets and sets the property Parameters. 
        /// <para>
        /// The required and optional parameters specified in the document being run.
        /// </para>
        /// </summary>
        public new Dictionary<string, object> Parameters
        {
            get { return this._parameters; }
            set { this._parameters = value; }
        }

        // Check to see if Parameters property is set
        internal bool IsSetParameters()
        {
            return this._parameters != null && this._parameters.Count > 0;
        }
    }
}

