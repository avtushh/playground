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
			var color =  Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 0.5f));;
			_shieldSprite.color = color;
		}
	}

	void OnDisable(){
		LeanTween.cancel(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("Bullet")){

			var star = other.gameObject.GetComponent<NinjaStar>();

			if (!star.IsPlayerStar)
				gameObject.SetActive(false);
			
		}
	}
}
