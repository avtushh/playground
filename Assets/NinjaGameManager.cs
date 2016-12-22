using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NinjaGameManager : MonoBehaviour {
	
	public NinjaController player, enemy;

	public LivesView playerLivesView, enemyLivesView;

	public List<NinjaStar> ninjaStarsList;

	void Start(){
		player.HitEvent += Player_HitEvent;
		enemy.HitEvent += Enemy_HitEvent;

		player.ThrowStarEvent += Player_ThrowStarEvent;
		enemy.ThrowStarEvent += Player_ThrowStarEvent;

		ninjaStarsList = new List<NinjaStar>();
	}

	void Player_ThrowStarEvent (NinjaStar obj)
	{
		ninjaStarsList.Add(obj);
	}

	void OnHit ()
	{
		PauseGameForHit ();
		ShowHitText();
		LeanTween.delayedCall(gameObject, 1.5f, Resume);
	}

	void ShowHitText(){
		
	}

	void ShowGameOverText(){


	}

	void ShowWinText(){


	}




	void PauseGameForHit ()
	{
		enemy.Pause ();
		player.Pause ();
		ninjaStarsList.ForEach (x =>  {
			Destroy (x);
		});
		ninjaStarsList.Clear();
	}

	void Resume(){
		enemy.Resume();
		player.Resume();
	}

	void Player_HitEvent (int lives, NinjaStar obj)
	{
		ninjaStarsList.Remove(obj);
		playerLivesView.SetLives(player.lives);

		if (player.lives <= 0){
			//TODO: set loss!
		}else{
			PauseGameForHit ();
		}
	}

	void Enemy_HitEvent (int lives, NinjaStar obj)
	{
		ninjaStarsList.Remove(obj);
		enemyLivesView.SetLives(enemy.lives);

		if (enemy.lives <= 0){
			//TODO: set win!
		}else{
			OnHit ();
		}
	}


}
