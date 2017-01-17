using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
namespace TabTale{
public class Grinder : MonoBehaviour {

	public Shape shape; 

	public string type;

	public bool isAlive = false;

	CameraViewListener viewListener;

	public Transform shapeContainer;

	public SpriteRenderer sprRenderer;
	public Sprite sprNormal, sprBloody;

	public static int lastTypeIndex = -1;

	void Awake(){

		//type = shapeToPrefab.Keys.ElementAt(UnityEngine.Random.Range(0, shapeToPrefab.Keys.Count));


	}

	public void SetShapeAndType(Shape shape, GameObject prefab ){
		this.shape = shape; 
		this.type = shape.ToString();
		var go = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

		go.transform.SetParent(shapeContainer, false);

		isAlive = true;
	}

	void Start(){
		viewListener = GetComponentInChildren<CameraViewListener>();
	}

	void SetShapePrefab(){
		
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


}