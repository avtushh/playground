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
			if (holder.GetComponent<Grinder> () != null){
				holder.GetComponent<Grinder> ().Kill();
			}else if (holder.GetComponent<Ramp>() != null){
				holder.GetComponent<Ramp> ().Kill();
			}
		}
	}
}
