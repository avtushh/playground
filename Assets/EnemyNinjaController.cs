using UnityEngine;
using System.Collections;

public class EnemyNinjaController : NinjaController{

	public float delayBetweenStars = 0.5f;

	[Header("Throw Frequency")]
	public int minFreq = 3;
	public int maxFreq = 8;

	void Start(){
		//Init ();
	}

	void Init ()
	{
		moveHoriz.MoveRight ();

		StartCoroutine(ChangeDirectionCoro());
		//StartCoroutine(ThrowCoro());

		DelayThrowStar();
	}

	void DelayThrowStar(){
		var delay = Random.Range(minFreq, maxFreq);
		LeanTween.delayedCall(gameObject, delay, ThrowStars);
	}

	void ThrowStars(){
		PauseMove();

		StartThrowAnimation(true);

		var numStars = Random.Range(1, 4);

		var throwGap = 0.4f;

		for(int i = 0; i < numStars; i++){
			LeanTween.delayedCall(gameObject, (i * throwGap), ()=>ThrowRandomDirectionStar(-throwSpeed));
		}

		LeanTween.delayedCall(gameObject, throwGap * numStars + 0.5f,() => {
			ResumeMove();
			DelayThrowStar();
		} );
	}

	IEnumerator ChangeDirectionCoro(){
		while(true){
			if (!isPaused && !isThrowing){
				var delay = Random.Range(2, 8);
				yield return new WaitForSeconds(delay);
				moveHoriz.SwitchDirection();
			}
		}
	}

	IEnumerator ThrowCoro(){
		while(true){
			if (!isPaused){
				
				var delay = Random.Range(minFreq, maxFreq);
				yield return new WaitForSeconds(delay);

				isThrowing = true;

				var numStars = Random.Range(1, 4);

				for (int i = 0; i < numStars; i++) {
					ThrowRandomDirectionStar(-throwSpeed);

					if (i == 0){
						//PauseMove();
					}
					yield return new WaitForSeconds(delayBetweenStars);
				}

				isThrowing = false;

				//ResumeMove();
			}
		}
	}

	public override void Pause ()
	{
		base.Pause ();
		StopAllCoroutines();
		LeanTween.cancel(gameObject);
	}

	public override void Resume ()
	{
		base.Resume ();
		Init ();
	}


}
