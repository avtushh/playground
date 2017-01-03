using UnityEngine;
using System.Collections;

public class MoveVert : MonoBehaviour {

	public float amount;

	float _maxUp, _maxDown;

	public float animationTime = 1.2f;

	LTDescr _currentTween;

	public AnimationCurve moveCurve;

	void Start () {
		_maxUp = transform.position.y + amount;
		_maxDown = transform.position.y - amount;

		if (Random.Range(0,2) == 0){
			MoveDown();
		}else{
			MoveUp();
		}
	}

	void MoveDown(){

		float time = GetAnimationTime(_maxDown) ;

		_currentTween = LeanTween.moveY(gameObject, _maxDown, time).setEase(moveCurve).setOnComplete(MoveUp);
	}

	void MoveUp(){

		float time = GetAnimationTime(_maxUp) ;

		_currentTween = LeanTween.moveY(gameObject, _maxUp, time).setEase(moveCurve).setOnComplete(MoveDown);
	}

	float GetAnimationTime(float targetY){

		var distanceForTime = Mathf.Abs(_maxUp - _maxDown); // 6

		var ratioOfDistance = (Mathf.Abs(targetY - transform.position.x)) / distanceForTime;

		return animationTime * ratioOfDistance;
	}

	void OnDestroy(){
		LeanTween.cancel(gameObject, _currentTween.id);
	}

}
