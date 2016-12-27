using UnityEngine;
using System.Collections;

public class FireBallCollider : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		switch(other.gameObject.tag){
			case "Obstacle":
				other.gameObject.SetActive(false);
				break;
			case "Bullet":
				//var otherStar = other.gameObject.GetComponent<NinjaStar>();


				break;
		}
	}
}
