using UnityEngine;
using System.Collections;
using System;

public class Ball : MonoBehaviour
{
	public event Action OnBallLaunch = () => {};
	public event Action OnBallTouchFloor = () => {};

	public GameObject particles; 

	public float ballInitialVelocity = 600f;

	public bool IsAttached {
		get;
		set;
	}

	Rigidbody2D _rigidBody;
	Vector3 _orgPosition;
	SpriteRenderer _renderer;

	bool _isSlow = false;
	bool _IsInPlay = false;

	void Start ()
	{
		_orgPosition = transform.position;
		_rigidBody = GetComponent<Rigidbody2D> ();
		_renderer = GetComponent<SpriteRenderer>();
		Reset ();
	}

	public void Reset ()
	{
		_IsInPlay = false;
		gameObject.SetActive (true);
		_renderer.enabled = true;
		IsAttached = true;

		_rigidBody.isKinematic = true;
		transform.position = _orgPosition;
	}

	void Update ()
	{
		if (IsAttached && Input.GetMouseButtonDown (0)) {
			Launch ();
		}
	}

	void Launch ()
	{
		_IsInPlay = true;
		IsAttached = false;
		_rigidBody.isKinematic = false;
		_rigidBody.AddForce (new Vector2 (ballInitialVelocity, ballInitialVelocity));
		_rigidBody.angularVelocity = 100f;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (_IsInPlay && other.CompareTag ("Floor")) {
			_IsInPlay = false;
			Fail ();
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		float newX = _rigidBody.velocity.x;
		float newY = _rigidBody.velocity.y;

		float minXVel = 1;
		float minYVel = 2;

		if (_rigidBody.velocity.x < minXVel && _rigidBody.velocity.x >= 0) {
			newX = minXVel;
		} else if (_rigidBody.velocity.x > -minXVel && _rigidBody.velocity.x < 0) {
			newX = -minXVel;
		}

		if (_rigidBody.velocity.y < minYVel && _rigidBody.velocity.y >= 0) {
			newY = minYVel;
		} else if (_rigidBody.velocity.y > -minYVel && _rigidBody.velocity.y < 0) {
			newY = -minYVel;
		}

		_rigidBody.velocity = new Vector2 (newX, newY);
	}

	void Fail ()
	{
		Debug.LogError ("Fail!");
		StopAllCoroutines ();
			
		_renderer.enabled = false;
		_rigidBody.isKinematic = true;
		_rigidBody.velocity = Vector2.zero;
		Instantiate (particles, transform.position, Quaternion.identity);
			
		LeanTween.delayedCall (1f, OnBallTouchFloor);
	}

	public void ToggleSlowMode (bool toggle)
	{
		if (toggle && !_isSlow) {
			_isSlow = true;
			_rigidBody.velocity *= 0.5f;
			LeanTween.delayedCall (20f, () => ToggleSlowMode (false));
		} else if (_isSlow && !toggle) {
			_isSlow = false;
			_rigidBody.velocity *= 2f;
		}
	}
}

