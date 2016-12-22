using UnityEngine;
using System.Collections;

public class EnemyNinjaController : NinjaController{

	void Start(){
		moveHoriz.MoveRight();
		DelayedThrow();
		DelayedChangedPosition();
	}

	void DelayedThrow(){
		var delay = Random.Range(3, 8);

		LeanTween.delayedCall(gameObject, delay, TryThrow);

	}

	void DelayedChangedPosition(){
		var delay = Random.Range(2, 8);

		LeanTween.delayedCall(gameObject, delay, moveHoriz.SwitchDirection);
	}

	void TryThrow(){
		//Debug.LogError("try throw");
		if (ninjaStarThrow == null || !ninjaStarThrow.gameObject.activeInHierarchy){

			var direction = new Vector2(Random.Range(0, 1f), Random.Range(0.1f, 1f));
			ThrowStar(direction, -throwSpeed);
			DelayedThrow();
		}else{
			LeanTween.delayedCall(gameObject, 1f, TryThrow);
		}
	}


}
