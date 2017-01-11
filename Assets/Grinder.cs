using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Grinder : MonoBehaviour {

	public string type;

	CameraViewListener viewListener;

	public Transform shapeContainer;

	public GameObject trianglePrefab, circlePrefab, letterEPrefab;

	public Dictionary<string, GameObject> shapeToPrefab = new Dictionary<string, GameObject>();

	void Awake(){

		shapeToPrefab.Add("triangle", trianglePrefab);
		shapeToPrefab.Add("circle", circlePrefab);
		//shapeToPrefab.Add("letterE", letterEPrefab);



		type = shapeToPrefab.Keys.ElementAt(UnityEngine.Random.Range(0, shapeToPrefab.Keys.Count));

		var prefabToLoad = shapeToPrefab[type];

		var go = Instantiate(prefabToLoad, Vector3.zero, Quaternion.identity) as GameObject;

		go.transform.SetParent(shapeContainer, true);


	}

	void Start(){
		viewListener = GetComponentInChildren<CameraViewListener>();

	}

	public bool IsVisible{
		get{
			return viewListener.isVisible;
		} 
	}
}


