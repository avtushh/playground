using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Renderer))]
public class CameraViewListener : MonoBehaviour{

	public static event Action<bool, string> onVisibilityChange = (b,sim) => {};

	Bounds _bounds;

	public bool isVisible;

	void Start(){
		_bounds = GetComponent<Renderer>().bounds;
	}

	void Update(){
		bool inCameraBounds = IsInCameraBounds();

		if (isVisible){
			if (!inCameraBounds){
				onVisibilityChange(false, tag);
			}
		}else{
			if (inCameraBounds){
				onVisibilityChange(true, tag);
			}
		}
		
		isVisible = inCameraBounds;
	}

	public bool IsInCameraBounds(bool addSafetlyDelta = false){

		return IsPointInCameraBounds(_bounds.min, addSafetlyDelta) || IsPointInCameraBounds(_bounds.max, addSafetlyDelta);
	}

	public static bool IsPointInCameraBounds(Vector3 worldPoint, bool addSafetlyDelta = false){
		var viewPortPosition = Camera.main.WorldToViewportPoint(worldPoint);

		//print ("enemy at: " + viewPortPosition);

		float minViewPort = addSafetlyDelta?-0.1f:0;
		float maxViewPort = addSafetlyDelta?1.1f:1;

		if (Mathf.Clamp(viewPortPosition.x, minViewPort, maxViewPort) != viewPortPosition.x || Mathf.Clamp(viewPortPosition.y, minViewPort, maxViewPort)!= viewPortPosition.y){
			return false;
		}

		return true;
	}
}
