using UnityEngine;
using System.Collections;
using GestureRecognizer;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class GRManager : MonoBehaviour {

	public SRPlayer player;

	public GestureBehaviour gestureBehaviour;
	public GameObject gameOverCanvas;

	public List<Grinder> visibleGrinders;

	bool _recognizeShape = false;

	void Start(){
		AddListeners ();

		StartCoroutine(CheckObstaclesForGesturesCoro());
	}

	IEnumerator CheckObstaclesForGesturesCoro(){
		while(true){
			yield return new WaitForSeconds(0.1f);

			visibleGrinders = FindObjectsOfType<Grinder>().Where(x=> x.IsInCameraBounds()).ToList();
		}
	}

	void OnDestroy(){
		RemoveListeners();
	}

	void AddListeners ()
	{
		GestureBehaviour.OnGestureRecognition += GestureRecognizer_GestureBehaviour_OnGestureRecognition;
		player.OnJump += Player_OnJump;
		player.OnLand += Player_OnLand;
		player.OnDie += Player_OnDie;
	}

	void RemoveListeners ()
	{
		GestureBehaviour.OnGestureRecognition -= GestureRecognizer_GestureBehaviour_OnGestureRecognition;
		player.OnJump -= Player_OnJump;
		player.OnLand -= Player_OnLand;
		player.OnDie -= Player_OnDie;
	}

	void Player_OnDie ()
	{
		Time.timeScale = 1f;
		gameOverCanvas.SetActive(true);
	}

	void Player_OnLand ()
	{
		gestureBehaviour.gameObject.SetActive(false);
		_recognizeShape = false;
	}

	void Player_OnJump ()
	{
		if (!_recognizeShape){
			_recognizeShape = true;
			gestureBehaviour.gameObject.SetActive(true);
		}
	}

	void GestureRecognizer_GestureBehaviour_OnGestureRecognition (Gesture gesture, Result result)
	{
		print("gesture result: " + result.Name + ", score: " + result.Score);

		visibleGrinders = FindObjectsOfType<Grinder>().Where(x=> x.IsInCameraBounds()).ToList();

		var grinders = visibleGrinders.Where(x => x.type == result.Name).ToList();

		if (grinders.Count > 0){
			Debug.LogError("Found shape: " + result.Name);

			grinders.ForEach(g => {

				visibleGrinders.Remove(g);
				Destroy(g.gameObject);
			});


		}else{
			print ("try again");
		}

		gestureBehaviour.ClearGesture();

		if (visibleGrinders.Count == 0){
			Debug.LogError("reset physics");
			LeanTween.delayedCall(0.2f, () => player.GetComponent<SlowDownJump>().ResetPhysics());
		}
	}
}
