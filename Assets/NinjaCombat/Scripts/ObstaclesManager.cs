using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ObstaclesManager : MonoBehaviour {

	public enum ObstacleType{
		None, BBTan, Wheels, Paddles, BBTan2, Bottle, Wind
	}

	public GameObject bbtnPrefab, bbtn2Prefab;
	public GameObject wheelsPrefab;
	public GameObject paddlesPrefab;
	public GameObject bottlePrefab;
	public GameObject windPrefab;

	public static ObstacleType initObstacleType = ObstacleType.Paddles;

	public ObstaclesGroup currentObstalcesGroup;
	public ObstacleType currentType = ObstacleType.None;

	public Dictionary<ObstacleType, GameObject> typeToPrefabDict = new Dictionary<ObstacleType, GameObject>();

	void Awake(){
		typeToPrefabDict.Add(ObstacleType.BBTan, bbtnPrefab);
		typeToPrefabDict.Add(ObstacleType.Wheels, wheelsPrefab);
		typeToPrefabDict.Add(ObstacleType.Paddles, paddlesPrefab);
		typeToPrefabDict.Add(ObstacleType.BBTan2, bbtn2Prefab);
		typeToPrefabDict.Add(ObstacleType.Bottle, bottlePrefab);
		typeToPrefabDict.Add(ObstacleType.Wind, windPrefab);
	}

	ObstacleType GetNextType(){
		if (currentType == ObstacleType.None)
		{
			return initObstacleType;
		}else{
			return currentType.Next(true);
		}
	}

	public void ActivateObstaclesGroupByType(){
		var obsTypeNext = GetNextType();

		var obsTypeRandom = EnumUtils.RandomEnumValue<ObstacleType>(true);

		if (currentType == obsTypeRandom){
			currentType = obsTypeNext;
		}else{
			currentType = obsTypeRandom;
		}


		if (currentObstalcesGroup != null){
			Destroy(currentObstalcesGroup.gameObject);
		}

		var go = GameObject.Instantiate(typeToPrefabDict[currentType]);

		go.transform.SetParent(transform, false);

		currentObstalcesGroup = go.GetComponent<ObstaclesGroup>();
	}
		
	void Hide(){
		currentObstalcesGroup.gameObject.SetActive(false);
	}
}
