using UnityEngine;
using System.Collections;

public class PlayerNinjaController : NinjaController{
	
	public NinjaInput touchInput;
	public LineRenderer swipeLine;

	void TouchInput_OnTap (Vector2 obj)
	{
		swipeLine.enabled = false;
		moveHoriz.SwitchDirection();
	}

	void TouchInput_OnSwipe (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		swipeLine.enabled = false;

		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		ThrowStar (normalizedSwipeDir, throwSpeed);

		LeanTween.delayedCall(0.2f, ResumeMove);
	}

	protected override void RemoveListeners(){
		base.RemoveListeners();
		touchInput.TapEvent -= TouchInput_OnTap;
		touchInput.SwipeDoneEvent -= TouchInput_OnSwipe;
		touchInput.SwipeUpdatedEvent -= TouchInput_OnUpdateSwipe;
		touchInput.MouseUpEvent -= TouchInput_OnMouseUp;
	}

	protected override void AddListeners(){
		base.AddListeners();
		touchInput.TapEvent += TouchInput_OnTap;
		touchInput.SwipeDoneEvent += TouchInput_OnSwipe;
		touchInput.SwipeUpdatedEvent += TouchInput_OnUpdateSwipe;
		touchInput.MouseUpEvent += TouchInput_OnMouseUp;
	}

	void TouchInput_OnMouseUp (Vector3 obj)
	{
		moveHoriz.paused = false;
		swipeLine.enabled = false;
	}

	void TouchInput_OnUpdateSwipe (Vector2 mousePos)
	{
		var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

		swipeLine.enabled = true;
		swipeLine.SetVertexCount(2);
		swipeLine.SetWidth(0.1f, 0.1f);
		swipeLine.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1));
		swipeLine.SetPosition(1, new Vector3(worldPoint.x, worldPoint.y, transform.position.z + 1));

		moveHoriz.paused = true;

		if (worldPoint.x < transform.position.x){
			//if (!moveHoriz.IsLeft)
				moveHoriz.TurnLeft();
		}else{
			//if (moveHoriz.IsLeft)
				moveHoriz.TurnRight();
		}
	}
}

