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
		RandomDirectionStarThrow();
		LeanTween.delayedCall(gameObject, 0.5f,() => {
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
					RandomDirectionStarThrow();

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

	void RandomDirectionStarThrow(){
		var direction = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
		ThrowStar(direction, -throwSpeed);
	}

	public override void Pause ()
	{
		base.Pause ();
		StopAllCoroutines();
	}

	public override void Resume ()
	{
		base.Resume ();
		Init ();
	}


}
