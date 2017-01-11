using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject wheelPrefab;

	void Awake(){
		SpawnEnemy();
	}

	void SpawnEnemy(){
		var go = Instantiate(wheelPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		go.transform.SetParent(transform, false);


	}



}
