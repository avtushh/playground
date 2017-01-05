using System.Collections;
using System;
using System.Collections.Generic;

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

	private static Random rng = new Random();  

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

}
