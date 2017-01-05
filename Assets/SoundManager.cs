using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance;

	public AudioClip sfxThrow, sfxHit, sfxHitWall, sfxPickupStar, sfxPickupPowerup;

	public AudioClip sfxRoundOne, sfxRoundTwo, sfxFinalRound;

	public AudioSource audioSource;


	void Awake(){
		Instance = this;
	}

	public static void PlayThrowSound(){
		Instance.PlaySound(Instance.sfxThrow, 0.5f);
	}

	public static void PlayHitSound(){
		Instance.PlaySound(Instance.sfxHit, 0.9f);
	}

	public static void PlayHitWallSound(){
		//Instance.audioSource.Play();
		Instance.PlaySound(Instance.sfxHitWall, 0.5f);
	}

	public static void PlayPickupStarSound(){
		Instance.PlaySound(Instance.sfxPickupStar, 0.5f);
	}

	public static void PlayRoundOne(){
		Instance.PlaySound(Instance.sfxRoundOne, 0.7f);
	}

	public static void PlayRoundTwo(){
		Instance.PlaySound(Instance.sfxRoundTwo, 0.7f);
	}

	public static void PlayFinalRound(){
		Instance.PlaySound(Instance.sfxFinalRound, 0.8f);
	}

	void PlaySound (AudioClip clip, float volume = 1.0f){
		AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
	}

}
