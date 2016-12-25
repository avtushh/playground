using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public float activeTime = 5f;

	SpriteRenderer _shieldSprite;

	void OnEnable(){
		LeanTween.delayedCall(gameObject, activeTime, ()=>gameObject.SetActive(false));
		_shieldSprite = GetComponent<SpriteRenderer>();
	}

	void Update(){
		if (_shieldSprite != null){
			_shieldSprite.color = Random.ColorHSV();
		}
	}

	void OnDisable(){
		LeanTween.cancel(gameObject);
	}
}
