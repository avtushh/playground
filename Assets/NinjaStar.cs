using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class NinjaTags{
	public const string Player = "Player";
	public const string Enemy = "Enemy";
}

public class NinjaStar : MonoBehaviour {
	public float GetYVelocity ()
	{
		return _rigidBody.velocity.y;
	}

	Rigidbody2D _rigidBody;
	public Sprite fireballSprite;
	public SpriteRenderer spriteRenderer;
	public Sprite sprBlack, sprRed;
	public string tagToHit = NinjaTags.Player;
	public bool isGrounded;
	public Collider2D normalCollider;

	public GameObject fireBall;

	bool _isFireball;

	Vector3 _orgScale;

	public bool IsFireball {
		get{
			return _isFireball;
		}

		set{
			_isFireball = value;
			if (_isFireball){
				spriteRenderer.sprite = fireballSprite;
			}
			fireBall.SetActive(value);
		}
	}

	void Awake () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_orgScale = transform.localScale;
	}

	void Update(){

		if (NinjaGameManager.isPaused){
			return;
		}

		if (!isGrounded && _rigidBody.velocity.magnitude < 0.5f && transform.parent == null){
			ThrowRandomDirection(10);
		}
	}

	public void SetTarget(string targetTag){
		tagToHit = targetTag;

		if (IsFireball)
			return;

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
			case "Obstacle":
				//Debug.LogError("hit obstacle");

				other.gameObject.GetComponent<ObstacleUnit>().Hit();
				//Hit();
				break;
			
		}
	}



	void OnCollisionExit2D(Collision2D other){
		
		switch(other.gameObject.tag){
			case "Obstacle":
				//other.gameObject.GetComponent<ObstacleUnit>().Hit();
				//Hit();
				break;
		}
	}

	public void Hit(){
		
		Destroy(gameObject);
	}

	public void Split ()
	{
		DuplicateStarAtAngle (90);
		DuplicateStarAtAngle (65);
		DuplicateStarAtAngle (-65);
		Hit();
	}

	public void DuplicateStarAtAngle (float angle)
	{
		var go = Instantiate (gameObject, transform.position, Quaternion.identity) as GameObject;
		var star = go.GetComponent<NinjaStar> ();
		star.IsFireball = IsFireball;
		star.isGrounded = false;
		star.SetTarget(star.tagToHit);
		Vector2 velocity = Quaternion.Euler (0, angle, 0) * _rigidBody.velocity;
		star.Throw (velocity);
	}

	public void HitGround ()
	{
		_rigidBody.velocity = Vector2.zero;
		_rigidBody.angularVelocity = 0;

		isGrounded = true;

		normalCollider.isTrigger = true;

		LeanTween.moveY (gameObject, transform.position.y * 1.005f, 0.1f).setEase (LeanTweenType.easeInOutSine).setLoopPingPong ();
	}

	public void Pickup ()
	{
		_rigidBody.velocity = Vector2.zero;
		_rigidBody.angularVelocity = 0;

		isGrounded = false;
		normalCollider.enabled = false;
		LeanTween.cancel(gameObject);
	}

	public void Throw(Vector2 velocity){
		transform.parent = null;
		_rigidBody.velocity = velocity;
		_rigidBody.angularVelocity = 800f;

		normalCollider.enabled = true;
		normalCollider.isTrigger = false;
		transform.localScale = _orgScale;
	}

	public void ThrowRandomDirection(float speed){

		Vector2 velocity = new Vector2(0, speed);

		var angle = UnityEngine.Random.Range(-70, 70);

		Throw(Quaternion.Euler (0, 0, angle) * velocity);
	}


}

