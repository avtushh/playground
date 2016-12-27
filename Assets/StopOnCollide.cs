using UnityEngine;
using System.Collections;

public class StopOnCollide : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Bullet"){
			other.GetComponent<NinjaStar>().HitGround();
		}

	}
}
