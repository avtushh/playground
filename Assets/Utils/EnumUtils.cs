using System.Collections;
using System;

public static class EnumUtils {

	public static T RandomEnumValue<T> (bool excludeFirstValue = false)
	{
		var v = Enum.GetValues (typeof (T));

		int minIndex = excludeFirstValue?1:0;

		return (T) v.GetValue (new Random ().Next(minIndex, v.Length));
	}

	public static Array EnumValues<T> ()
	{
		return Enum.GetValues (typeof (T));
	}

	public static T Next<T>(this T src, bool skipFirstIndex = false) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1;
		return (Arr.Length==j) ? Arr[skipFirstIndex?1:0] : Arr[j];            
	}

}
