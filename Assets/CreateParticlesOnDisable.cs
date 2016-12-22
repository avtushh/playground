using UnityEngine;
using System.Collections;

public class CreateParticlesOnDisable : MonoBehaviour {

	public GameObject particlesPrefab;

	void OnDisable(){
		GameObject.Instantiate(particlesPrefab, transform.position, Quaternion.identity);
	}


}
