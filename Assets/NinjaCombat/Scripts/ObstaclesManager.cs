using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ObstaclesManager : MonoBehaviour {

	public enum ObstacleType{
		None,BBTan, Wheels
	}

	public GameObject bbtnPrefab;
	public GameObject wheelsPrefab;


	public static ObstacleType initObstacleType = ObstacleType.BBTan;

	public ObstaclesGroup currentObstalcesGroup;
	public ObstacleType currentType = ObstacleType.None;

	public Dictionary<ObstacleType, GameObject> typeToPrefabDict = new Dictionary<ObstacleType, GameObject>();

	void Awake(){
		typeToPrefabDict.Add(ObstacleType.BBTan, bbtnPrefab);
		typeToPrefabDict.Add(ObstacleType.Wheels, wheelsPrefab);
	}

	ObstacleType GetNextType(){
		if (currentType == ObstacleType.None)
		{
			return initObstacleType;
		}else{
			return currentType == ObstacleType.BBTan?ObstacleType.Wheels:ObstacleType.BBTan;
		}
	}


	public void ActivateObstaclesGroupByType(ObstacleType obsType = ObstacleType.None){

		if (obsType == ObstacleType.None){
			obsType = GetNextType();
		}

		if (currentObstalcesGroup != null){
			Destroy(currentObstalcesGroup.gameObject);
		}

		var go = GameObject.Instantiate(typeToPrefabDict[obsType]);

		go.transform.SetParent(transform, false);

		currentObstalcesGroup = go.GetComponent<ObstaclesGroup>();

		if (currentObstalcesGroup != null){
			currentType = obsType;
		}


	}
		
	void Hide(){
		currentObstalcesGroup.gameObject.SetActive(false);
	}
}
