namespace Logires.Extensions;

public static class StringExtensions
{
	public static IEnumerable<string> Split(this string value, int length)
	{
	  if (value.Length < length)
	  {
	  	return Enumerable.Repeat(value, 1);
	  }
	  
	  return Enumerable.Range(0, value.Length / length).Select(x => value.Substring(x * length, length));
  }
}
