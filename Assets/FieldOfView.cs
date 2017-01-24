using UnityEngine;
using System.Collections;
using System;

public class FieldOfView : MonoBehaviour {

	public event Action<bool, GameObject> OnCollision = (g,b) => {};

	void OnTriggerEnter2D (Collider2D other)
	{
		OnCollision(true, other.gameObject);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		OnCollision(false, other.gameObject);
	}
}
