using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ObstaclesManager : MonoBehaviour {

	public enum ObstacleType{
		None,BBTan, Wheels
	}

	public List<ObstaclesGroup> obstaclesList;

	public static ObstacleType initObstacleType = ObstacleType.BBTan;

	public ObstaclesGroup currentObstalcesGroup;
	public ObstacleType currentType = ObstacleType.None;

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

			if (currentObstalcesGroup.obstacleType == obsType){
				currentObstalcesGroup.gameObject.SetActive(true);
			}else{
				currentObstalcesGroup.gameObject.SetActive(false);
			}
		}

		currentObstalcesGroup = obstaclesList.FirstOrDefault(x => x.obstacleType == obsType);
		if (currentObstalcesGroup != null){
			currentObstalcesGroup.gameObject.SetActive(true);
			currentType = obsType;

			if (currentType == ObstacleType.BBTan){
				//ObstacleUnit.ActivateAll();
			}
		}


	}
		
	void Hide(){
		currentObstalcesGroup.gameObject.SetActive(false);
	}
}
