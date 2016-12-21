using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {

	public NinjaInput touchInput;
	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	// Use this for initialization
	void Start () {
		AddListeners();
	}

	void OnDestroy(){
		RemoveListeners();
	}

	void AddListeners(){
		touchInput.OnTap += TouchInput_OnTap;
		touchInput.OnSwipe += TouchInput_OnSwipe;
	}

	void RemoveListeners(){
		touchInput.OnTap -= TouchInput_OnTap;
		touchInput.OnSwipe -= TouchInput_OnSwipe;
	}

	void TouchInput_OnSwipe (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		var ninjaStar = GameObject.Instantiate(ninjaStarPrefab, transform.position, Quaternion.identity) as GameObject;

		swipeSpeed = 10;

		if (normalizedSwipeDir.y < 0.1f){
			normalizedSwipeDir.y = 0.1f;
		}

		var throwXSpeed = normalizedSwipeDir.x * swipeSpeed;
		var throwYSpeed = normalizedSwipeDir.y * swipeSpeed;

		var throwScr = ninjaStar.GetComponent<Throw>();

		throwScr.initThrowX = throwXSpeed;
		throwScr.initThrowY = throwYSpeed;

	}

	void TouchInput_OnTap (Vector2 obj)
	{
		moveHoriz.SwitchDirection();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
