using System;

namespace OrleansBasicApp
{
	public interface IHello : Orleans.IGrainWithIntegerKey
	{
		Task<string> SayHello(string greeting);
	}
}

