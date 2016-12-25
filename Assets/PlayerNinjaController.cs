using UnityEngine;
using System.Collections;

public class PlayerNinjaController : NinjaController{
	
	public NinjaInput touchInput;
	public LineRenderer swipeLine;

	public PowerUp.PowerupType activePowerup = PowerUp.PowerupType.None;

	public GameObject shield;

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

	public override void Pause ()
	{
		base.Pause ();
		shield.SetActive(false);
	}

	public void SetPowerUp (PowerUp.PowerupType powerUpType)
	{
		switch(powerUpType){
			case PowerUp.PowerupType.Shield:
				SetShield();
				break;
			case PowerUp.PowerupType.Split:
				break;
			case PowerUp.PowerupType.DestroyObstalceGroup:
				break;
		}
	}

	void TouchInput_OnTap (Vector2 obj)
	{
		if (isPaused)
			return;
		
		swipeLine.enabled = false;
		moveHoriz.SwitchDirection();
	}

	void TouchInput_OnSwipe (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		if (isPaused)
			return;
		
		swipeLine.enabled = false;

		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

		ThrowStar (normalizedSwipeDir, throwSpeed);

		LeanTween.delayedCall(0.2f, ResumeMove);
	}


	void SetShield ()
	{
		shield.SetActive(true);
	}


	void TouchInput_OnMouseUp (Vector3 obj)
	{
		if (isPaused)
			return;
		
		moveHoriz.Resume();
		swipeLine.enabled = false;
	}

	void TouchInput_OnUpdateSwipe (Vector2 mousePos)
	{
		if (isPaused)
			return;
		
		var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

		swipeLine.enabled = true;
		swipeLine.SetVertexCount(2);
		swipeLine.SetWidth(0.1f, 0.1f);
		swipeLine.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1));
		swipeLine.SetPosition(1, new Vector3(worldPoint.x, worldPoint.y, transform.position.z + 1));

		moveHoriz.Pause();

		if (worldPoint.x < transform.position.x){
			//if (!moveHoriz.IsLeft)
				moveHoriz.TurnLeft();
		}else{
			//if (moveHoriz.IsLeft)
				moveHoriz.TurnRight();
		}
	}
}

