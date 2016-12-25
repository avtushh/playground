using UnityEngine;
using System.Collections;
using System;

public class PowerUp : MonoBehaviour
{
	public event Action HitEvent = () => {};

	public enum PowerupType{
		Shield, Split, DestroyObstalceGroup, None
	}

	public PowerupType type;

	SpriteRenderer _sprRenderer;

	public Sprite sprShield, sprSplit, sprDestroyGroup;

	void Awake(){
		type = EnumUtils.RandomEnumValue<PowerupType>();
		type = PowerupType.Shield;
		_sprRenderer = GetComponent<SpriteRenderer>();

		switch(type){
			case PowerupType.Shield:
				_sprRenderer.sprite = sprShield;
				break;
			case PowerupType.Split:
				_sprRenderer.sprite = sprSplit;
				break;
			case PowerupType.DestroyObstalceGroup:
				_sprRenderer.sprite = sprDestroyGroup;
				break;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bullet") {
			var ninjaStar = other.gameObject.GetComponent<NinjaStar>();

			if (ninjaStar.IsPlayerStar){
				FindObjectOfType<PlayerNinjaController>().SetPowerUp(type);
				HitEvent();
				Destroy(gameObject);
			}
		}
	}
}




