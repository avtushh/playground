﻿using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {


	void OnTriggerExit2D(Collider2D other)
	{
		Destroy(other.gameObject);
	}
}