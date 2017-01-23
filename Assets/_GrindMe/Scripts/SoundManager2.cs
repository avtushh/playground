using UnityEngine;
using System.Collections;

public class SoundManager2 : MonoBehaviour {

	public static SoundManager2 Instance;

	public AudioClip sfxBoing, sfxHit, sfxKill, sfxDanger;

	public AudioSource bgAudioSource;


	void Awake(){

		if (Instance != null){
			Destroy(gameObject);
		}else{
			DontDestroyOnLoad(this);
			Instance = this;
			bgAudioSource.Play();
		}
	}



	public static void PlayBoing(){
		Instance.PlaySound(Instance.sfxBoing, 0.7f);
	}

	public static void PlayHitSound(){
		Instance.PlaySound(Instance.sfxHit, 0.9f);
	}

	public static void PlayKillSound(){
		Instance.PlaySound(Instance.sfxKill, 1f);
	}

	public static void PlayDangerSound(){
		Instance.PlaySound(Instance.sfxDanger, 0.7f);
	}

	void PlaySound (AudioClip clip, float volume = 1.0f){
		AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
	}

}
