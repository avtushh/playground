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

		public List<ShapeDataComponent> visibleShapes;

		public Text txtScore;
		public int score;

		void UpdateScore ()
		{
			txtScore.text = score.ToString();
		}

		void Start ()
		{
			score = 0;
			UpdateScore ();
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
			player.OnLand += Player_OnLand;
			player.OnDie += Player_OnDie;
			player.OnDanger += Player_OnDanger;
			player.OnStartSlide += () => tapToSlide.SetActive (false);
			CameraViewListener.onVisibilityChange += CameraViewListener_onVisibilityChange;
		}


		void RemoveListeners ()
		{
			GestureBehaviour.OnGesturesRecognition -= OnGesturessResult;
			player.OnLand -= Player_OnLand;
			player.OnDie -= Player_OnDie;
			player.OnSlowDown -= Player_OnDanger;
			CameraViewListener.onVisibilityChange -= CameraViewListener_onVisibilityChange;

		}

		void CameraViewListener_onVisibilityChange (bool visible, GameObject gameObject)
		{
			if (gameObject.tag == GrindMeTags.Shape){

				var shapeHolder = gameObject.GetComponent<ShapeDataComponent>();
				if (visible){
					visibleShapes.Add(shapeHolder);
				}else{
					visibleShapes.Remove(shapeHolder);

					if (visibleShapes.Count == 0){

						if (gestureBehaviour != null && gestureBehaviour.gameObject != null)
							gestureBehaviour.gameObject.SetActive (false);
					}
				}
			}
		}


		void Player_OnDanger ()
		{
			if (!gestureBehaviour.gameObject.activeInHierarchy){
				gestureBehaviour.gameObject.SetActive (true);
				SoundManager2.PlayDangerSound();	
			}
		}


		IEnumerator CheckObstaclesForGesturesCoro ()
		{
			while (true) {
				yield return new WaitForSeconds (0.05f);

				var shapesToFind = visibleShapes.Select (item => item.Type).ToList ();

				GestureBehaviour.shapesToFind = shapesToFind;
			}
		}

		void OnGesturessResult (Gesture gesture, List<Result> results)
		{
			gestureBehaviour.ClearGesture ();

			if (results == null || results.Count == 0) {
				return;
			}

			Result minScoreShape = new Result ();

			visibleShapes.ForEach (shape => {
				var result = results.FirstOrDefault (x => x.Name == shape.Type);

				if (result != null) {
					if (minScoreShape.OriginalScore > result.OriginalScore) {
						minScoreShape = result;
					}
				}
			});

			bool didDestroy = DestroyEnemiesOfType (minScoreShape.Name);
		}

		bool DestroyEnemiesOfType (string name)
		{
			

			var enemy = player.GetClosestEnemy(name);

			if (enemy != null){
				Debug.LogWarning("******* WANT TO DESTROY shape: " + name + " ,found shape: " + enemy.shapeData.Type);
				enemy.shapeData.Activate();
				score++;
				UpdateScore();
				return true;
			}

			return false;
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
	}
}