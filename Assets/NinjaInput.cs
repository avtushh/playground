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
	public float minSwipeSpeed = 10;

	bool isMouseDown = false;
	bool didSwipe = false;

	Vector3 mouseDownPos;

	float downTime;

	void UpdateSwipeData ()
	{
		var mouseUpPos = Input.mousePosition;
		var deltaPos = mouseUpPos - mouseDownPos;
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
			mouseDownPos = Input.mousePosition;
		}

		else if (Input.GetMouseButtonUp(0) && isMouseDown){
			isMouseDown = false;
			CheckSwipe (true);

			if (!didSwipe){
				if (downTime < minSwipeTime){
					Debug.LogError("on tap");
					TapEvent(new Vector2(Input.mousePosition.x, Input.mousePosition.y));					
				}else{
					MouseUpEvent(Input.mousePosition);
				}
			}

			downTime = 0;
			didSwipe = false;
		}else{
			if (isMouseDown){
				downTime += Time.deltaTime;
				if (downTime >= minSwipeTime){
					var currPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);					
					SwipeUpdatedEvent(currPos);
				}
				if (!didSwipe)
					CheckSwipe(false);
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
