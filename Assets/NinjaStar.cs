using UnityEngine;
using System.Collections;
using System;

public static class NinjaTags{
	public const string Player = "Player";
	public const string Enemy = "Enemy";
}

public class NinjaStar : MonoBehaviour {

	Rigidbody2D _rigidBody;
	public Sprite fireballSprite;
	public SpriteRenderer spriteRenderer;
	public Sprite sprBlack, sprRed;
	public string tagToHit = NinjaTags.Player;

	public Collider2D activeCollider;

	bool _isFireball;

	public bool IsFireball {
		get{
			return _isFireball;
		}

		set{
			_isFireball = value;
			if (_isFireball){
				spriteRenderer.sprite = fireballSprite;
			}
		}
	}

	void Awake () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.angularVelocity = 800f;
		IsFireball = false;

		activeCollider.enabled = false;

		LeanTween.delayedCall(gameObject, 0.2f, () => activeCollider.enabled = true);
	}

	public void SetTarget(string targetTag){
		tagToHit = targetTag;

		if (IsPlayerStar){
			spriteRenderer.sprite = sprBlack;
		}else{
			spriteRenderer.sprite = sprRed;
		}
	}

	public bool IsPlayerStar{
		get{
			return tagToHit == NinjaTags.Enemy;
		}
	}

	public void Throw(Vector2 velocity){
		//Debug.LogError("throw, velocity: " + x + "," + y);

		_rigidBody.velocity = velocity;

	}

	public void ThrowRandomDirection(float speed){

		Vector2 velocity = new Vector2(0, speed);

		var angle = UnityEngine.Random.Range(-70, 70);

		_rigidBody.velocity = Quaternion.Euler (0, 0, angle) * velocity;

		//Debug.LogError("Throw to: " + _rigidBody.velocity);
	}

	void OnDisable(){
		_rigidBody.velocity = Vector2.zero;
	}

	void OnCollisionEnter2D(Collision2D other){

		switch(other.gameObject.tag){
			case "Bullet":
				var otherStar = other.gameObject.GetComponent<NinjaStar>();

				if (otherStar.tagToHit != tagToHit){
					//Debug.LogError("hit another bullet");
					Hit();	
				}

				break;
			case "Shield":
				other.gameObject.SetActive(false);
				Hit();
				break;
			
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!IsFireball){
			return;
		}

		switch(other.gameObject.tag){
			case "Obstacle":
				other.gameObject.SetActive(false);
				break;
			case "Bullet":
				var otherStar = other.gameObject.GetComponent<NinjaStar>();

				if (otherStar.tagToHit != tagToHit){
					//Debug.LogError("hit another bullet");
					otherStar.Hit();
				}
				break;
		}
	}

	void OnCollisionExit2D(Collision2D other){

		switch(other.gameObject.tag){
			case "Obstacle":
				//Debug.LogError("hit obstacle");
				other.gameObject.SetActive(false);
				Hit();

//				if (_rigidBody.velocity.magnitude < 5){
//					_rigidBody.velocity *= 2;
//				}


				break;

		}
	}

	public void Hit(){
		
		Destroy(gameObject);
	}

	public void Split ()
	{
		DuplicateStarAtAngle (-65);
		DuplicateStarAtAngle (90);
		DuplicateStarAtAngle (65);
	}

	public void DuplicateStarAtAngle (float angle)
	{
		var go = Instantiate (gameObject, transform.position, Quaternion.identity) as GameObject;
		var star = go.GetComponent<NinjaStar> ();
		star.IsFireball = IsFireball;
		star.SetTarget(star.tagToHit);
		Vector2 velocity = Quaternion.Euler (0, angle, 0) * _rigidBody.velocity;
		star.Throw (velocity);
	}
}
