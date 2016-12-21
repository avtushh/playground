using UnityEngine;
using System.Collections;

public class DelayedDisable : MonoBehaviour {

	public int forceFieldTime = 10;

	void OnEnable(){
		LeanTween.delayedCall(gameObject, forceFieldTime, () => gameObject.SetActive(false));
	}



	void OnDisable(){
		LeanTween.cancel(gameObject);
	}

}
