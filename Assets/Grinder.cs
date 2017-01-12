using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Grinder : MonoBehaviour {

	public string type;

	public bool isAlive = false;

	CameraViewListener viewListener;

	public Transform shapeContainer;

	public GameObject trianglePrefab, circlePrefab, letterEPrefab, number3Prefab, number8Prefab;

	public SpriteRenderer sprRenderer;
	public Sprite sprNormal, sprBloody;

	public Dictionary<string, GameObject> shapeToPrefab = new Dictionary<string, GameObject>();

	public static string [] PREFABS_TYPES = {"triangle", "circle", "letterS", "number3", "number8"};

	public static int lastTypeIndex = -1;

	void Awake(){

		shapeToPrefab.Add("triangle", trianglePrefab);
		shapeToPrefab.Add("circle", circlePrefab);
		shapeToPrefab.Add("s", letterEPrefab);
		shapeToPrefab.Add("number3", number3Prefab);
		shapeToPrefab.Add("number8", number8Prefab);

		//type = shapeToPrefab.Keys.ElementAt(UnityEngine.Random.Range(0, shapeToPrefab.Keys.Count));

		type = GetNextType();
		type = "s";

		var prefabToLoad = shapeToPrefab[type];

		var go = Instantiate(prefabToLoad, Vector3.zero, Quaternion.identity) as GameObject;

		go.transform.SetParent(shapeContainer, false);

		isAlive = true;
	}

	public static string GetNextType(){
		int count = PREFABS_TYPES.Length;
		count = 3;
		int index = UnityEngine.Random.Range(0, count);

		while (index == lastTypeIndex){
			index = UnityEngine.Random.Range(0, count);
		}

		lastTypeIndex = index;
		return PREFABS_TYPES[index];

	}

	void Start(){
		viewListener = GetComponentInChildren<CameraViewListener>();
	}

	public bool IsVisible(bool addSafetyDelta){
		
		return viewListener.IsInCameraBounds(addSafetyDelta);
	}

	public void Kill(){
		isAlive = false;
		Destroy(gameObject);
	}

	public void OnKillPlayer(){
		sprRenderer.sprite = sprBloody;
	}
}


