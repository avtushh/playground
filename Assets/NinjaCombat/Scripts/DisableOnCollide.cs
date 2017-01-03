using UnityEngine;
using System.Collections;

public class DisableOnCollide : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {

		if (other.CompareTag("Bullet")){
			other.gameObject.GetComponent<NinjaStar>().Hit();
		}
	}
}
