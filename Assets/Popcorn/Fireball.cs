using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	public int speed = 100;

	void Start () {
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag("Wall")){
			Destroy(gameObject);
		}
	}
}
