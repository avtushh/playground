using UnityEngine;
using System.Collections;

public class EnemyNinjaController : NinjaController{

	public float delayBetweenStars = 0.5f;

	[Header("Throw Frequency")]
	public float minFreq = 1;
	public int maxFreq = 6;

	public NinjaController opponentTransform;

	public DetectStarCollision detector;

	protected override void AddListeners ()
	{
		base.AddListeners ();
		detector.StarComingEvent += Detector_StarComingEvent;

	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		detector.StarComingEvent -= Detector_StarComingEvent;
	}

	void Detector_StarComingEvent (NinjaStar star)
	{
		if (moveHoriz.direction == 1){ // moving right
			if (star.transform.position.x < transform.position.x){ // star is to my right
				moveHoriz.MoveLeft();
			}
		}else{ // moving left
			if (star.transform.position.x > transform.position.x){ // star is to my left
				moveHoriz.MoveRight();
			}
		}
	}

	void StartAI ()
	{
		moveHoriz.Resume ();

		StartCoroutine(ChangeDirectionCoro());

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

		var numStars = Random.Range(1, Mathf.Min(ActiveStarsCount, 4));

		var throwGap = 0.4f;

		for(int i = 0; i < numStars; i++){

			var target = new Vector3(opponentTransform.transform.position.x, opponentTransform.GetComponent<Collider2D>().bounds.max.y, 0);

			var distanceFromTarget = Vector3.Distance(target, transform.position);

			var timeToTarget = distanceFromTarget / Mathf.Abs(throwSpeed);

			var moveHoriz = opponentTransform.GetComponent<MoveHoriz>();

			if (moveHoriz != null && moveHoriz.enabled){
				target.x = moveHoriz.getFuturePosX(timeToTarget);
			}

			var obstaclesInTheWay = TestHit(target);

			if (obstaclesInTheWay.Length > 0){

				var cheatTarget = FindObjectOfType<EnemyCheatTarget>();

				if (cheatTarget != null){
					target = cheatTarget.transform.position;
				}
			}

			LeanTween.delayedCall(gameObject, (i * throwGap), ()=>ThrowAt(target));	
		}

		LeanTween.delayedCall(gameObject, throwGap * numStars + 0.5f,() => {
			ResumeMove();
			DelayThrowStar();
		} );
	}

	RaycastHit2D[] TestHit(Vector3 target){

		Vector3 deltaVector = target - transform.position;

		var direction = deltaVector.normalized;

		var obstaclesHit = Physics2D.RaycastAll(transform.position, new Vector2(direction.x, direction.y), Mathf.Infinity, 1 << LayerMask.NameToLayer("Obstacle"));

		return obstaclesHit;
	}

	Vector2 GetMinObstalcesAngle(){
		
		float minObstacles = Mathf.Infinity;
		float minAngle = 0f;

		for (float i = -0.3f; i <= 0.3f; i+=0.05f) {
			var currentObstacles = Physics2D.RaycastAll(transform.position, new Vector2(i, -1), Mathf.Infinity, 1 << LayerMask.NameToLayer("Obstacle"));
			if (currentObstacles.Length < minObstacles){
				
			}
		}

		return new Vector2(minAngle, -1);
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
