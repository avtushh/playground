using UnityEngine;
using System.Collections;
using System;

public class DetectStarCollision : MonoBehaviour {

	public Collider2D col;

	public event Action<NinjaStar> StarComingEvent = (s) => {};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet"){
			var star = other.GetComponent<NinjaStar>();

			if (star.IsPlayerStar && star.IsActive){
				StarComingEvent(star);
			}
		}

	}
}
