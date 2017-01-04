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
		Instance.PlaySound(Instance.sfxHitWall);
	}

	public static void PlayPickupStarSound(){
		Instance.PlaySound(Instance.sfxPickupStar, 0.5f);
	}

	public static void PlayRoundOne(){
		Instance.PlaySound(Instance.sfxRoundOne);
	}

	public static void PlayRoundTwo(){
		Instance.PlaySound(Instance.sfxRoundTwo);
	}

	public static void PlayFinalRound(){
		Instance.PlaySound(Instance.sfxFinalRound);
	}

	void PlaySound (AudioClip clip, float volume = 1.0f){
		AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
	}

}
