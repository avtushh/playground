using UnityEngine;
using System.Collections;
using System;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public NinjaStar ninjaStarThrow;

	public float throwSpeed = 10f;

	public int lives = 3;

	public bool isPaused = false;

	public event Action<int,NinjaStar> HitEvent = (lives,star) => {};
	public event Action<NinjaStar> ThrowStarEvent = (star) => {};

	void Start () {
		AddListeners();
	}

	void OnDestroy(){
		RemoveListeners();
	}

	protected virtual void AddListeners(){
		
	}

	protected virtual void RemoveListeners(){
		
	}

	public virtual void Pause(){
		isPaused = true;
		PauseMove();
		RemoveListeners();
	}

	public virtual void Resume(){
		isPaused = false;
		ResumeMove();
		AddListeners();
	}

	public virtual void Die(){
		Destroy(gameObject);
	}

	protected void ThrowStar (Vector2 normalizedSwipeDir, float throwSpeed)
	{
		var throwXSpeed = normalizedSwipeDir.x * throwSpeed;
		var throwYSpeed = normalizedSwipeDir.y * throwSpeed;

		//if (ninjaStarThrow == null){
			var ninjaGo = GameObject.Instantiate (ninjaStarPrefab, transform.position, Quaternion.identity) as GameObject;
			ninjaStarThrow = ninjaGo.GetComponent<NinjaStar> ();
		//}else{
		//	ninjaStarThrow.transform.position = transform.position;
		//	ninjaStarThrow.Activate();
		//}

		ninjaStarThrow.Throw(throwXSpeed, throwYSpeed);
		ThrowStarEvent(ninjaStarThrow);
	}

	protected void ResumeMove(){
		moveHoriz.paused = false;
	}

	protected void PauseMove(){
		moveHoriz.paused = true;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (isPaused)
			return;
		
		if (other.gameObject.CompareTag("Bullet")){

			var star = other.gameObject.GetComponent<NinjaStar>();

			if (star.tagToHit == gameObject.tag){
				Debug.LogError("Hit " + star.tagToHit);
				lives--;
				HitEvent(lives, star);
				star.Hit();
			}
		}
	}
}

