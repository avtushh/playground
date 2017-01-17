using UnityEngine;
using System.Collections;

namespace TabTale
{
	public class GrinderGroup : MonoBehaviour
	{

		public ShapesList shapesList;


		void Awake ()
		{
			var grinders = GetComponentsInChildren<Grinder> ();

			var randomShapes = shapesList.GetRandomShapes (grinders.Length);

			for (int i = 0; i < grinders.Length; i++) {

				var shape = randomShapes [i];
				var prefab = shapesList.prefabByNameDict [shape];

				grinders [i].SetShapeAndType (shape, prefab);
			}
		}
	}
}