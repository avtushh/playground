using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SlowDownJump))]
[RequireComponent(typeof(Rigidbody2D))]
public class SRPlayer : MonoBehaviour {

	public event Action OnJump = () => {};
	public event Action OnLand = () => {};
	public event Action OnDie = () => {};
	public event Action OnStartSlide = () => {};

	public float speed = 1;

	public float maxSpeed;
	public float minSpeed;

	Rigidbody2D _rigidBody;
	SlowDownJump _slowDownJump;

	public SpriteRenderer sprRenderer;
	public Sprite sprNormal, sprJump;

	public GameObject bloodParticles, trailParticles, grassParticles;

	public float minRotationAngle = -60f, maxRotationAngle = 90f;

	public float baseTimeScale = 0.6f;
	public float portraitTimeScale = 0.15f;

	public enum State{
		Idle, Sliding, Jumping, Dead
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



		_state = State.Idle;
		trailParticles.SetActive(false);


	}

	void StartSliding(){
		_state = State.Sliding;
		trailParticles.SetActive(true);

		var vel = _rigidBody.velocity;
		vel.x = speed;
		_rigidBody.velocity = vel;
		OnStartSlide();
	}



	void OnDestroy(){
		_slowDownJump.JumpEvent -= OnJumpEvent;
		_slowDownJump.LandEvent -= OnLandEvent;

		CameraViewListener.onVisibilityChange -= CameraViewListener_onVisibilityChange;
	}

	void CameraViewListener_onVisibilityChange (bool isVisible, string tag)
	{
		if (tag == "Enemy"){
			if (isVisible){
				numVisibleEnemies++;
			}else{
				numVisibleEnemies--;
			}

			if (numVisibleEnemies > 0){
				
			}
		}
	}

	int numVisibleEnemies;


	void Update(){

		switch(_state){
			case State.Idle:
				if (Input.GetMouseButtonDown(0)){
					StartSliding();
				}
				break;
			case State.Sliding:
				
				KeepMinXSpeed();
				BalanceRotation();

				break;
			case State.Jumping:
				
				if (numVisibleEnemies > 0 && Time.timeScale == 1){

					var scale = baseTimeScale;

					if (Screen.height > Screen.width){
						scale = portraitTimeScale;

					}else{
						scale = baseTimeScale - numVisibleEnemies * 0.1f;
					}

					_slowDownJump.TriggerSlowdown(scale);
				}
				break;
		}
	}

	void KeepMinXSpeed(){
		var vel = _rigidBody.velocity;
		if (vel.x < speed){
			vel.x = speed;
			_rigidBody.velocity = vel;
		}
	}

	void BalanceRotation(){
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

	void OnLandEvent (GameObject collidedWithObj)
	{
		if (_state != State.Jumping){
			return;
		}
		LeanTween.delayedCall(gameObject, 0.1f, ()=>trailParticles.SetActive(true));
		sprRenderer.sprite = sprNormal;
		_state = State.Sliding;
		grassParticles.SetActive(true);
		OnLand();
	}

	void OnJumpEvent ()
	{
		trailParticles.SetActive(false);
		sprRenderer.sprite = sprJump;
		_state = State.Jumping;
		grassParticles.SetActive(false);
		OnJump();
	}

	void Die(GameObject other){

		var grinder = other.gameObject.GetComponentInParent<Grinder>();

		_slowDownJump.ResetPhysics();

		_rigidBody.isKinematic = true;

		_rigidBody.velocity = Vector2.zero;

		trailParticles.SetActive(false);

		LeanTween.move(gameObject, other.transform.position, 0.1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>{
			grinder.OnKillPlayer();
			transform.SetParent(other.transform);
			FindObjectOfType<CameraFollow>().enabled = false;
		});

		_state = State.Dead;

		OnDie();
	}


	void OnTriggerEnter2D(Collider2D other) {

		if (_state == State.Dead){
			return;
		}

		switch(other.tag){
			case "Enemy":
				Die(other.gameObject);
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

	public void OnNoEnemies(){
		_slowDownJump.TriggerSlowdown(1f);
	}

}
