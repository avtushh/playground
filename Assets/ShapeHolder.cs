using UnityEngine;
using System.Collections;
using System;

namespace TabTale
{
	public class ShapeHolder : MonoBehaviour{

		public static event Action<GameObject> OnDestroyEvent = (g) => {};

		public bool isAlive = false;

		CameraViewListener viewListener;

		public Transform shapeContainer;

		ParticleSystem particles;

		ParticleSystemAutoDestroy particlesAutoDestroy;

		void Start ()
		{
			viewListener = GetComponentInChildren<CameraViewListener> ();
			particles = GetComponentInChildren<ParticleSystem> ();

			if (particles != null){
				particlesAutoDestroy = particles.GetComponent<ParticleSystemAutoDestroy>();
				particlesAutoDestroy.OnFinishedPlaying += OnDestructionParticlesDone;
			}

			isAlive = true;

			OnStart();
		}

		void OnDestructionParticlesDone(){
			Destroy(gameObject);
		}

		protected virtual void OnStart(){}

		public void LoadShapePrefab (GameObject prefab)
		{
			var go = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;

			go.transform.SetParent (shapeContainer, false);

			go.GetComponent<ShapeDataComponent>().Set(gameObject);

			viewListener = GetComponentInChildren<CameraViewListener> ();
		}

		public bool IsVisible (bool addSafetyDelta)
		{

			return viewListener.IsInCameraBounds (addSafetyDelta);
		}

		public void Kill ()
		{
			if (!isAlive)
				return;
			SoundManager2.PlayKillSound();
			isAlive = false;

			if (particles != null){
				var emission = particles.emission;
				emission.enabled = true;
				particles.Play();
				particlesAutoDestroy.enabled = true;

				OnKilled();
			}else{
				OnKilled();
				Destroy (gameObject);
			}
		}

		protected virtual void OnKilled(){
			OnDestroyEvent(gameObject);
		}

		public void DestroyShape(){
			if (viewListener != null){
				Destroy(viewListener.gameObject);
			}	
		}

	}
	
}
