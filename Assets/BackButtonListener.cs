using UnityEngine;
using System.Collections;
using UnityEditor;


public class BackButtonListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (Application.loadedLevel == 1){
				Application.LoadLevel(0);
			}else{
				#if UNITY_EDITOR
				EditorApplication.isPlaying = false; 
				#else
					Application.Quit();
				#endif
			}
		}
	}
}
