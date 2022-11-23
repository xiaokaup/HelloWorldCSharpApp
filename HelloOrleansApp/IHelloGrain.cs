using System;
namespace HelloOrleansApp
{
	public interface IHelloGrain : IGrainWithStringKey
	{
		ValueTask<string> SayHello(string greeting);
	}
}

