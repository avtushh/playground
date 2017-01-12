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


	void Start(){
		AddListeners ();

		StartCoroutine(CheckObstaclesForGesturesCoro());
	}

	void OnDestroy(){
		RemoveListeners();
	}

	void AddListeners ()
	{
		//GestureBehaviour.OnGestureRecognition += OnGestureResult;
		GestureBehaviour.OnGesturesRecognition += OnGesturessResult;
		player.OnJump += Player_OnJump;
		player.OnLand += Player_OnLand;
		player.OnDie += Player_OnDie;
	}

	void RemoveListeners ()
	{
		GestureBehaviour.OnGesturesRecognition -= OnGesturessResult;

		player.OnJump -= Player_OnJump;
		player.OnLand -= Player_OnLand;
		player.OnDie -= Player_OnDie;
	}

	IEnumerator CheckObstaclesForGesturesCoro(){
		while(true){
			yield return new WaitForSeconds(0.05f);

			var grinders = GetVisibleGrinders(false);
			var shapesToFind = grinders.Select(item => item.type).ToList();

			GestureBehaviour.shapesToFind = shapesToFind;
		}
	}

	public List<Grinder> GetVisibleGrinders(bool addSafety){
		return FindObjectsOfType<Grinder>().Where(x=> x.IsVisible(addSafety)).ToList();
	}

	void OnGesturessResult (Gesture gesture, List<Result> results)
	{
		Result minScoreShape = new Result();

		var grinders = GetVisibleGrinders(false);

		grinders.ForEach(grinder => {
			var res = results.FirstOrDefault(x => x.Name == grinder.type);

			if (res != null){
				if (minScoreShape.Score > res.Score){
					minScoreShape = res;
				}
			}
		});

		gestureBehaviour.ClearGesture ();

		bool didDestroy = DestroyEnemiesOfType (minScoreShape.Name);

		if (didDestroy){
			//CheckForNoEnemies ();
		}
	}

	void CheckForNoEnemies ()
	{
		var grinders = GetVisibleGrinders(true).Where(x => x.isAlive).ToList();
		if (grinders == null || grinders.Count == 0) {
			
			player.GetComponent<SlowDownJump> ().ResetPhysics ();
		}
	}

	bool DestroyEnemiesOfType (string name)
	{
		print ("Check shape: " + name);
		var grindersWithShape = GetVisibleGrinders(false).Where(x => x.type == name).ToList();
		if (grindersWithShape.Count > 0) {
			print ("Found shape: " + name);
			grindersWithShape.ForEach (g =>  {
				g.Kill();
			});
			return true;
		}
		else {
			return false;
			print ("try again");
		}



	}




	void Player_OnDie ()
	{
		Time.timeScale = 1f;
		gameOverCanvas.SetActive(true);
	}

	void Player_OnLand ()
	{
		gestureBehaviour.gameObject.SetActive(false);
	}

	void Player_OnJump ()
	{
		gestureBehaviour.gameObject.SetActive(true);

	}
}
