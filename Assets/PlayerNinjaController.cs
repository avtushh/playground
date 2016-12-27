using UnityEngine;
using System.Collections;

public class PlayerNinjaController : NinjaController
{
	
	public NinjaInput touchInput;
	public LineRenderer swipeLine;

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		touchInput.TapEvent -= TouchInput_OnTap;
		touchInput.SwipeDoneEvent -= TouchInput_OnSwipeRelease;
		touchInput.SwipeUpdatedEvent -= TouchInput_OnSwiping;
		touchInput.MouseUpEvent -= TouchInput_OnMouseUp;
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		touchInput.TapEvent += TouchInput_OnTap;
		touchInput.SwipeDoneEvent += TouchInput_OnSwipeRelease;
		touchInput.SwipeUpdatedEvent += TouchInput_OnSwiping;
		touchInput.MouseUpEvent += TouchInput_OnMouseUp;
	}

	public override void Pause ()
	{
		base.Pause ();
		swipeLine.enabled = false;

	}

	void TouchInput_OnTap (Vector2 obj)
	{
		if (isPaused || isHit)
			return;
		
		swipeLine.enabled = false;
		moveHoriz.SwitchDirection ();
	}

	void TouchInput_OnMouseUp (Vector3 obj)
	{
		if (isPaused || isHit)
			return;
		
		moveHoriz.Resume ();
		swipeLine.enabled = false;
	}


	bool _isThrowing = false;
	bool _isSwiping = false;

	void TouchInput_OnSwiping (Vector3 downMousePos, Vector3 mousePos)
	{
		if (isPaused || _isThrowing || isHit)
			return;

		if (!canThrow ()) {
			return;
		}
													
		var endPoint = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y));
		var startPoint = Camera.main.ScreenToWorldPoint (new Vector3 (downMousePos.x, downMousePos.y));

		var playerPoint = transform.position;

		var delta = startPoint - playerPoint;

		endPoint = endPoint - delta;
		endPoint.z = playerPoint.z;

		ShowDirectionLine (playerPoint, endPoint);

		moveHoriz.Pause ();

		if (endPoint.x < transform.position.x) {
			moveHoriz.TurnLeft ();
		} else {
			moveHoriz.TurnRight ();
		}

		if (!_isSwiping) {
			_isSwiping = true;
			StartThrowAnimation ();
		}
	}

	void TouchInput_OnSwipeRelease (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		if (isPaused || _isThrowing || isHit)
			return;

		if (!canThrow ()) {
			return;
		}

		_isSwiping = false;
		
		swipeLine.enabled = false;

		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		_isThrowing = true;

		ThrowStar (normalizedSwipeDir, throwSpeed);
		EndThrowAnimation ();
		LeanTween.delayedCall (gameObject, 0.2f, EndThrow);
	}

	void EndThrow ()
	{
		
		_isThrowing = false;
		ResumeMove ();
	}

	void ShowDirectionLine (Vector3 startpoint, Vector3 endPoint)
	{
		swipeLine.enabled = true;
		swipeLine.SetVertexCount (2);
		swipeLine.SetWidth (0.2f, 0.2f);
		swipeLine.SetColors (Color.yellow, Color.yellow);
		swipeLine.SetPosition (0, new Vector3 (startpoint.x, startpoint.y, transform.position.z + 1));
		swipeLine.SetPosition (1, new Vector3 (endPoint.x, endPoint.y, transform.position.z + 1));
	}
}

