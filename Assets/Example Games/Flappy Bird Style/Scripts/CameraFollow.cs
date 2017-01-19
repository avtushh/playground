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

		Camera cam;

		float orgZoom;
		float targetZoom;

		void Start ()
		{
			topTransform = bottomTransform = target;

			CameraViewListener.OnDestroyEvent += Grinder_OnDestroyEvent;

			cam = GetComponent<Camera> ();

			orgZoom = cam.orthographicSize;
			targetZoom = cam.orthographicSize;
		}

		void Grinder_OnDestroyEvent (GameObject obj)
		{
			elementsInView.Remove (gameObject.transform);
		}

		void Update ()
		{
			var x = followHorizontal ? target.position.x + xOffset : transform.position.x;
			//var y = followVertical ? target.position.y + yOffset : transform.position.y;

			UpdateTargetY ();

			var y1 = Mathf.SmoothDamp (transform.position.y, targetY, ref currVel, 0.2f);

			transform.position = new Vector3 (x, y1, transform.position.z);

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
			if (other.tag == GrindMeTags.Enemy || other.tag == GrindMeTags.Player || other.tag == GrindMeTags.Jumper) {
				elementsInView.Add (other.transform);
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (other.tag == GrindMeTags.Enemy|| other.tag == GrindMeTags.Jumper) {
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

			if (topTransform == target && bottomTransform == target){
				targetY = target.position.y + yOffset;
			}else
				targetY = (topTransform.position.y + bottomTransform.position.y) / 2;
		}




	}
}
