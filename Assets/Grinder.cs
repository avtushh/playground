using UnityEngine;
using System.Collections;

public class Grinder : MonoBehaviour {

	public string type;

	public Collider2D col2d;

	VisibleListener _visibleListener;

	Bounds _bounds;

	void Start(){
		_bounds = col2d.bounds;

		_visibleListener = GetComponentInChildren<VisibleListener>();

		if (_visibleListener != null){
			_visibleListener.OnVisibilityChange += OnVisibilityChange;
		}
	}

	void OnVisibilityChange (bool visible)
	{
		Debug.LogError("Visibility Change: " + visible + " for transform at: " + _visibleListener.transform.position.x);
	}

	public bool IsInCameraBounds(bool addSafetlyDelta = false){

		return IsPointInCameraBounds(_bounds.min, addSafetlyDelta) || IsPointInCameraBounds(_bounds.max, addSafetlyDelta);
	}

	public static bool IsPointInCameraBounds(Vector3 worldPoint, bool addSafetlyDelta = false){
		var viewPortPosition = Camera.main.WorldToViewportPoint(worldPoint);

		float minViewPort = addSafetlyDelta?-0.3f:0;
		float maxViewPort = addSafetlyDelta?1.3f:1;

		if (Mathf.Clamp(viewPortPosition.x, minViewPort, maxViewPort) != viewPortPosition.x || Mathf.Clamp(viewPortPosition.y, minViewPort, maxViewPort)!= viewPortPosition.y){
			return false;
		}

		return true;
	}

}
