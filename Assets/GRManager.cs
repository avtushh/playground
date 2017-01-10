using UnityEngine;
using System.Collections;
using GestureRecognizer;
using System.Linq;
using UnityEngine.UI;

public class GRManager : MonoBehaviour {

	public SRPlayer player;

	public GestureBehaviour gestureBehaviour;
	public GameObject gameOverCanvas;

	bool _recognizeShape = false;

	void Start(){
		AddListeners ();
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

		var grinders = FindObjectsOfType<Grinder>().ToList().Where(x => x.type == result.Name && x.IsInCameraBounds()).ToList();

		if (grinders.Count > 0){
			Debug.LogError("FOund!");
			gestureBehaviour.gameObject.SetActive(false);
			grinders.ForEach(g => Destroy(g.gameObject));
			LeanTween.delayedCall(0.2f, () => player.GetComponent<SlowDownJump>().ResetPhysics());
		}else{
			print ("try again");
		}

		gestureBehaviour.ClearGesture();



	}
}
