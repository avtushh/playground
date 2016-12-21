using UnityEngine;
using System.Collections;
using System;

public class NinjaInput : MonoBehaviour {

	public event Action OnMouseDown = () => {};
	public event Action<Vector2> OnTap = v => {};
	public event Action<Vector2, float> OnSwipe = (v,f) => {};

	public Vector2 swipeDir;
	public float swipeSpeed;

	public float minSwipeTime = 0.3f;
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
		OnSwipe(swipeDir, swipeSpeed);		
	}

	void CheckSwipe ()
	{
		if (downTime > minSwipeTime) {
			UpdateSwipeData ();
			if (swipeSpeed > minSwipeSpeed) {
				FireSwipeEvent ();
			}
		}
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)){
			isMouseDown = true;
			mouseDownPos = Input.mousePosition;
			//OnMouseDown();
		}

		else if (Input.GetMouseButtonUp(0) && isMouseDown){
			isMouseDown = false;
			if (!didSwipe){
				CheckSwipe ();
			}

			if (!didSwipe){
				Debug.LogError("on tap");
				OnTap(new Vector2(Input.mousePosition.x, Input.mousePosition.y));					
			}

			didSwipe = false;
		}else{
			if (isMouseDown && !didSwipe){
				downTime += Time.deltaTime;
				CheckSwipe();
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
