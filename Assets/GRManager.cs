using UnityEngine;
using System.Collections;
using GestureRecognizer;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TabTale
{
	public class GRManager : MonoBehaviour
	{

		public SRPlayer player;

		public GestureBehaviour gestureBehaviour;
		public GameObject gameOverCanvas;
		public GameObject tapToSlide;


		void Start ()
		{
			AddListeners ();

			StartCoroutine (CheckObstaclesForGesturesCoro ());
		}

		void OnDestroy ()
		{
			RemoveListeners ();
		}

		void AddListeners ()
		{
			//GestureBehaviour.OnGestureRecognition += OnGestureResult;
			GestureBehaviour.OnGesturesRecognition += OnGesturessResult;
			player.OnJump += Player_OnJump;
			player.OnLand += Player_OnLand;
			player.OnDie += Player_OnDie;
			player.OnStartSlide += () => tapToSlide.SetActive (false);
		}

		void RemoveListeners ()
		{
			GestureBehaviour.OnGesturesRecognition -= OnGesturessResult;

			player.OnJump -= Player_OnJump;
			player.OnLand -= Player_OnLand;
			player.OnDie -= Player_OnDie;
		}




		IEnumerator CheckObstaclesForGesturesCoro ()
		{
			while (true) {
				yield return new WaitForSeconds (0.05f);

				var grinders = GetVisibleGrinders (false);
				var shapesToFind = grinders.Select (item => item.type).ToList ();

				GestureBehaviour.shapesToFind = shapesToFind.Select (x => x.ToString ()).ToList ();
			}
		}

		public List<Grinder> GetVisibleGrinders (bool addSafety)
		{
			return FindObjectsOfType<Grinder> ().Where (x => x.IsVisible (addSafety)).ToList ();
		}

		void OnGesturessResult (Gesture gesture, List<Result> results)
		{
			gestureBehaviour.ClearGesture ();
			if (results == null || results.Count == 0) {
				return;
			}

			Result minScoreShape = new Result ();

			var grinders = GetVisibleGrinders (false);

			grinders.ForEach (grinder => {
				var result = results.FirstOrDefault (x => x.Name == grinder.type.ToString ());

				if (result != null) {
					if (minScoreShape.OriginalScore > result.OriginalScore) {
						minScoreShape = result;
					}
				}
			});

			bool didDestroy = DestroyEnemiesOfType (minScoreShape.Name);

			if (didDestroy) {
				CheckNoEnemies ();
			}
		}

		void CheckNoEnemies ()
		{
			var grinders = GetVisibleGrinders (false).Where (x => x.isAlive).ToList ();
			if (grinders == null || grinders.Count == 0) {
				player.OnNoEnemies ();

			}
		}

		bool DestroyEnemiesOfType (string name)
		{
			print ("Check shape: " + name);
			var grindersWithShape = GetVisibleGrinders (false).Where (x => x.type == name).ToList ();
			if (grindersWithShape.Count > 0) {
				print ("Found shape: " + name);
				grindersWithShape.ForEach (g => {
					g.Kill ();
				});
				return true;
			} else {
				return false;
				print ("try again");
			}



		}




		void Player_OnDie ()
		{
			Time.timeScale = 1f;
			gestureBehaviour.ClearGesture ();
			gestureBehaviour.gameObject.SetActive (false);
			LeanTween.delayedCall (1f, () => gameOverCanvas.SetActive (true));
		}

		void Player_OnLand ()
		{
			gestureBehaviour.gameObject.SetActive (false);
		}

		void Player_OnJump ()
		{
			gestureBehaviour.gameObject.SetActive (true);

		}
	}
}