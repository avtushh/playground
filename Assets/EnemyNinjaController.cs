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

	void StartAI ()
	{
		moveHoriz.MoveRight ();

		StartCoroutine(ChangeDirectionCoro());
		//StartCoroutine(ThrowCoro());

		DelayThrowStar();
	}

	void DelayThrowStar(){
		LeanTween.delayedCall(gameObject, Random.Range(minFreq, maxFreq), ThrowStars);
	}

	void ThrowStars(){

		if (!canThrow()){
			DelayThrowStar();
			return;
		}

		PauseMove();

		StartThrowAnimation(true);

		var numStars = activeStars.Count;

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

	public override void Resume ()
	{
		base.Resume ();
		StartAI ();
	}


}
