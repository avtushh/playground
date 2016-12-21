using UnityEngine;
using System.Collections;

public class EnemyNinjaController : NinjaController{

	void Start(){
		DelayedThrow();
	}

	void DelayedThrow(){
		var delay = Random.Range(0, 10);

		LeanTween.delayedCall(delay, TryThrow);

	}

	void TryThrow(){
		if (ninjaStarThrow == null || !ninjaStarThrow.gameObject.activeInHierarchy){

			var direction = new Vector2(Random.Range(0, 1), Random.Range(-1, 0));
			ThrowStar(direction, throwSpeed);
		}
	}


}
