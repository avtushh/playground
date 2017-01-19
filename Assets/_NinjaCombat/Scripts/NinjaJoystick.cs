using UnityEngine;
using System.Collections;
using System;

public class NinjaJoystick : MonoBehaviour {

	public event Action <Vector3> JoystickMoveEvent = v3 => {};
	public event Action <Vector3, Vector3> SwipeUpdateEvent = (startPos,endPos) => {};
	public event Action<Vector2> SwipeDoneEvent = (v) => {};
	public event Action SwipeCanceledEvent = () => {};

	public float swipeMinDistance = 50;

	public float minSwipeAngle;

	Vector3 _startSwipePos;

	Vector3 _lastTouchPosition;

	bool _isSwiping = false;

	DateTime _swipeStartTime;

	bool _isTouching;

	Vector2 swipeDir;
	float swipeDistance;

	public float maxAimingTime = 0.5f;

	void Update(){

		#if UNITY_EDITOR
		UpdateMouseInput ();

		#else

		UpdateTouchInput();

		#endif
	}

	void UpdateMouseInput ()
	{
		if (Input.GetMouseButtonDown (0)) {
			OnTouchDown (Input.mousePosition);

			_isTouching = true;
		}
		else
			if (Input.GetMouseButtonUp (0)) {
				OnTouchUp (Input.mousePosition);
				_isTouching = false;
			}
			else
				if (_isTouching) {
					OnTouchMove (Input.mousePosition);
				}
	}

	void UpdateTouchInput(){
		if (Input.touches.Length == 1){
			var touch = Input.GetTouch(0);
			switch(touch.phase){
				case TouchPhase.Began:
					_isTouching = true;
					OnTouchDown(new Vector3(touch.position.x, touch.position.y, 0));
					break;
				case TouchPhase.Moved:

					OnTouchMove(new Vector3(touch.position.x, touch.position.y, 0));
					break;
				case TouchPhase.Ended:
					_isTouching = false;
					OnTouchUp(new Vector3(touch.position.x, touch.position.y, 0));
					break;
			}
		}
	}


	void OnTouchUp (Vector3 currScreenPos)
	{
		if (_isSwiping){
			CheckSwipe(_startSwipePos, currScreenPos);
			SwipeDoneEvent(swipeDir);
			_isSwiping = false;
		}
	}

	void OnTouchMove (Vector3 currScreenPos)
	{
		var currWorldPos = Camera.main.ScreenToWorldPoint(currScreenPos);
	
		if (!_isSwiping){
			_isSwiping = CheckSwipe(_lastTouchPosition, currScreenPos);

			if (_isSwiping){
				_swipeStartTime = DateTime.Now;
				_startSwipePos = currScreenPos;
				_lastTouchPosition = currScreenPos;
				return;
			}
		}

		if (!_isSwiping){
			JoystickMoveEvent(currWorldPos);
			_lastTouchPosition = currScreenPos;
		}else{
			TimeSpan span = DateTime.Now - _swipeStartTime;

			if (span.TotalSeconds >= maxAimingTime || swipeDir.y < minSwipeAngle){
				
				_startSwipePos = currScreenPos;
				SwipeDoneEvent(swipeDir);
				_isSwiping = false;

			}else{
				CheckSwipe(_startSwipePos, currScreenPos);
				var downWorldPos = Camera.main.ScreenToWorldPoint(_startSwipePos);
				SwipeUpdateEvent(downWorldPos, currWorldPos);
			}
		}

		_lastTouchPosition = currScreenPos;
	}

	void OnTouchDown (Vector3 downPos)
	{
		_startSwipePos = downPos;
		_swipeStartTime = DateTime.Now;
		_lastTouchPosition = downPos;
	}

	bool CheckSwipe(Vector3 downPos, Vector3 currPos){
		var deltaPos = currPos - downPos;

		swipeDir = deltaPos.normalized;

		//Unit Vector of change in position
		swipeDistance = deltaPos.magnitude;

		DebugText.SetText(swipeDir.ToString());

		return swipeDistance >= swipeMinDistance && swipeDir.y > minSwipeAngle && currPos.y - swipeMinDistance > downPos.y;
		//var swipeSpeed = deltaPos.magnitude / downTime;
	}
}
