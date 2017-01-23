using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TabTale
{
	//[ExecuteInEditMode]
	public class CameraFollow : MonoBehaviour
	{
		
		public Transform target;
		//target for the camera to follow
		public float xOffset;
		//how much x-axis space should be between the camera and target
		public float yOffset;

		public bool followHorizontal, followVertical;

		public List<Transform> elementsInView;

		public Transform topTransform, bottomTransform;

		float currVel;
		float currZoomVel;

		float targetY;

		public Camera cam;

		float orgZoom;
		float targetZoom;

		SRPlayer player;

		void Start ()
		{
			topTransform = bottomTransform = target;

			CameraViewListener.OnDestroyEvent += Grinder_OnDestroyEvent;

			orgZoom = cam.orthographicSize;
			targetZoom = cam.orthographicSize;

			if (target.GetComponent<SRPlayer>() != null){
				player = target.GetComponent<SRPlayer>();
			}
		}

		void Grinder_OnDestroyEvent (GameObject obj)
		{
			elementsInView.Remove (gameObject.transform);
		}

		void LateUpdate ()
		{
			var x = followHorizontal ? target.position.x + xOffset : cam.transform.position.x;
			//var y = followVertical ? target.position.y + yOffset : transform.position.y;

			List<Transform> elementsToRemove = new List<Transform>();

			elementsInView.ForEach(e => {
				if (e != null && e.position.x < target.position.x){
					elementsToRemove.Add(e);
				}	
			});

			elementsToRemove.ForEach(e => {
				elementsInView.Remove(e);
			});

			UpdateTargetY ();

			var y1 = Mathf.SmoothDamp (cam.transform.position.y, targetY, ref currVel, 0.2f);

			cam.transform.position = new Vector3 (x, y1, cam.transform.position.z);

			if (!zooming)
				UpdateZoom();			
		}

		void SetZoom(float value){
			cam.orthographicSize = value;
		}

		void UpdateZoom(){

			var top = GetTopY(topTransform);
			var bottom = GetBottomY(bottomTransform);

			var topViewPort = cam.WorldToViewportPoint(top);
			var bottomViewPort = cam.WorldToViewportPoint(bottom);

			if (topViewPort.y > 1 || bottomViewPort.y < 0) {
				targetZoom = 3.6f;
				zooming = true;
			}else if (topViewPort.y <= 0.7f || bottomViewPort.y > 0.3f){
				targetZoom = orgZoom;
				zooming = true;
			}

			if (zooming){
				LeanTween.value(gameObject, SetZoom, cam.orthographicSize, targetZoom, 0.2f).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>zooming = false);
			}

		}

		bool zooming = false;

		Vector3 GetTopY(Transform t){
			var myRenderer = t.GetComponent<Renderer>();

			if (myRenderer != null){
				return myRenderer.bounds.max;
			}else{
				myRenderer = t.GetComponentInChildren<Renderer>();
				return myRenderer.bounds.max;
			}

		}

		Vector3 GetBottomY(Transform t){
			var myRenderer = t.GetComponent<Renderer>();

			if (myRenderer != null){
				return myRenderer.bounds.min;
			}else{
				myRenderer = t.GetComponentInChildren<Renderer>();
				return myRenderer.bounds.min;
			}

		}

		void OnDestroy ()
		{
			CameraViewListener.OnDestroyEvent -= Grinder_OnDestroyEvent;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == GrindMeTags.Enemy || other.tag == GrindMeTags.Jumper) {
				elementsInView.Add (other.transform);
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (other.tag == GrindMeTags.Enemy || other.tag == GrindMeTags.Jumper) {
				elementsInView.Remove (other.transform);
			}
		}

		void UpdateTargetY ()
		{
			topTransform = target;
			bottomTransform = target;

			elementsInView.ForEach (x => {
				if (x != null && x.position.y > topTransform.position.y) {
					topTransform = x;
				} else if (x != null && x.position.y < bottomTransform.position.y) {
					bottomTransform = x;
				}
			});
				
			if (topTransform != target && bottomTransform != target){
				topTransform = target;
				bottomTransform = target;
			}

			if (target != topTransform){
				if (Vector3.Distance(target.position, topTransform.position) > 5){
					topTransform = target;
				}
			}
				
			if (topTransform == target && bottomTransform == target){
				if (player.CurrState == SRPlayer.State.Jumping){
					targetY = target.position.y + yOffset-4;
					print("jump!");
				}else{
					targetY = target.position.y + yOffset;
				}

			}else
				targetY = (topTransform.position.y + bottomTransform.position.y) / 2;
		}




	}
}
