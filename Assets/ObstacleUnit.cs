using UnityEngine;
using System.Collections;

public class ObstacleUnit : MonoBehaviour {

	public GameObject particlesPrefab;

	public void Hit(){
		gameObject.SetActive(false);
		Instantiate(particlesPrefab, transform.position, Quaternion.identity);
	}
}
