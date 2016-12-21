using UnityEngine;
using System.Collections;

public class CreateParticlesOnDestory : MonoBehaviour {

	public GameObject particlesPrefab;

	void OnDestroy(){
		GameObject.Instantiate(particlesPrefab, transform.position, Quaternion.identity);
	}
}
