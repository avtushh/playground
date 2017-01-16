using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShapesList : ScriptableObject {

	public GameObject circle, infinity, letter_e, triangle; //alpha, letter_s

	public Dictionary<Shape, GameObject> prefabByNameDict = new Dictionary<Shape, GameObject>();

	void OnEnable(){
		prefabByNameDict.Add(Shape.circle, circle);
		prefabByNameDict.Add(Shape.triangle, triangle);
		prefabByNameDict.Add(Shape.infinity, infinity);
		prefabByNameDict.Add(Shape.e, letter_e);
//		prefabByNameDict.Add(Shape.alpha, alpha);
//		prefabByNameDict.Add(Shape.s, letter_s);
	}

	public List<Shape> GetRandomShapes(int count){

		return prefabByNameDict.RandomUniqueKeys(count);

	}
}


public enum Shape{
	circle, triangle, infinity, e, s, alpha
}