using UnityEngine;
using System.Collections;
using System;

public class DetectPlatformEdge : MonoBehaviour {


	public event Action OnCollideWithEdge = () => {};

	void OnTriggerExit2D(Collider2D other) {

		if (other.tag == "Edge"){
			OnCollideWithEdge();

		}
	}
}
