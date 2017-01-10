using UnityEngine;
using System.Collections;

public class Grinder : MonoBehaviour {

	public string type;

	public bool IsInCameraBounds(){

		var viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);

		if (Mathf.Clamp(viewPortPosition.x, 0, 1) != viewPortPosition.x || Mathf.Clamp(viewPortPosition.y, 0, 1)!= viewPortPosition.y){
			return false;
		}

		return true;
	}
}
