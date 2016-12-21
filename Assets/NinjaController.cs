using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public Throw ninjaStarThrow;

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

		if (ninjaStarThrow == null){
			var ninjaGo = GameObject.Instantiate (ninjaStarPrefab, transform.position, Quaternion.identity) as GameObject;
			ninjaStarThrow = ninjaGo.GetComponent<Throw> ();
		}else{
			ninjaStarThrow.transform.position = transform.position;
			ninjaStarThrow.Activate();
		}

		ninjaStarThrow.ThrowMe(throwXSpeed, throwYSpeed);
	}



	protected void UnPauseNinja(){
		moveHoriz.paused = false;
	}

	protected void PauseNinja(){
		moveHoriz.paused = true;
	}
}

