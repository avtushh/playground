using System.Collections;
using System;

public static class EnumUtils {

	public static T RandomEnumValue<T> (bool excludeFirstValue = false)
	{
		var v = Enum.GetValues (typeof (T));

		int minIndex = excludeFirstValue?1:0;

		return (T) v.GetValue (new Random ().Next(minIndex, v.Length));
	}

}
