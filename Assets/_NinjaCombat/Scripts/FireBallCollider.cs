using UnityEngine;
using System.Collections;

public class FireBallCollider : MonoBehaviour {

	NinjaStar _parentStar;

	void Start(){
		_parentStar = GetComponentInParent<NinjaStar>();
	}

	void OnTriggerEnter2D(Collider2D other){

		switch(other.gameObject.tag){
			case "Obstacle":
				Destroy(other.gameObject);
				break;
			case "Bullet":
				var ninjaStar = other.gameObject.GetComponent<NinjaStar>();
				if (ninjaStar.IsActive && _parentStar.IsActive)
					ninjaStar.Hit();
				break;
		}

	}
}
