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

	public float minRotationAngle = -60f, maxRotationAngle = 90f;

	bool _isJumping;

	void Awake(){
		_rigidBody = GetComponent<Rigidbody2D>();
		_slowDownJump = GetComponent<SlowDownJump>();
	}

	// Use this for initialization
	void Start () {

		_slowDownJump.JumpEvent += OnJumpEvent;
		_slowDownJump.LandEvent += OnLandEvent;

		var vel = _rigidBody.velocity;

		vel.x = speed;

		_rigidBody.velocity = vel;
			

	}

	void Update(){
		
		if (!_isJumping){
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
		_isJumping = false;
		OnLand();
	}

	void OnJumpEvent ()
	{
		_isJumping = true;
		OnJump();
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Obstacle"){
			gameObject.SetActive(false);

			OnDie();
		}
	}

}
