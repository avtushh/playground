using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TabTale
{
	public class SlowDownJump : MonoBehaviour
	{

		public event Action JumpEvent = () => {};
		public event Action<GameObject> LandEvent = (s) => {};

		public LayerMask collisionMask;

		Rigidbody2D _rigidBody;

		bool _isSlowMotion;

		float _orgVelX;
		float _orgGravityScale;

		void Awake ()
		{
			_rigidBody = GetComponent<Rigidbody2D> ();
			_orgGravityScale = _rigidBody.gravityScale;
			targetScale = Time.timeScale;
		}

		public void Jump (float landVel, float velX = 7f, float velY = 10f, float gravityScale = 2f)
		{
			TriggerSlowdown(1f);
			_orgVelX = landVel;
			_rigidBody.gravityScale = gravityScale;
			_rigidBody.velocity = new Vector2 (velX, velY);

			LeanTween.delayedCall (0.2f, () => JumpEvent ());
		}

		public void Land (Collision2D coll, bool resetTime = true)
		{
			ResetPhysics (resetTime);

			LandEvent (coll.gameObject);
		}

		public void ResetPhysics (bool resetTime = true)
		{
			print ("reset physics");
			if (resetTime)
				TriggerSlowdown (1);

			_rigidBody.isKinematic = false;

			_rigidBody.gravityScale = _orgGravityScale;
			var vel = _rigidBody.velocity;
			vel.x = _orgVelX;
			_rigidBody.velocity = vel;
		}

		public void Freeze ()
		{
			_rigidBody.gravityScale = 0;
			_rigidBody.velocity = new Vector2 (0, 0);
			TriggerSlowdown (1);
		}
			
		public void TriggerSlowdown (float scale)
		{
//			_rigidBody.gravityScale /= (lastScale*lastScale);
//			_rigidBody.velocity /= lastScale;
//
//			_rigidBody.gravityScale *= (scale*scale);
//			_rigidBody.velocity *= scale;
//			lastScale = scale;
			if (targetScale != scale)
				Debug.LogWarning("trigger slow down: " + scale);
			targetScale = scale;

		}

		float currVel;
		float targetScale = 1;

		void Update ()
		{
			if (Time.timeScale != targetScale) {
				Time.timeScale = Mathf.SmoothDamp (Time.timeScale, targetScale, ref currVel, 0.01f);
				Time.fixedDeltaTime = 0.02F * Time.timeScale;
			}
		}
	}
}