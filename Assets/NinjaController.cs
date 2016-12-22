using UnityEngine;
using System.Collections;
using System;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public float throwSpeed = 10f;

	public int lives = 3;

	public bool isPaused = false;

	public bool isThrowing = false;

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

		var ninjaGo = GameObject.Instantiate (ninjaStarPrefab, transform.position, Quaternion.identity) as GameObject;
		var ninjaStarThrow = ninjaGo.GetComponent<NinjaStar> ();

		ninjaStarThrow.Throw(throwXSpeed, throwYSpeed);
		ThrowStarEvent(ninjaStarThrow);
	}

	protected void ResumeMove(){
		moveHoriz.Resume();
	}

	protected void PauseMove(){
		moveHoriz.Pause();
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

