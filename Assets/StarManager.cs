using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StarManager : MonoBehaviour {

	public GameObject starPrefab;

	public Transform left, right, floor, ceiling;


	public NinjaController player, enemy;

	bool _paused = false;

	public int initPlayerStars = 3, initEnemyStars = 3;

	List<NinjaStar> _playerStarsOnGround, _enemyStarsOnGround;

	public void Clear(){
		var starsList = FindObjectsOfType<NinjaStar>().ToList();
		starsList.ForEach(x => Object.Destroy(x.gameObject));
		starsList.Clear();
	}

	public void Pause(){
		_paused = true;
	}

	public void Resume(){
		_paused = false;

	}

	void Start(){
		player.PickUpStarEvent += OnPlayerPickup;
		enemy.PickUpStarEvent += OnPlayerPickup;
	}

	void OnPlayerPickup (NinjaStar obj)
	{
		_playerStarsOnGround.Remove(obj);
	}

	void OnEnemyPickup (NinjaStar obj)
	{
		_enemyStarsOnGround.Remove(obj);
	}

	public void InitRound(){
		_playerStarsOnGround = new List<NinjaStar>();
		_enemyStarsOnGround = new List<NinjaStar>();
		Clear();

		CreateInitialStarsOnGround();

		StartCoroutine(SpawnStarsCoro());
	}

	void CreateInitialStarsOnGround ()
	{
		for (int i = 0; i < initPlayerStars; i++) {
			AddPlayerStarOnGround();
		}

		for (int i = 0; i < initEnemyStars; i++) {
			AddEnemyStarOnGround();
		}

	}

	IEnumerator SpawnStarsCoro(){
		while (true){
			if (!_paused){

				if (FindObjectsOfType<NinjaStar>().Length <= 2){
					CreateInitialStarsOnGround();
				}

				yield return new WaitForSeconds(0.5f);
			}else{
				yield return null;
			}
		}
	}

	void AddPlayerStarOnGround ()
	{
		var star = CreateStarInRange (floor.position);
		_playerStarsOnGround.Add (star);
	}

	void AddEnemyStarOnGround ()
	{
		var star = CreateStarInRange (ceiling.position);
		_enemyStarsOnGround.Add (star);
	}

	NinjaStar CreateStarInRange(Vector3 pos){

		pos.x = Random.Range(left.position.x, right.position.x);

		var starGo = Instantiate (starPrefab, pos, Quaternion.identity) as GameObject;
		var star = starGo.GetComponent<NinjaStar> ();
		star.IsFireball = false;
		star.isGrounded = true;

		return star;
	}
}
