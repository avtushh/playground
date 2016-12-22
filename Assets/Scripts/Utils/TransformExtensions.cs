using UnityEngine;
using System.Collections;

public static class TransformExtensions {

	public static Transform RemoveAllChildren(this Transform transform)
	{
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		return transform;
	}
	
}
