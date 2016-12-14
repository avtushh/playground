using UnityEngine;
using System.Collections;

public class JumpUp : MonoBehaviour {

    Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            rigidBody.velocity = new Vector2(0, 8);
        }
	}
}
