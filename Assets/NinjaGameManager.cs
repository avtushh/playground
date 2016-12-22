using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NinjaGameManager : MonoBehaviour {
	
	public NinjaController player, enemy;

	public LivesView playerLivesView, enemyLivesView;

	public List<NinjaStar> ninjaStarsList;

	public List<TweenStartRoundAnimation> roundAnimations = new List<TweenStartRoundAnimation>();

	public GameObject messagePanel;
	public Text scoreText;

	public int currentRound; // 0,1,2

	void Start(){
		player.HitEvent += Player_HitEvent;
		enemy.HitEvent += Enemy_HitEvent;

		player.ThrowStarEvent += Player_ThrowStarEvent;
		enemy.ThrowStarEvent += Player_ThrowStarEvent;

		ninjaStarsList = new List<NinjaStar>();

		roundAnimations.ForEach(x => {
			x.CompleteEvent += OnStartRoundAnimationComplete;	
		});

		//player.Pause();
		//enemy.Pause();
		currentRound = 0;
		ShowNextRound();
	}

	void ShowNextRound(){
		messagePanel.SetActive(false);
		scoreText.gameObject.SetActive(false);
		roundAnimations[currentRound].gameObject.SetActive(true);
	}

	void OnStartRoundAnimationComplete ()
	{
		LeanTween.delayedCall(1f, () => {
			roundAnimations[currentRound].gameObject.SetActive(false);
			StartNextRound();

		});
	}

	void StartNextRound(){
		currentRound++;
		Resume();
	}

	void Player_ThrowStarEvent (NinjaStar obj)
	{
		ninjaStarsList.Add(obj);
	}

	void OnHit ()
	{
		PauseGameForHit ();
		ShowHitText();
		LeanTween.delayedCall(gameObject, 1.5f, ShowNextRound);
	}

	void ShowHitText(){

		messagePanel.SetActive(true);
		scoreText.gameObject.SetActive(true);
		scoreText.text = scoreText.text.Replace("%", player.lives.ToString()).Replace("$", enemy.lives.ToString());
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
			OnHit ();
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
