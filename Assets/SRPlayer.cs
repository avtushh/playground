using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabTale
{
	[RequireComponent (typeof(SlowDownJump))]
	[RequireComponent (typeof(Rigidbody2D))]
	public class SRPlayer : MonoBehaviour
	{

		public event Action OnLand = () => {};
		public event Action OnDie = () => {};

		public event Action OnSlowDown = () => {};
		public event Action OnStartSlide = () => {};
		public event Action OnDanger = () => {};

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

		List<Transform> shapes = new List<Transform>();

		public enum State
		{
			Idle,
			Sliding,
			Jumping,
			Dead
		}

		State _state;

		public State CurrState{
			get{
				return _state;
			}
		}

		bool _isInDanger;

		void Awake ()
		{
			_rigidBody = GetComponent<Rigidbody2D> ();
			_slowDownJump = GetComponent<SlowDownJump> ();

			sprRenderer.sprite = sprNormal;
		}

		void Start ()
		{

			_slowDownJump.JumpEvent += OnJumpEvent;
			_slowDownJump.LandEvent += OnLandEvent;

			CameraViewListener.onVisibilityChange += CameraViewListener_onVisibilityChange;

			_state = State.Idle;
			trailParticles.SetActive (false);
		}

		void StartSliding ()
		{
			_state = State.Sliding;
			trailParticles.SetActive (true);

			var vel = _rigidBody.velocity;
			vel.x = speed;
			_rigidBody.velocity = vel;
			OnStartSlide ();
		}



		void OnDestroy ()
		{
			_slowDownJump.JumpEvent -= OnJumpEvent;
			_slowDownJump.LandEvent -= OnLandEvent;

			CameraViewListener.onVisibilityChange -= CameraViewListener_onVisibilityChange;
		}

		void CameraViewListener_onVisibilityChange (bool isVisible, GameObject obj)
		{
			if (obj.tag == GrindMeTags.Shape) {
				if (isVisible) {
					shapes.Add(obj.transform);
				} else {
					shapes.Remove(obj.transform);
				}
			}else if (obj.tag == GrindMeTags.Jumper){
				hasJumper = isVisible;
			}
		}

		bool hasJumper = false;

		void Update ()
		{
			switch (_state) {
				case State.Idle:
					if (Input.GetMouseButtonDown (0)) {
						StartSliding ();
					}
					break;
				case State.Sliding:
				
					KeepMinXSpeed ();
					BalanceRotation ();
					if (!hasJumper)
						CheckSlowDown();
					break;
				case State.Jumping:
					CheckSlowDown();
					break;
			}
		}

		public class ClosestShape{
			public ShapeDataComponent shapeData; 
			public float distance;
			public GrinderGroup grinderGroup;
		}

		public ClosestShape GetClosestEnemy(string shapeName = null){

			ClosestShape closestEnemy = null;
			float minDistance = Mathf.Infinity;

			shapes.ForEach(shape => {

				if (shape != null && shape.transform.position.x > transform.position.x - 3){

					//if (shape.transform.position.y <= transform.position.y && _rigidBody.velocity.y <= 0 || shape.transform.position.y > transform.position.y && _rigidBody.velocity.y > 0){
						var dist = Vector3.Distance(shape.transform.position, transform.position);

						if (dist < minDistance)	{

							var shapeData = shape.GetComponentInChildren<ShapeDataComponent>();

							if (shapeName == null || shapeData.Type == shapeName){
								if (closestEnemy == null){
									closestEnemy = new ClosestShape();
								}
								closestEnemy.shapeData = shapeData;
								minDistance = dist;
							}
						}	
					//}
				}
			});

			if (closestEnemy != null){
				closestEnemy.distance = minDistance;
				closestEnemy.grinderGroup = closestEnemy.shapeData.GetComponentInParent<GrinderGroup>();
			}

			return closestEnemy;
		}

		public float closetsEnemyDistance;

		void CheckSlowDown(){
			if (shapes.Count > 0) {

				var closetsEnemy = GetClosestEnemy();
				if (closetsEnemy != null){
					closetsEnemyDistance = closetsEnemy.distance;

					var slowDownDistance = 7.8f;
					var dangerDistance = 8f;

					if (closetsEnemyDistance < slowDownDistance){
						TriggerSlowDownMode (GetSlowDownScale (closetsEnemy));
						OnDanger();
						_isInDanger = true;
					}else if (closetsEnemyDistance < dangerDistance){
						TriggerSlowDownMode (1f);
						OnDanger();
						_isInDanger = true;
					}else{
						TriggerSlowDownMode (1f);
						_isInDanger = false;
					}
				}
			}else{
				if (Time.timeScale < 1){
					TriggerSlowDownMode(1f);
					sprRenderer.sprite = sprNormal;
				}
			}
		}



		float GetSlowDownScale (ClosestShape closestEnemy)
		{
			if(closestEnemy.grinderGroup == null){ // not a grinder, a ramp or a crate
				return 0.4f;
			}

			if (closestEnemy.distance < 5){
				return 0.15f;
			}
			var slowScale = 0.2f;
			if (_state == State.Sliding) {
				
				if (closestEnemy.grinderGroup != null && closestEnemy.grinderGroup.forcedShape == Shape.line) {
					slowScale = 0.7f;
				}
				else {
					slowScale = 0.2f;
				}
			}
			return slowScale;
		}

		void TriggerSlowDownMode (float scale)
		{
			
			sprRenderer.sprite = scale == 1?sprNormal:sprJump;
			_slowDownJump.TriggerSlowdown (scale);

			trailParticles.SetActive (scale == 1);

			if (scale < 1)
				OnSlowDown ();
		}

		void KeepMinXSpeed ()
		{
			var vel = _rigidBody.velocity;
			if (vel.x < speed) {
				vel.x = speed;
			}else if (vel.x > maxSpeed){
				vel.x = maxSpeed;
			}

			_rigidBody.velocity = vel;

		}

		void BalanceRotation ()
		{
			var quaternion = transform.rotation;

			var rotation = quaternion.eulerAngles;

			if (rotation.z > maxRotationAngle && rotation.z < 200) {
				rotation.z = maxRotationAngle;
			} else if (rotation.z > 200 && rotation.z < 360 - maxRotationAngle) {

				rotation.z = 360 - maxRotationAngle;
			}

			quaternion.eulerAngles = rotation;

			transform.rotation = quaternion;
		}

		void OnLandEvent (GameObject collidedWithObj)
		{
			if (_state != State.Jumping) {
				return;
			}

			trailParticles.SetActive (true);
			sprRenderer.sprite = sprNormal;
			_state = State.Sliding;
			grassParticles.SetActive (true);
			OnLand ();
		}

		void OnJumpEvent ()
		{
			trailParticles.SetActive (false);

			_state = State.Jumping;
			grassParticles.SetActive (false);


		}

		void Die (GameObject other)
		{
			var grinder = other.gameObject.GetComponentInParent<Grinder> ();

			_slowDownJump.ResetPhysics ();

			_rigidBody.isKinematic = true;

			_rigidBody.velocity = Vector2.zero;

			trailParticles.SetActive (false);

			LeanTween.move (gameObject, other.transform.position, 0.1f).setEase (LeanTweenType.easeInOutSine).setOnComplete (() => {
				grinder.OnKillPlayer ();
				transform.SetParent (other.transform);
				FindObjectOfType<CameraFollow> ().enabled = false;
			});

			_state = State.Dead;



			OnDie ();
		}


		void OnTriggerEnter2D (Collider2D other)
		{
			if (_state == State.Dead) {
				return;
			}

			switch (other.tag) {
				case GrindMeTags.Enemy:
					if (!other.GetComponentInParent<Grinder>().isAlive)
						return;

					SoundManager2.PlayHitSound();
					Die (other.gameObject);
					break;
				case GrindMeTags.Jumper:
					SoundManager2.PlayBoing();
					var ramp = other.GetComponent<Ramp>();
					ramp.DestroyShape();
					_slowDownJump.Jump(speed, ramp.velX, ramp.velY, ramp.gravityScale);
					break;
			}

		}

		void OnCollisionEnter2D (Collision2D coll)
		{
			switch (_state) {
				case State.Jumping:
					if (coll.gameObject.tag == GrindMeTags.Platform){
						_slowDownJump.Land (coll, !_isInDanger);
					}else if (coll.gameObject.tag == GrindMeTags.Enemy){
						if (coll.gameObject.GetComponentInParent<ShapeHolder>().isAlive)
							_slowDownJump.Land(coll);
					}
					break;
			}

		}
	}
}