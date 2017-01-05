using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour {

	AudioSource audioSource;

	void Awake(){
		DontDestroyOnLoad(this);
		audioSource = GetComponent<AudioSource>();

	}

	void OnLevelWasLoaded(int level){
		if (Application.loadedLevel == 0){
			audioSource.volume = 1f;
		}

		else{
			audioSource.volume = 0.3f;
		}
	}


}
