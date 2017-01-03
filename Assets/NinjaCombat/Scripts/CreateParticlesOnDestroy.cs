using UnityEngine;
using System.Collections;

public class CreateParticlesOnDestroy : MonoBehaviour {

	public GameObject particlesPrefab;

	void OnDestroy(){
		GameObject.Instantiate(particlesPrefab, transform.position, Quaternion.identity);
	}
}
