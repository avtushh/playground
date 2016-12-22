using UnityEngine;
using System.Collections;

public class MoveHoriz : MonoBehaviour {

	public float speed = 1;
	public int direction = 1;

	public Transform leftBound, rightBound;

	public AnimationCurve moveCurve;

	LTDescr currentTween;

	public SpriteRenderer icon;

	public bool isInitalPosLeft = true;

	bool _paused;

	public bool paused{
		get{
			return _paused;
		}

		set{
			if (_paused == value){
				return;
			}

			_paused = value;

			if (_paused)
				LeanTween.cancel(gameObject);
			else{
				Move();
			}


		}

	}

	float _right, _left;

	void Start () {
		_left = leftBound.position.x;
		_right = rightBound.position.x;
	}

	public void MoveRight(){

		TurnRight ();

		direction = 1;
		if (currentTween != null)
			LeanTween.cancel(currentTween.uniqueId);
		var time = Mathf.Abs(transform.position.x - _right) / 5;

		currentTween = LeanTween.moveX(gameObject, _right, time).setEase(moveCurve).setOnComplete(MoveLeft);
	}

	public void TurnRight ()
	{
		icon.flipX = isInitalPosLeft?true:false;
	}

	public void TurnLeft ()
	{
		icon.flipX = isInitalPosLeft?false:true;
	}

	public void MoveLeft(){

		TurnLeft ();

		direction = -1;
		if (currentTween != null)
			LeanTween.cancel(currentTween.uniqueId);
		var time = Mathf.Abs(transform.position.x - _left) / 5;

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