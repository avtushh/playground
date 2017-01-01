using UnityEngine;
using System.Collections;

public class FireBallCollider : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		switch(other.gameObject.tag){
			case "Obstacle":
				if (other.gameObject.GetComponent<ObstacleUnit>() != null){
					other.gameObject.GetComponent<ObstacleUnit>().CriticleHit();
				}

				//other.gameObject.SetActive(false);
				break;
			case "Bullet":
				//var otherStar = other.gameObject.GetComponent<NinjaStar>();


				break;
		}
	}
}
