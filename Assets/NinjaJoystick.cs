using UnityEngine;
using System.Collections;
using System;

public class NinjaJoystick : MonoBehaviour {

	public event Action <Vector3> JoystickMoveEvent = v3 => {};
	public event Action <Vector3, Vector3> SwipeUpdateEvent = (startPos,endPos) => {};
	public event Action<Vector2> SwipeDoneEvent = (v) => {};
	public event Action SwipeCanceledEvent = () => {};


	public NinjaInput inputManager;

	public Bounds bottomRect;

	Vector3 _startSwipePos;

	bool _isSwiping = false;

	public float swipeMinDistance = 50;

	public float minSwipeAngle;

	DateTime _swipeStartTime;

	bool _isMouseDown;

	void Start(){
		AddListeners();
	}

	void AddListeners(){
		

	}

	void Update(){

		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)){
			InputManager_MouseDownEvent(Input.mousePosition);
			_isMouseDown = true;
		}else if (Input.GetMouseButtonUp(0)){
			InputManager_MouseUpEvent(Input.mousePosition);
			_isMouseDown = false;
		}else if (_isMouseDown){
			InputManager_MouseMoveEvent(Input.mousePosition);
		}

		#else

		if (Input.touches.Length == 1){
			var touch = Input.GetTouch(0);
			switch(touch.phase){
				case TouchPhase.Began:
					InputManager_MouseDownEvent(new Vector3(touch.position.x, touch.position.y, 0));
					break;
				case TouchPhase.Moved:
					InputManager_MouseMoveEvent(new Vector3(touch.position.x, touch.position.y, 0));
					break;
				case TouchPhase.Ended:
					InputManager_MouseUpEvent(new Vector3(touch.position.x, touch.position.y, 0));
					break;
			}
		}

		#endif
	}



	void InputManager_MouseUpEvent (Vector3 currScreenPos)
	{
		if (_isSwiping){
			_isSwiping = CheckSwipe(_startSwipePos, currScreenPos);
			SwipeDoneEvent(swipeDir);
		}
	}

	Vector2 swipeDir;

	float swipeDistance;

	bool CheckSwipe(Vector3 downPos, Vector3 currPos){
		var deltaPos = currPos - downPos;
		swipeDir = deltaPos.normalized;

		//Unit Vector of change in position
		swipeDistance = deltaPos.magnitude;

		if (swipeDistance >= swipeMinDistance && swipeDir.y > minSwipeAngle){
			return true;
		}else{
			return false;
		}

		//var swipeSpeed = deltaPos.magnitude / downTime;
	}

	void InputManager_MouseMoveEvent (Vector3 currScreenPos)
	{
		var currWorldPos = Camera.main.ScreenToWorldPoint(currScreenPos);
	
		if (!_isSwiping){
			_isSwiping = CheckSwipe(_startSwipePos, currScreenPos);

			if (_isSwiping){
				_swipeStartTime = DateTime.Now;
				_startSwipePos = currScreenPos;
				return;
			}
		}

		if (!_isSwiping){
			JoystickMoveEvent(currWorldPos);
			_startSwipePos = currScreenPos;
		}else{
			TimeSpan span = DateTime.Now - _swipeStartTime;

			if (span.TotalSeconds > maxAimingTime){
				
				_startSwipePos = currScreenPos;
				SwipeDoneEvent(swipeDir);
				//SwipeCanceledEvent();
				_isSwiping = false;
			}else{
				CheckSwipe(_startSwipePos, currScreenPos);
				var downWorldPos = Camera.main.ScreenToWorldPoint(_startSwipePos);
				SwipeUpdateEvent(downWorldPos, currWorldPos);
			}
		}
	}

	public float maxAimingTime = 0.5f;

	void InputManager_MouseDownEvent (Vector3 downPos)
	{
		
		_isMouseDown = true;
		_startSwipePos = downPos;
	}

	bool InJoystickArea(Vector3 pos, bool screenPoint = false){

		if (screenPoint)
			pos = Camera.main.ScreenToWorldPoint(pos);

		return bottomRect.Contains(pos);
	}


}
