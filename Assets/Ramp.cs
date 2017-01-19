using UnityEngine;
using System.Collections;

namespace TabTale
{
	public class Ramp : MonoBehaviour
	{
	
		public float velX, velY;
		public float gravityScale;
	
		CameraViewListener viewListener;
	
		public Transform shapeContainer;

		public ShapesList shapesBank;

		public void LoadShapePrefab (GameObject prefab)
		{
			var go = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;

			go.transform.SetParent (shapeContainer, false);

			go.GetComponent<ShapeDataComponent>().Set(gameObject);

			viewListener = GetComponentInChildren<CameraViewListener> ();
		}

		void Start ()
		{
			if (shapesBank != null){
				var shape = shapesBank.GetRandomShape();

				LoadShapePrefab(shapesBank.prefabByNameDict[shape]);
			}
		}

		public bool IsVisible (bool addSafetyDelta)
		{

			return viewListener.IsInCameraBounds (addSafetyDelta);
		}

		public void Kill ()
		{
			Destroy (gameObject);
		}

		public void DestroyShape(){
			if (viewListener != null){
				Destroy(viewListener.gameObject);
			}	
		}
	}
}
