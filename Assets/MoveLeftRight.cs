using UnityEngine;
using System.Collections;

public class MoveLeftRight : MonoBehaviour {

	public float left = -3f;
	public float right = 3f;

	public float speed = 1f;



	// Update is called once per frame
	void Update () {
		if (speed > 0){
			if (transform.position.x < right){
				var pos = transform.position;
				pos.x += speed;
				transform.position = pos;
			}else{
				speed *= -1;
			}

		}else{
			if (transform.position.x > left){
				var pos = transform.position;
				pos.x += speed;
				transform.position = pos;
			}else{
				speed *= -1;
			}
		}

	}
}
