using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GRTileManager : MonoBehaviour {

	public List<GameObject> tilesPrefab;

	public GameObject tileGo;

	public Transform playerTransform;

	public Transform cameraSpawnPoint;

	EditableTerrain2D terrain2D;

	Vector3 maxPoint;

	// Use this for initialization
	void Start () {
		UpdatePoints ();
	}

	void Update () {
		
		if (maxPoint.x < cameraSpawnPoint.position.x){

			tileGo = GameObject.Instantiate(GetRandomPrefab(), maxPoint, Quaternion.identity) as GameObject;

			tileGo.transform.SetParent(transform);
			UpdatePoints();
		}
	}

	void UpdatePoints ()
	{
		var terrains = tileGo.GetComponentsInChildren<EditableTerrain2D> ();

		terrain2D = terrains[0];
		maxPoint = tileGo.transform.position + terrain2D.GetControlPoint (terrain2D.controlPoints.Count - 1);
		print(maxPoint);
		terrains.ToList().ForEach(x => {
			x.UpdateAllChunkMeshes();
		});
	}

	GameObject GetRandomPrefab(){
		return tilesPrefab[Random.Range(0, tilesPrefab.Count)];
	}
}
