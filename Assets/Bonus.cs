using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Bonus : MonoBehaviour {

	public int speed = 100;
	public Text text;


	float orgScale;

	Rigidbody2D _rigidBody;

	public BonusType bonusType{
		get; private set;
	}

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();

		bonusType = (BonusType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(BonusType)).Length);
		
		SetTextByType();

		orgScale = transform.localScale.y;

		_rigidBody.AddForce(new Vector2(0, -speed));
		LeanTween.scaleY(gameObject, -orgScale, 1.5f).setLoopPingPong(-1);
	}

	void SetTextByType(){
		string result = "b";

		switch(bonusType){
		case BonusType.Abort:
			result = "A";
			break;
		case BonusType.Expand:
			result = "E";
			break;
		case BonusType.Fire:
			result = "L";
			break;
		case BonusType.Slow:
			result = "S";
			break;
		case BonusType.ForceField:
			result = "F";
			break;
		}

		text.text = result;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Floor")) {
			Destroy(gameObject);
		}
	}

}

public enum BonusType{
	Abort, 
	Slow, 
	Fire, 
	ForceField, 
	Expand
}
