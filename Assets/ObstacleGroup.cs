using UnityEngine;
using System.Collections;

public class ObstacleGroup : MonoBehaviour {

	public float speed = 0.5f;

	public float right, left;

	public bool active = false;

	void Start(){
		speed *= 0.5f;
	}

	void Update(){



		transform.Translate(speed * Time.deltaTime, 0, 0);

		if (transform.position.x >= right && speed > 0){
			speed *= -1;
		}else if (transform.position.x < left && speed < 0){
			speed *= -1;
		}
	}

}
