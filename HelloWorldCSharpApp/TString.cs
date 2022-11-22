using System;
namespace HelloWorldCSharpApp
{
	public class TString
	{
		public TString()
		{
            
        }

        public static string ToDebugString<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}

