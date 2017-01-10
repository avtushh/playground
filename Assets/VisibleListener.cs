using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Renderer))]
public class VisibleListener : MonoBehaviour {

	public event Action<bool> OnVisibilityChange = (b) => {};

	bool _visible;

	Renderer _renderer;

	void Start(){
		_renderer = GetComponent<Renderer>();
	}

	void Update(){

		bool isVisible = _renderer.IsVisibleFrom(Camera.main);

		SetVisible(isVisible);
	}

	void SetVisible(bool val){

		if (_visible == val)
			return;
		
		_visible = val;

		//Debug.LogError("Visibility Change: " + _visible + " for transform at: " + transform.position.x);

		//OnVisibilityChange(val);

	}
}
