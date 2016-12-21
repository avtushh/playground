using UnityEngine;
using System.Collections;
using System;

public class NinjaInput : MonoBehaviour {

	public event Action OnMouseDown = () => {};
	public event Action<Vector2> OnTap = v => {};
	public event Action<Vector2, float> OnSwipe = (v,f) => {};

	public Vector2 swipeDir;
	public float swipeSpeed;

	bool isMouseDown = false;

	Vector3 mouseDownPos;

	float downTime;

	void Update () {

		Vector2 direction;
		float speed;

		if (Input.GetMouseButtonDown(0)){
			isMouseDown = true;
			mouseDownPos = Input.mousePosition;
			OnMouseDown();
		}

		else if (Input.GetMouseButtonUp(0)){
			
			isMouseDown = false;

			var mouseUpPos = Input.mousePosition;

			var deltaPos = mouseUpPos - mouseDownPos;

			direction = deltaPos.normalized;  //Unit Vector of change in position
			speed = deltaPos.magnitude / downTime; //distance traveled divided by time elapsed

			swipeDir = direction;
			swipeSpeed = speed;

			if (swipeSpeed < 10)
				OnTap(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			else
				OnSwipe(swipeDir, swipeSpeed);
		}else{
			if (isMouseDown)
				downTime += Time.deltaTime;
		}

		if (Input.touchCount > 0){
			if(Input.touches[0].phase == TouchPhase.Moved)//Check if Touch has moved.
			{
				direction = Input.touches[0].deltaPosition.normalized;  //Unit Vector of change in position
				speed = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime; //distance traveled divided by time elapsed
			}
		}
	}
}
