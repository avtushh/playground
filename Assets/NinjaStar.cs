using UnityEngine;
using System.Collections;
using System;

public class NinjaStar : MonoBehaviour {

	Rigidbody2D _rigidBody;
	public string tagToHit = "Player";

	void Awake () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.angularVelocity = 800f;
	}

	public bool IsPlayerStar{
		get{
			return tagToHit == "Enemy";
		}
	}

	public void Throw(float x, float y){
		//Debug.LogError("throw, velocity: " + x + "," + y);

		_rigidBody.velocity = new Vector2(x, y);

	}

	public void Activate(){
		gameObject.SetActive(true);
	}

	void OnDisable(){
		_rigidBody.velocity = Vector2.zero;
	}

	void OnCollisionEnter2D(Collision2D other){

		switch(other.gameObject.tag){
			case "Bullet":
				var otherStar = other.gameObject.GetComponent<NinjaStar>();

				if (otherStar.tagToHit != tagToHit){
					Debug.LogError("hit another bullet");
					Hit();
				}

				break;
			case "Shield":
				other.gameObject.SetActive(false);
				Hit();
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

}
