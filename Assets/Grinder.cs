using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TabTale
{
	public class Grinder : MonoBehaviour
	{

		#region INotifyDestroy implementation

		public static event Action<GameObject> OnDestroyEvent = (g) => {};

		#endregion

		public bool isAlive = false;

		CameraViewListener viewListener;

		public Transform shapeContainer;

		public SpriteRenderer sprRenderer;
		public Sprite sprNormal, sprBloody;

		public static int lastTypeIndex = -1;

		ParticleSystem particles;


		public void LoadShapePrefab (GameObject prefab)
		{
			var go = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;

			go.transform.SetParent (shapeContainer, false);

			go.GetComponent<ShapeDataComponent>().Set(gameObject);

			isAlive = true;
		}

		void Start ()
		{
			viewListener = GetComponentInChildren<CameraViewListener> ();
			particles = GetComponentInChildren<ParticleSystem> ();
		}

		void Update(){
			
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
				OnDestroyEvent(gameObject);
			}else{
				OnDestroyEvent(gameObject);
				Destroy (gameObject);
			}



		}

		public void OnKillPlayer ()
		{
			sprRenderer.sprite = sprBloody;
		}

		void OnDestroy(){
			
		}

	
	}


}