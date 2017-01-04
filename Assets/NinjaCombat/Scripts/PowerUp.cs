using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour
{
	public event Action HitEvent = () => {};

	public enum PowerupType
	{
		None,
		Shield,
		FireBall,
		Split
	}

	public PowerupType type;

	SpriteRenderer _sprRenderer;

	public Sprite sprShield, sprSplit, sprFireball;

	void Awake ()
	{
		List<PowerupType> types = new List<PowerupType>(){PowerupType.Shield, PowerupType.Shield, PowerupType.Shield, PowerupType.Split, PowerupType.Split, PowerupType.Split, PowerupType.Split, PowerupType.Split, PowerupType.FireBall};

		type = types[UnityEngine.Random.Range(0, types.Count)];

		_sprRenderer = GetComponent<SpriteRenderer> ();

		switch (type) {
			case PowerupType.Shield:
				_sprRenderer.sprite = sprShield;
				break;
			case PowerupType.Split:
				_sprRenderer.sprite = sprSplit;
				break;
			case PowerupType.FireBall:
				_sprRenderer.sprite = sprFireball;
				break;
		}

		LeanTween.moveY (gameObject, transform.position.y * 1.05f, 0.5f).setEase (LeanTweenType.easeInOutSine).setLoopPingPong ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bullet") {
			var ninjaStar = other.gameObject.GetComponent<NinjaStar> ();

			SoundManager.PlayPickupStarSound();

			switch (type) {
				case PowerupType.FireBall:
					ninjaStar.IsFireball = true;
					break;
				case PowerupType.Split:
					ninjaStar.Split ();	
					break;
				default:
					NinjaController ninjaController = ninjaStar.IsPlayerStar ? (NinjaController)FindObjectOfType<PlayerNinjaController> () : (NinjaController)FindObjectOfType<EnemyNinjaController> ();
					ninjaController.SetPowerUp (type);
					break;
			}

			HitEvent ();
			Destroy (gameObject);

		}
	}
}




