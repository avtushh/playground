using UnityEngine;
using System.Collections;
using System;

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
		type = EnumUtils.RandomEnumValue<PowerupType> (true);

		type = PowerupType.Split;

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

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Bullet") {
			var ninjaStar = other.gameObject.GetComponent<NinjaStar> ();

			if (type == PowerupType.Split) {
				ninjaStar.Split ();				
			} else {
				NinjaController ninjaController = ninjaStar.IsPlayerStar ? (NinjaController)FindObjectOfType<PlayerNinjaController> () : (NinjaController)FindObjectOfType<EnemyNinjaController> ();
				ninjaController.SetPowerUp (type);
			}

			HitEvent ();
			Destroy (gameObject);

		}
	}
}




