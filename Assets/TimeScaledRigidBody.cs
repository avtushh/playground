using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class TimeScaledRigidBody : MonoBehaviour {

	float _timeScale;

	Rigidbody2D _rigidBody;

	void Awake(){
		_rigidBody = GetComponent<Rigidbody2D>();
		_timeScale = 1f;
	}

	public float timeScale {
		get { return _timeScale; }
		set {
			value = Mathf.Abs(value);
			float tempTimeScaleApplied = value/timeScale;
			_timeScale = value;

			_rigidBody.gravityScale = 0;
			_rigidBody.velocity *= tempTimeScaleApplied;
			_rigidBody.angularVelocity *= tempTimeScaleApplied;
		}
	}
}
