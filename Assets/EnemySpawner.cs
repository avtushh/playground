using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TabTale
{
	public class EnemySpawner : MonoBehaviour
	{
		public List<GameObject> prefabs;
	
		public Shape forcedShape = Shape.None;

		void Start ()
		{
			SpawnEnemy ();
			//SpawnEnemy();
		}

		void SpawnEnemy ()
		{
	
			int index = Random.Range (0, prefabs.Count);
	
			//index = 2;
	
	
			var go = Instantiate (prefabs [index], Vector3.zero, Quaternion.identity) as GameObject;
	
			go.transform.SetParent (transform, false);

			go.GetComponent<GrinderGroup>().forcedShape = forcedShape;
		}
	}
}
