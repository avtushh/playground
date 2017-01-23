using UnityEngine;
using System.Collections;
using System;

public class FieldOfView : MonoBehaviour {

	public static event Action<bool, GameObject> OnCollision = (g,b) => {};
	public static event Action<GameObject> OnCollisionStay = (g) => {};


	void OnTriggerEnter2D (Collider2D other)
	{
		OnCollision(true, other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other){
		OnCollisionStay(other.gameObject);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		OnCollision(false, other.gameObject);
	}
}
