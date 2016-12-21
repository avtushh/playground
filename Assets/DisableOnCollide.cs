using UnityEngine;
using System.Collections;

public class DisableOnCollide : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		other.gameObject.SetActive(false);
	}
}
