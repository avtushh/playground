using UnityEngine;
using System.Collections;
using System;

public class SlowDownJump : MonoBehaviour {

	public event Action JumpEvent = () => {};
	public event Action<GameObject> LandEvent = (s) => {};

	public LayerMask collisionMask;

	bool _isJumping;

	Rigidbody2D _rigidBody;

	Vector3 _jumpPos;
	bool _shouldSlowWhileJumping;
	bool _isSlowMotion;

	float _startSlowHeight = 2;
	float _finishSlowHeight = 2;

	float _orgVelX; 

	void Awake(){
		_rigidBody = GetComponent<Rigidbody2D>();
		_orgGravityScale = _rigidBody.gravityScale;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Jumper"){
			if (!_isJumping){
				_orgVelX = _rigidBody.velocity.x;
				_shouldSlowWhileJumping = true;
			}
			Jump();

		}
	}

	void Update(){
		
		if (_isJumping){

			if (!_isSlowMotion && _shouldSlowWhileJumping){
				
				if (_rigidBody.velocity.y > 0 && Mathf.Abs(transform.position.y - _jumpPos.y) > _startSlowHeight){
					TriggerSlowdown(true);
				}
			}else{
				if (_rigidBody.velocity.y < 0){
					Debug.DrawRay(transform.position, Vector2.down *10, Color.red);
					var hit = Physics2D.Raycast(transform.position, Vector2.down, 10, collisionMask);

					if (hit != null && hit.collider != null){
						print("hit: " + hit.collider.gameObject.tag);
						if (hit.collider.gameObject.tag == "Platform"){
							if (Mathf.Abs(transform.position.y - _jumpPos.y) < _finishSlowHeight){
								//TriggerSlowdown(false);			
							}
						}
					}
				}
			}
		}
	}

	float _orgGravityScale;

	public void Jump ()
	{
		Time.timeScale = 1;

		_rigidBody.gravityScale = 2;
		var vel = _rigidBody.velocity;
		vel.y = 15f;
		if (_shouldSlowWhileJumping)
			vel.x = _orgVelX / 2f ;
		else
			vel.x = _orgVelX;
		
		_rigidBody.velocity = vel;
		_isJumping = true;
		_jumpPos = transform.position;
		JumpEvent ();
	}



	void Land (Collision2D coll)
	{
		ResetPhysics ();
		_isJumping = false;
		LandEvent (coll.gameObject);
	}

	public void ResetPhysics ()
	{
		_isSlowMotion = false;
		_rigidBody.gravityScale = _orgGravityScale;
		Time.timeScale = 1;
		var vel = _rigidBody.velocity;
		vel.x = _orgVelX;
		_rigidBody.velocity = vel;
	}

	void TriggerSlowdown(bool val){
		_isSlowMotion = val;
		_shouldSlowWhileJumping = false;

		Time.timeScale = val?0.3f:1f;
	}


	void OnCollisionEnter2D (Collision2D coll)
	{
		if (_isJumping){
			Land (coll);

		}
	}
}
