using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TabTale
{
	public class ShapesList : ScriptableObject
	{

		public GameObject circle, infinity, letter_e, triangle, alpha, line;


		public Dictionary<Shape, GameObject> prefabByNameDict = new Dictionary<Shape, GameObject> ();

		void OnEnable ()
		{
			prefabByNameDict.Add (Shape.circle, circle);
			prefabByNameDict.Add (Shape.triangle, triangle);
			prefabByNameDict.Add (Shape.infinity, infinity);
			prefabByNameDict.Add (Shape.e, letter_e);
			prefabByNameDict.Add (Shape.alpha, alpha);
			prefabByNameDict.Add (Shape.line, line);
		}

		public List<Shape> GetRandomShapes (int count)
		{

			return prefabByNameDict.RandomUniqueKeys (count);

		}

		public Shape GetRandomShape(){
			return prefabByNameDict.RandomUniqueKeys (1)[0];
		}
	}


	public enum Shape
	{
		circle,
		triangle,
		infinity,
		e,
		s,
		alpha,
		line,
		number_3,
		None
	}
}