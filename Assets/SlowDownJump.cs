using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class SlowDownJump : MonoBehaviour {

	public event Action JumpEvent = () => {};
	public event Action<GameObject> LandEvent = (s) => {};

	public AnimationCurve animationYCurve;

	public LayerMask collisionMask;

	bool _isJumping;

	Rigidbody2D _rigidBody;

	bool _shouldSlowWhileJumping;
	bool _isSlowMotion;

	float _orgVelX; 

	void Awake(){
		_rigidBody = GetComponent<Rigidbody2D>();
		_orgGravityScale = _rigidBody.gravityScale;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Jumper"){
			if (!_isJumping){
				_orgVelX = _rigidBody.velocity.x;
				//_shouldSlowWhileJumping = true;
			}
			Jump();
		}
	}

	public void SetSlowDownFlag(){
		_shouldSlowWhileJumping = true;
	}

	void Update(){
		if (_isJumping){

			if (!_isSlowMotion && _shouldSlowWhileJumping){
				TriggerSlowdown(true);
				return;
			}
//				
//
//			var hit = Physics2D.Raycast(transform.position, Vector2.down, 10, collisionMask);
//			Debug.DrawRay(transform.position, Vector2.down *10, Color.red);
//
//			if (hit != null && hit.collider != null){
//				print("hit: " + hit.collider.gameObject.tag);
//
//				if (!_isSlowMotion && _shouldSlowWhileJumping){
//					if (hit.collider.gameObject.tag == "Enemy"){
//						TriggerSlowdown(true);
//					}
//				}else{
//					
//				}
//			}
		}
	}

	float _orgGravityScale;

	public void Jump ()
	{
		_rigidBody.gravityScale = 2;
		_rigidBody.velocity = new Vector2(_orgVelX / 2f, 15);
		_isJumping = true;
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
		TriggerSlowdown(false);

		_rigidBody.gravityScale = _orgGravityScale;
		var vel = _rigidBody.velocity;
		vel.x = _orgVelX;
		_rigidBody.velocity = vel;

	}

	public void TriggerSlowdown(bool val){
		_isSlowMotion = val;
		_shouldSlowWhileJumping = false;

		Time.timeScale = val?0.1f:1f;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}


	void OnCollisionEnter2D (Collision2D coll)
	{
		if (_isJumping){
			Land (coll);
		}
	}
}
