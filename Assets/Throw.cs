using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	Rigidbody2D _rigidBody;

	public float initThrowX = 20;
	public float initThrowY = 2;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();

		ThrowMe(initThrowX, initThrowY);
	}

	public void ThrowMe(float x, float y){
		_rigidBody.velocity = new Vector2(x, y);
	}
}
