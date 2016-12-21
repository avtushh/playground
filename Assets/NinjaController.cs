using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour {

	public NinjaInput touchInput;
	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public Throw ninjaStarThrow;

	public float throwSpeed = 10f;

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
		touchInput.OnMouseDown += TouchInput_OnMouseDown;
	}

	void TouchInput_OnMouseDown ()
	{
		moveHoriz.paused = true;
	}

	void RemoveListeners(){
		touchInput.OnTap -= TouchInput_OnTap;
		touchInput.OnSwipe -= TouchInput_OnSwipe;
		touchInput.OnMouseDown -= TouchInput_OnMouseDown;
	}

	void ThrowStar (Vector2 normalizedSwipeDir)
	{
		if (normalizedSwipeDir.y < 0.1f) {
			normalizedSwipeDir.y = 0.1f;
		}

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

	void TouchInput_OnSwipe (Vector2 normalizedSwipeDir, float swipeSpeed)
	{
		ThrowStar (normalizedSwipeDir);

		LeanTween.delayedCall(0.5f, UnPauseNinja);
	}

	void UnPauseNinja(){
		moveHoriz.paused = false;
	}

	void PauseNinja(){
		moveHoriz.paused = true;
	}

	void TouchInput_OnTap (Vector2 obj)
	{
		moveHoriz.paused = false;
		moveHoriz.SwitchDirection();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
