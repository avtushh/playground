using UnityEngine;
using System.Collections;

namespace TabTale
{
	public class ShapeDataComponent : MonoBehaviour
	{
		GameObject holder;
		public Shape shape;

		public string Type{
			get{
				return shape.ToString();
			}
		}

		public void Set(GameObject holder){
			this.holder = holder;
		}

		public void Activate(){
			if (GetComponentInParent<ShapeHolder>() != null){
				GetComponentInParent<ShapeHolder>().Kill();
			}
		
		}
	}
}
