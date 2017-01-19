using UnityEngine;
using System.Collections;

namespace TabTale
{
	public class GrinderGroup : MonoBehaviour
	{

		public ShapesList shapesList;

		public Shape forcedShape = Shape.None;

		void Awake ()
		{
			var grinders = GetComponentsInChildren<Grinder> ();

			if (forcedShape == Shape.None){
				var randomShapes = shapesList.GetRandomShapes (grinders.Length);

				for (int i = 0; i < grinders.Length; i++) {

					var shape = randomShapes [i];
					var prefab = shapesList.prefabByNameDict [shape];

					grinders [i].LoadShapePrefab (prefab);
				}
			}else{
				for (int i = 0; i < grinders.Length; i++) {

					var prefab = shapesList.prefabByNameDict [forcedShape];

					grinders [i].LoadShapePrefab (prefab);
				}
			}



		}
	}
}