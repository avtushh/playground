using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public List<GameObject> prefabs;

	void Awake(){
		SpawnEnemy();
	}

	void SpawnEnemy(){

		int index = Random.Range(0, prefabs.Count);

		var go = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity) as GameObject;

		go.transform.SetParent(transform, false);


	}



}
