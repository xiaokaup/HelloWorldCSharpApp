using System;
namespace HelloOrleansApp
{
	public class HelloGrain : Grain, IHelloGrain
	{
		public ValueTask<string> SayHello(string greeting)
		{
			return ValueTask.FromResult($"Hello, {greeting}");
		}
	}
}

