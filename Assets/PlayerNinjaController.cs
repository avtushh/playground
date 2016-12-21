using UnityEngine;
using System.Collections;

public class PlayerNinjaController : NinjaController{
	
	public NinjaInput touchInput;

	void TouchInput_OnTap (Vector2 obj)
	{
		moveHoriz.paused = false;
		moveHoriz.SwitchDirection();
	}

	void TouchInput_OnSwipe (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		ThrowStar (normalizedSwipeDir, throwSpeed);

		LeanTween.delayedCall(0.5f, UnPauseNinja);
	}

	protected override void RemoveListeners(){
		base.RemoveListeners();
		touchInput.OnTap -= TouchInput_OnTap;
		touchInput.OnSwipe -= TouchInput_OnSwipe;
		touchInput.OnMouseDown -= TouchInput_OnMouseDown;
	}

	protected override void AddListeners(){
		base.AddListeners();
		touchInput.OnTap += TouchInput_OnTap;
		touchInput.OnSwipe += TouchInput_OnSwipe;
		touchInput.OnMouseDown += TouchInput_OnMouseDown;
	}

	void TouchInput_OnMouseDown ()
	{
		moveHoriz.paused = true;
	}
}
