using UnityEngine;
using System.Collections;
using System;

public class NinjaInput : MonoBehaviour {

	public event Action<Vector3, Vector3> SwipeUpdatedEvent = (v1, v2) => {};
	public event Action<Vector2> TapEvent = v => {};
	public event Action<Vector2, float> SwipeDoneEvent = (v,f) => {};
	public event Action<Vector3> MouseUpEvent = (v) => {};

	public Vector2 swipeDir;
	public float swipeSpeed;
	public float swipeDistance;

	public Vector3 downMousePos;

	public float minSwipeTime = 0.2f;
	public float maxSwipeTime = 0.2f;

	bool isMouseDown = false;
	bool didSwipe = false;

	float downTime;

	void UpdateSwipeData ()
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

		var deltaPos = Input.mousePosition - screenPos;

		deltaPos = Input.mousePosition - downMousePos;
		swipeDir = deltaPos.normalized;
		//Unit Vector of change in position
		swipeDistance = deltaPos.magnitude;
		swipeSpeed = deltaPos.magnitude / downTime;
		//distance traveled divided by time elapsed
	}

	void FireSwipeEvent(){
		didSwipe = true;
		SwipeDoneEvent(swipeDir, swipeSpeed);		
	}

	void CheckSwipe (bool allowSwipeFire = false)
	{
		UpdateSwipeData ();

		if (IsSwiping() && allowSwipeFire) {
			FireSwipeEvent ();
		}
	}

	bool IsSwiping(){
		return swipeDistance >= 30;
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)){
			isMouseDown = true;
			downMousePos = Input.mousePosition;
		}

		else if (Input.GetMouseButtonUp(0) && isMouseDown){
			isMouseDown = false;

			if (!IsSwiping()){
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

					CheckSwipe(downTime > maxSwipeTime);

					if (IsSwiping()){
						SwipeUpdatedEvent(downMousePos, Input.mousePosition);
					}
				}
					
			}
		}

//		if (Input.touchCount > 0){
//			if(Input.touches[0].phase == TouchPhase.Moved)//Check if Touch has moved.
//			{
//				swipeDir = Input.touches[0].deltaPosition.normalized;  //Unit Vector of change in position
//				swipeSpeed = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime; //distance traveled divided by time elapsed
//			}
//		}
	}
}
