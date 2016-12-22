using UnityEngine;
using System.Collections;
using System;

public class NinjaInput : MonoBehaviour {

	public event Action<Vector2> SwipeUpdatedEvent = (v1) => {};
	public event Action<Vector2> TapEvent = v => {};
	public event Action<Vector2, float> SwipeDoneEvent = (v,f) => {};
	public event Action<Vector3> MouseUpEvent = (v) => {};

	public Vector2 swipeDir;
	public float swipeSpeed;


	public float minSwipeTime = 0.2f;
	public float maxSwipeTime = 1f;

	bool isMouseDown = false;
	bool didSwipe = false;

	float downTime;

	void UpdateSwipeData ()
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

		var deltaPos = Input.mousePosition - screenPos;
		swipeDir = deltaPos.normalized;
		//Unit Vector of change in position
		swipeSpeed = deltaPos.magnitude / downTime;
		//distance traveled divided by time elapsed
	}

	void FireSwipeEvent(){
		didSwipe = true;
		SwipeDoneEvent(swipeDir, swipeSpeed);		
	}

	void CheckSwipe (bool allowSwipeFire = false)
	{
		if (downTime >= minSwipeTime) {
			UpdateSwipeData ();

			if (allowSwipeFire) {
				FireSwipeEvent ();
			}
		}
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)){
			isMouseDown = true;
		}

		else if (Input.GetMouseButtonUp(0) && isMouseDown){
			isMouseDown = false;

			if (downTime < minSwipeTime){
				TapEvent(new Vector2(Input.mousePosition.x, Input.mousePosition.y));					
			}else{
				if (!didSwipe)
					CheckSwipe (true);
				if (!didSwipe){
					MouseUpEvent(Input.mousePosition);
				}
			}	

			downTime = 0;
			didSwipe = false;
		}else{
			if (isMouseDown){
				downTime += Time.deltaTime;

				if (!didSwipe){
					
					if (downTime >= minSwipeTime){
						var currPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);					
						SwipeUpdatedEvent(currPos);
					}

					CheckSwipe(downTime > maxSwipeTime);
				}
					
			}
		}

		if (Input.touchCount > 0){
			if(Input.touches[0].phase == TouchPhase.Moved)//Check if Touch has moved.
			{
				swipeDir = Input.touches[0].deltaPosition.normalized;  //Unit Vector of change in position
				swipeSpeed = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime; //distance traveled divided by time elapsed
			}
		}
	}
}
