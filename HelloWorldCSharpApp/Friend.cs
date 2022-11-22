using System;
namespace HelloWorldCSharpApp
{
	public class Friend
    {
		public string firstName;
		public string lastName;
		public string location;

		public Friend()
		{
		}

        public Friend(string firstName, string lastName, string location)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.location = location;
        }
    }
}

