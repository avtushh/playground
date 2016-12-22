using UnityEngine;
using System.Collections;
using System;

public class NinjaStar : MonoBehaviour {

	Rigidbody2D _rigidBody;
	public string tagToHit = "Player";

	void Awake () {
		_rigidBody = GetComponent<Rigidbody2D>();
	}

	public void Throw(float x, float y){
		//Debug.LogError("throw, velocity: " + x + "," + y);

		if (x < 0.1f && y < 1f){
			y = 5;
		}
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
				Debug.LogError("hit another bullet");

				Hit();
				other.gameObject.GetComponent<NinjaStar>().Hit();
				break;
			case "Obstacle":
				Debug.LogError("hit obstacle");
				Hit();
				break;
			
		}

	}

	public void Hit(){
		Destroy(gameObject);
	}

}
