using System.Linq.Expressions;
using Logires.Pins;
using Logires.Interfaces;
using Logires.Extensions;

namespace Logires;

public static class PinsConverter
{
	private static Dictionary<int, object> _convertions = new Dictionary<int, object>();

	static PinsConverter()
	{
		AddConvertator<bool, List<bool>>(BooleanToBit);
		AddConvertator<bool, int>(BooleanToInt32);
		AddConvertator<bool, double>(BooleanToDouble);
		
		AddConvertator<List<bool>, bool>(BitToBoolean);
		AddConvertator<List<bool>, int>(BitToInt32);
		AddConvertator<List<bool>, double>(BitToDouble);
		
		AddConvertator<int, bool>(Int32ToBoolean);
		AddConvertator<int, List<bool>>(Int32ToBit);
		AddConvertator<int, double>(Int32ToDouble);
		
		AddConvertator<double, bool>(DoubleToBoolean);
		AddConvertator<double, List<bool>>(DoubleToBit);
		AddConvertator<double, int>(DoubleToInt32);
	}

	public static Func<T1, T2>? GetConvertator<T1, T2>()
	{
		return GetConvertator(typeof(T1), typeof(T2)) as Func<T1, T2>;
	}
	
	public static object GetConvertator(Type t1, Type t2)
	{
		var hc = GetHashCode(t1, t2);

		/*Console.WriteLine("Get");
		Console.WriteLine(hc);*/

		return _convertions[hc];
	}

	public static List<bool> BooleanToBit(bool value)
	{
		return new List<bool>() { value };
	}
	public static int BooleanToInt32(bool value)
	{
		return value ? 1 : 0;
	}
	public static double BooleanToDouble(bool value)
	{
		return value ? 1 : 0;
	}

	public static bool BitToBoolean(List<bool> value)
	{
		return value.Any(x => x);
	}
	public static int BitToInt32(List<bool> value)
	{
		var bits = value.Select(x => x ? "1" : "0").Aggregate((x, y) => x + y).PadLeft(32, '0').Split(8).Select(x => Convert.ToByte(x, 2)).Reverse().ToArray();
		 
		if (bits.Length > 0)
		{
		  return BitConverter.ToInt32(bits, 0);
		}
		 
		return 0;
	}
	public static double BitToDouble(List<bool> value)
	{
		var bits = value.Select(x => x ? "1" : "0").Aggregate((x, y) => x + y).PadLeft(64, '0').Split(8).Select(x => Convert.ToByte(x, 2)).Reverse().ToArray();
		 
		if (bits.Length > 0)
		{
			return BitConverter.ToDouble(bits, 0);
		}
		 
		return 0;
	}

	public static bool Int32ToBoolean(int value)
	{
		return value != 0;
	}
	public static List<bool> Int32ToBit(int value)
	{
		var bits = BitConverter.GetBytes(value).Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).Reverse().Aggregate((x, y) => x + y).Select(x => x != '0');
		 
		return bits.ToList();
	}
	public static double Int32ToDouble(int value)
	{
		return Convert.ToDouble(value);
	}
	
	public static bool DoubleToBoolean(double value)
	{
		return value != 0;
	}
	public static List<bool> DoubleToBit(double value)
	{
		var bits = BitConverter.GetBytes(value).Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).Reverse().Aggregate((x, y) => x + y).Select(x => x != '0');
		 
		return bits.ToList();
	}
	public static int DoubleToInt32(double value)
	{
		return Convert.ToInt32(value);
	}

	private static void AddConvertator<T1, T2>(Func<T1, T2> convertator)
	{
		var hc = GetHashCode<T1, T2>();

		/*Console.WriteLine("Add");
		Console.WriteLine(hc);*/
		
		//_convertions[hc] = Expression.Lambda<Func<object, object>>(convertator.Body, convertator.Parameters[0]);
		_convertions[hc] = convertator;
	}

	private static int GetHashCode<T1, T2>()
	{
		return GetHashCode(typeof(T1), typeof(T2));
	}
	
	private static int GetHashCode(Type t1, Type t2)
	{
		return HashCode.Combine(t1, t2);
	}
}
