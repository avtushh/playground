using UnityEngine;
using System.Collections;

public class UpAndDownMovement : MonoBehaviour {

	float maxUp;
	float maxDown;
	Bounds bounds;

	int dir = 1;

	[Range(0,2)]
	public float speed = 0.1f;

	void Start(){
		bounds = GetComponent<BoxCollider2D>().bounds;

		maxUp = transform.localPosition.y + bounds.size.y * 0.8f;
		maxDown = transform.localPosition.y;
	}

	void Update () {
		if (dir > 0){
			if (transform.localPosition.y < maxUp)
				Move ();
			else
				dir = -dir;
		}else{
			if (transform.localPosition.y > maxDown)
				Move ();
			else
				dir = -dir;
		}
	}

	void Move(){
		transform.Translate(speed * Time.deltaTime * dir, 0, 0);
	}
}
