using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SlowDownJump))]
[RequireComponent(typeof(Rigidbody2D))]
public class SRPlayer : MonoBehaviour {

	public event Action OnJump = () => {};
	public event Action OnLand = () => {};
	public event Action OnDie = () => {};

	public float speed = 1;

	public float maxSpeed;
	public float minSpeed;

	Rigidbody2D _rigidBody;
	SlowDownJump _slowDownJump;

	public SpriteRenderer sprRenderer;
	public Sprite sprNormal, sprJump;

	public float minRotationAngle = -60f, maxRotationAngle = 90f;

	public enum State{
		Sliding, Jumping, Dead
	}

	State _state;

	void Awake(){
		_rigidBody = GetComponent<Rigidbody2D>();
		_slowDownJump = GetComponent<SlowDownJump>();

		sprRenderer.sprite = sprNormal;
	}

	// Use this for initialization
	void Start () {

		_slowDownJump.JumpEvent += OnJumpEvent;
		_slowDownJump.LandEvent += OnLandEvent;

		CameraViewListener.onVisibilityChange += CameraViewListener_onVisibilityChange;

		var vel = _rigidBody.velocity;

		vel.x = speed;

		_rigidBody.velocity = vel;

		_state = State.Sliding;
	}

	void OnDestroy(){
		_slowDownJump.JumpEvent -= OnJumpEvent;
		_slowDownJump.LandEvent -= OnLandEvent;

		CameraViewListener.onVisibilityChange -= CameraViewListener_onVisibilityChange;
	}

	void CameraViewListener_onVisibilityChange (bool isVisible, string tag)
	{
		if (tag == "Enemy"){

			print("Enemy visibility change: " + isVisible);

			if (isVisible){
				_slowDownJump.TriggerSlowdown(true);	
			}
		}
	}


	void Update(){
		
		if (_state == State.Sliding){
			var vel = _rigidBody.velocity;
			if (vel.x < speed){
				vel.x = speed;
				_rigidBody.velocity = vel;
			}

			var quaternion = transform.rotation;

			var rotation = quaternion.eulerAngles;

			if (rotation.z > maxRotationAngle && rotation.z < 200){
				rotation.z = maxRotationAngle;
			}else if (rotation.z > 200 && rotation.z < 360-maxRotationAngle){

				rotation.z = 360-maxRotationAngle;
			}

			quaternion.eulerAngles = rotation;

			transform.rotation = quaternion;
		}
	}

	void OnLandEvent (GameObject collidedWithObj)
	{
		if (_state != State.Jumping){
			return;
		}

		sprRenderer.sprite = sprNormal;
		_state = State.Sliding;
		OnLand();
	}

	void OnJumpEvent ()
	{
		sprRenderer.sprite = sprJump;
		_state = State.Jumping;
		OnJump();
	}


	void OnTriggerEnter2D(Collider2D other) {

		if (_state == State.Dead){
			return;
		}

		switch(other.tag){
			case "Enemy":
				gameObject.transform.position = other.gameObject.transform.position;
				_slowDownJump.Freeze();
				_state = State.Dead;
				OnDie();

				break;
			case"Jumper":
				_slowDownJump.Jump();
				break;
		}

	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		switch(_state){
			case State.Jumping:
				if (coll.gameObject.tag == "Platform" || coll.gameObject.tag == "Enemy")
					_slowDownJump.Land (coll);
				break;
		}

	}

}
