using UnityEngine;
using System.Collections;

public class PlayerNinjaController : NinjaController
{
	
	public NinjaInput touchInput;
	public LineRenderer swipeLine;

	public NinjaJoystick joystickInput;

	bool _isSwiping = false;

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		//touchInput.TapEvent -= TouchInput_OnTap;
		//touchInput.SwipeDoneEvent -= TouchInput_OnSwipeRelease;
		//touchInput.SwipeUpdatedEvent -= TouchInput_OnSwiping;
		//touchInput.MouseUpEvent -= TouchInput_OnMouseUp;

		joystickInput.JoystickMoveEvent -= JoystickInput_JoystickMoveEvent;
		joystickInput.SwipeUpdateEvent -= JoystickInput_SwipeUpdateEvent;
		joystickInput.SwipeCanceledEvent -= JoystickInput_SwipeCanceledEvent;
		joystickInput.SwipeDoneEvent -= JoystickInput_SwipeDoneEvent;
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		//touchInput.TapEvent += TouchInput_OnTap;
		//touchInput.SwipeDoneEvent += TouchInput_OnSwipeRelease;
		//touchInput.SwipeUpdatedEvent += TouchInput_OnSwiping;
		//touchInput.MouseUpEvent += TouchInput_OnMouseUp;

		joystickInput.JoystickMoveEvent += JoystickInput_JoystickMoveEvent;
		joystickInput.SwipeUpdateEvent += JoystickInput_SwipeUpdateEvent;
		joystickInput.SwipeCanceledEvent += JoystickInput_SwipeCanceledEvent;
		joystickInput.SwipeDoneEvent += JoystickInput_SwipeDoneEvent;

	}

	void JoystickInput_SwipeDoneEvent (Vector2 normalizedSwipeDir)
	{
		if (isPaused || isThrowing || isHit)
			return;

		if (!canThrow ()) {
			FindObjectOfType<NinjaGameManager>().ShowErrorFrame();
			return;
		}

		_isSwiping = false;

		swipeLine.enabled = false;

		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		isThrowing = true;

		ThrowStar (normalizedSwipeDir, throwSpeed);
		EndThrowAnimation ();
		LeanTween.delayedCall (gameObject, 0.2f, EndThrow);
		_isMoving = false;
	}

	void JoystickInput_SwipeCanceledEvent ()
	{
		EndThrowAnimation ();
		_isSwiping = false;

		swipeLine.enabled = false;
		_isMoving = false;
	}

	void JoystickInput_SwipeUpdateEvent (Vector3 startPos, Vector3 endPos)
	{
		if (isPaused || isThrowing || isHit)
			return;

		_isMoving = false;

		if (!canThrow ()) {
			return;
		}

		UpdateSwipe(startPos, endPos);
	}

	bool _isMoving = false;
	float targetMoveX;
	public float moveSpeed = 10f;

	void JoystickInput_JoystickMoveEvent (Vector3 pos)
	{
		if (isPaused || isThrowing || isHit)
			return;

		targetMoveX = Mathf.Clamp(pos.x, moveHoriz.leftBound.localPosition.x, moveHoriz.rightBound.localPosition.x);

		_isMoving = true;
		//transform.SetPositionX(pos.x);
	}

	void Update(){
		if (_isMoving){
			if (transform.position.x != targetMoveX){
				transform.position = Vector3.Lerp(transform.position, new Vector3(targetMoveX, transform.position.y, transform.position.z), Time.deltaTime * moveSpeed);

			}else{
				_isMoving = false;
			}

		}
	}

	public override void Pause ()
	{
		base.Pause ();
		swipeLine.enabled = false;

	}

	public override void Resume ()
	{
		base.Resume ();
		isHit = false;
		isThrowing = false;
		_isSwiping = false;
	}

	void TouchInput_OnTap (Vector2 obj)
	{
		if (isPaused || isHit)
			return;
		
		swipeLine.enabled = false;
		moveHoriz.SwitchDirection ();
		//moveHoriz.Pause();
	}

	void TouchInput_OnMouseUp (Vector3 startPos, Vector3 endPos)
	{
		if (isPaused || isHit)
			return;
		
		moveHoriz.Resume ();
		swipeLine.enabled = false;
	}


	void TouchInput_OnSwiping (Vector3 downMousePos, Vector3 mousePos)
	{
		if (isPaused || isThrowing || isHit)
			return;

		if (!canThrow ()) {
			return;
		}
													
		var endPoint = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y));
		var startPoint = Camera.main.ScreenToWorldPoint (new Vector3 (downMousePos.x, downMousePos.y));

		UpdateSwipe (startPoint, endPoint);
	}

	void UpdateSwipe (Vector3 startPoint, Vector3 endPoint)
	{
		var starPosition = starHolder.position;
		var delta = startPoint - starPosition;
		endPoint = endPoint - delta;
		endPoint.z = starPosition.z;

		if (GameSettings.ShowAim)
			ShowDirectionLine (starPosition, endPoint);
		moveHoriz.Pause ();
		if (endPoint.x < transform.position.x) {
			moveHoriz.TurnLeft ();
		}
		else {
			moveHoriz.TurnRight ();
		}
		if (!_isSwiping) {
			_isSwiping = true;
			StartThrowAnimation ();
		}
	}

	void TouchInput_OnSwipeRelease (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		if (isPaused || isThrowing || isHit)
			return;

		if (!canThrow ()) {
			return;
		}

		_isSwiping = false;
		
		swipeLine.enabled = false;

		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		isThrowing = true;

		ThrowStar (normalizedSwipeDir, throwSpeed);
		EndThrowAnimation ();
		LeanTween.delayedCall (gameObject, 0.2f, EndThrow);
	}

	void EndThrow ()
	{
		
		isThrowing = false;
		ResumeMove ();
	}

	void ShowDirectionLine (Vector3 startpoint, Vector3 endPoint)
	{
		swipeLine.enabled = true;
		swipeLine.SetVertexCount (2);
		swipeLine.SetWidth (0.2f, 0.2f);
		//swipeLine.SetColors (Color.yellow, Color.yellow);
		swipeLine.SetPosition (0, new Vector3 (startpoint.x, startpoint.y, transform.position.z + 1));
		swipeLine.SetPosition (1, new Vector3 (endPoint.x, endPoint.y, transform.position.z + 1));
	}
}

