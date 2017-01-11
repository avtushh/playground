using UnityEngine;
using System.Collections;

public class TileSpawner : MonoBehaviour {

	public GameObject tilePrefab;

	public Vector2 tileOffset;

	public Transform container;

	public Vector2 nextSpawnPosition;

	public DetectPlatformEdge platformEdge;

	void Start(){
		nextSpawnPosition = tileOffset;

		platformEdge.OnCollideWithEdge += OnCollideWithEdge;

	}

	void OnDestroy(){
		platformEdge.OnCollideWithEdge -= OnCollideWithEdge;
	}

	void OnCollideWithEdge ()
	{
		CreateTile();
		nextSpawnPosition += tileOffset;
	}

	void CreateTile(){

		Vector3 pos = new Vector3(nextSpawnPosition.x, nextSpawnPosition.y, 0);

		var go = Instantiate(tilePrefab, pos, Quaternion.identity) as GameObject;

		go.transform.SetParent(container, false);

	}



}
