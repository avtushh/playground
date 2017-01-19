using UnityEngine;
using System.Collections;
using System;

public class TweenStartRoundAnimation : MonoBehaviour {

	public RectTransform rectTrans;

	public float targetScale = 4f;
	public float animTime = 1.2f;

	public event Action CompleteEvent = () => {};

	void OnEnable(){
		Show();
	}

	public void Show () {
		LeanTween.scale(rectTrans, new Vector3(targetScale, targetScale, 1), animTime).setEase(LeanTweenType.easeOutElastic).setOnComplete(CompleteEvent);
	}



}
