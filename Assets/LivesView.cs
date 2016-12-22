using UnityEngine;
using System.Collections;

public class LivesView : MonoBehaviour {

	public int lives = 3;

	public GameObject lifePrefab;

	void Start(){
		SetLives(lives);
	}

	public void SetLives(int count){
		lives = count;

		if (lives < 0)
			lives = 0;

		transform.RemoveAllChildren();

		for (int i = 0; i < lives; i++) {
			var go = Instantiate(lifePrefab, Vector3.zero, Quaternion.identity) as GameObject;

			go.transform.SetParent(transform);
		}
	}
}
