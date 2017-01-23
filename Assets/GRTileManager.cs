using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GRTileManager : MonoBehaviour {

	public List<GameObject> tilesPrefab;

	GameObject tileGo;

	public Transform playerTransform;

	public Transform cameraSpawnPoint;

	Vector3 maxPoint;

	int tilesSinceFirst = 0;

	void Start () {
		tileGo = transform.GetChild(0).gameObject;
		UpdatePoints ();
	}

	void Update () {
		
		if (maxPoint.x < cameraSpawnPoint.position.x){

			CreateNewTile ();

		}
	}

	void CreateNewTile ()
	{
		var randomPrefab = GetRandomPrefab ();


		tileGo = GameObject.Instantiate (randomPrefab, maxPoint, Quaternion.identity) as GameObject;
		tileGo.transform.SetParent (transform);
		UpdatePoints ();
	}

	void UpdatePoints ()
	{
		var terrains = tileGo.GetComponentsInChildren<EditableTerrain2D> ();

		maxPoint = GetRightmostControlPoint(terrains);
		terrains.ToList().ForEach(x => {
			x.UpdateAllChunkMeshes();
		});
	}

	Vector3 GetRightmostControlPoint(Terrain2D[] array){
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < array.Length; i++) {
			var terrain2D = array[i];
			//var rightPoint = terrain2D.GetControlPoint (terrain2D.controlPoints.Count - 1);
			var rightPoint = terrain2D.transform.position + terrain2D.GetRightmostPoint();
			if (rightPoint.x > maxPoint.x){
				maxPoint = rightPoint;
			}
		}

		return maxPoint;
	}

	GameObject GetRandomPrefab(){

		int randomIndex = Random.Range(0, tilesPrefab.Count);

		if (randomIndex != 0){
			tilesSinceFirst++;
			if (tilesSinceFirst == 3){
				randomIndex = 0;
			}
		}else{
			tilesSinceFirst = 0;
		}

		return tilesPrefab[randomIndex];
	}
}
