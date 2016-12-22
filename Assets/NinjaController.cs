using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public NinjaStar ninjaStarThrow;

	public float throwSpeed = 10f;

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
	}



	protected void UnPauseNinja(){
		moveHoriz.paused = false;
	}

	protected void PauseNinja(){
		moveHoriz.paused = true;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("Bullet")){

			var star = other.gameObject.GetComponent<NinjaStar>();

			if (star.tagToHit == gameObject.tag){
				Debug.LogError("Hit " + star.tagToHit);
				star.Hit();
			}
		}
	}
}

