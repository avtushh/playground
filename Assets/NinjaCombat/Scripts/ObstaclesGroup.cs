using UnityEngine;
using System.Collections;

public class ObstaclesGroup : MonoBehaviour {

	public ObstaclesManager.ObstacleType obstacleType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		if (obstacleType == ObstaclesManager.ObstacleType.BBTan){
			
		}
	}
}
