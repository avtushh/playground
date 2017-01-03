using UnityEngine;
using System.Collections;

public class MoveHoriz : MonoBehaviour {

	public float speed = 1;
	public int direction = 1;
	public float animationTime = 1.2f;

	public Transform leftBound, rightBound;

	public AnimationCurve moveCurve;

	public LTDescr currentTween;

	public SpriteRenderer icon;

	public bool isInitalPosLeft = true;

	public bool moveOnStart = false;

	public Vector3 lastPos;

	bool _paused;

	public bool IsPaused {
		get {
			if (!enabled)
				return true;
			return _paused;
		}
	}

	float _right, _left;

	void Start () {
		_left = leftBound.position.x;
		_right = rightBound.position.x;

		if (moveOnStart){
			Resume();
		}
	}

	public void Pause(){
		if (!enabled)
			return;

		_paused = true;
		if (currentTween != null){
			LeanTween.cancel(gameObject, currentTween.id);
			currentTween = null;
		}
	}

	public void Resume(){
		if (!enabled)
			return;

		_paused = false;
		Move();
	}


	void Update(){
		if (currentTween != null){

			speed = (transform.position.x - lastPos.x) / Time.deltaTime;

			lastPos = transform.position;
		}
	}

	public float getFuturePosX(float futureTime){

		if (currentTween == null)
			return transform.position.x;

		var futureTweenTime = currentTween.passed + futureTime;

		if (futureTweenTime >= currentTween.time){

			var deltaTime = futureTweenTime - currentTween.time;

			var startPos = IsMovingRight()?_right:_left;
			var targetpos = IsMovingRight()?_left:_right;

			float deltaRatio = deltaTime / currentTween.time;

			return startPos + (targetpos * moveCurve.Evaluate(deltaRatio));
		}

		var futureRatio = futureTweenTime / currentTween.time;

		return LeanTween.tweenOnCurve(currentTween, futureRatio);
	}


	public void TurnRight ()
	{
		if (icon != null){
			icon.flipX = isInitalPosLeft?true:false;
			direction = 1;
		}
	}

	public void TurnLeft ()
	{
		if (icon != null){
			icon.flipX = isInitalPosLeft?false:true;
			direction = -1;
		}
	}

	public void MoveRight(){

		TurnRight ();

		direction = 1;
		if (currentTween != null){
			LeanTween.cancel(gameObject, currentTween.id);
		}

		var time = GetAnimationTime(_right);

		currentTween = LeanTween.moveX(gameObject, _right, time).setEase(moveCurve).setOnComplete(MoveLeft);
	}

	float GetAnimationTime(float targetX){

		var distanceForTime = Mathf.Abs(_right - _left); // 6

		var ratioOfDistance = (Mathf.Abs(targetX - transform.position.x)) / distanceForTime;

		return animationTime * ratioOfDistance;
	}

	public bool IsMovingRight(){
		return direction == 1;
	}

	public void MoveLeft(){

		TurnLeft ();

		direction = -1;
		if (currentTween != null){
			LeanTween.cancel(gameObject, currentTween.id);
		}

		var time = GetAnimationTime(_left);

		currentTween = LeanTween.moveX(gameObject, _left, time).setEase(moveCurve).setOnComplete(MoveRight);
	}
		
	public void SwitchDirection(){

		if (_paused){
			_paused = false;
		}

		direction *= -1;

		Move ();
	}


	void Move ()
	{
		//Debug.LogError("MOVE: " + direction);
		if (direction == 1) {
			MoveRight ();
		}
		else {
			MoveLeft ();
		}
	}


}