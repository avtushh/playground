using UnityEngine;
using System.Collections;
using System;

public class Brick : MonoBehaviour {

	public event Action<Brick> OnHit = (b) => {};
	public GameObject brickParticle; 
		 
	SpriteRenderer _spriteRenderer;
	ParticleSystem _particles;
	Collider2D _collider;

	int _hits;
	bool _hasBonus;

	int _currentHits;

	void Start(){
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_particles = GetComponent<ParticleSystem>();
		UnityEngine.Random.seed = Time.time.GetHashCode();
		_hits = UnityEngine.Random.Range(1, 4);
		_currentHits = _hits;
		_hasBonus = UnityEngine.Random.Range(1, 4) == 1;
		ChangeColorByHitsLeft();
	}

	void TakeHit ()
	{
		_currentHits--;
		if (_currentHits > 0) {
			ChangeColorByHitsLeft ();
		}
		else {
			if (!_hasBonus) {
				Instantiate (brickParticle, transform.position, Quaternion.identity);
			}
			else {
				var bonusPrefab = Resources.Load ("Bonus") as GameObject;
				var bonus = Instantiate (bonusPrefab, transform.position, Quaternion.identity) as GameObject;
			}

			OnHit(this);

			gameObject.SetActive (false);
		}
	}



	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.CompareTag("Ball")){
			TakeHit ();
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.CompareTag("Bullet")){
			TakeHit();
			Destroy(coll.gameObject);
		}
	}

	void ChangeColorByHitsLeft ()
	{
		Color color = Color.white;

		switch(_currentHits){
		case 5:
			color = Color.white;
			break;
		case 4:
			color = Color.yellow;
			break;
		case 3:
			color = Color.cyan;
			break;
		case 2:
			color = Color.green;
			break;
		case 1:
			color = Color.red;
			break;
		}
		_spriteRenderer.color = color;

	}

	public int GetValue(){
		return _hits;
	}
}
