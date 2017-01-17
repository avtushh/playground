using UnityEngine;
using System.Collections;

namespace TabTale
{
	public class CameraFollow : MonoBehaviour
	{
		public Transform target;
		//target for the camera to follow
		public float xOffset;
		//how much x-axis space should be between the camera and target
		public float yOffset;

		public bool followHorizontal, followVertical;

		void Update ()
		{
			var x = followHorizontal ? target.position.x + xOffset : transform.position.x;
			var y = followVertical ? target.position.y + yOffset : transform.position.y;

			transform.position = new Vector3 (x, y, transform.position.z);

		}
	}
}
