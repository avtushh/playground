using UnityEngine;
using System.Collections;

public class ObstacleUnit : MonoBehaviour {

	public GameObject particlesPrefab;

	public int initHitPoints = 1;

	int _hitpoints;

	public void Hit(){

		//_hitpoints--;

		if (_hitpoints == 0){
			gameObject.SetActive(false);
			Instantiate(particlesPrefab, transform.position, Quaternion.identity);
		}
	}

	void OnEnable(){
		_hitpoints = initHitPoints;
	}
}
