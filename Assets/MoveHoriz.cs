using UnityEngine;
using System.Collections;

public class MoveHoriz : MonoBehaviour {

	public float speed = 1;
	public int direction = 1;

	public Transform leftBound, rightBound;

	float maxX, minX;

	void Start () {
		minX = leftBound.position.x;
		maxX = rightBound.position.x;
	}
	
	void Update () {
		MoveBy(speed * direction * Time.deltaTime);
	}

	public void MoveBy(float x){
		var pos = transform.position;
		pos.x += x;

		if (pos.x >= maxX){
			pos.x = maxX;
			SwitchDirection();
		}else if (pos.x <= minX){
			pos.x = minX;
			SwitchDirection();
		}

		transform.position = pos;
	}

	public void SwitchDirection(){
		direction *= -1;
	}


}