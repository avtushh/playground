using UnityEngine;
using System.Collections;
using System;

namespace TabTale
{

	public class Ramp : ShapeHolder
	{
	
		public float velX, velY;
		public float gravityScale;
	
		public ShapesList shapesBank;

		protected override void OnStart ()
		{
			if (shapesBank != null){
				var shape = shapesBank.GetRandomShape();

				LoadShapePrefab(shapesBank.prefabByNameDict[shape]);
			}
		}
	}
}
