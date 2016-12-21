using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(other.gameObject);
	}
}
