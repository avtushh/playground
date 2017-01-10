using UnityEngine;
using System.Collections;

public class ReloadLevel : MonoBehaviour {

	public void ReloadCurrentLevel(){
		Application.LoadLevel(Application.loadedLevel);
	}
}
