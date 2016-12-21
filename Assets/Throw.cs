using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	Rigidbody2D _rigidBody;

	public float initThrowX = 20;
	public float initThrowY = 2;

	void Awake () {
		_rigidBody = GetComponent<Rigidbody2D>();
	}

	public void ThrowMe(float x, float y){
		Debug.LogError("throw");
		_rigidBody.velocity = new Vector2(x, y);
	}

	public void Activate(){
		gameObject.SetActive(true);
	}

	void OnDisable(){
		_rigidBody.velocity = Vector2.zero;
	}

}
