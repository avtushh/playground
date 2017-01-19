using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class NinjaGameManager : MonoBehaviour {
	
	public NinjaController player, enemy;

	//public LivesView playerLivesView, enemyLivesView;

	public List<TweenStartRoundAnimation> roundAnimations = new List<TweenStartRoundAnimation>();

	public GameObject messagePanel;
	public Text scoreText;

	public int currentRound; // 0,1,2

	public int playerScore; 
	public int enemyScore;

	string scoreStr;
	public GameObject endGamePanel;
	public Button  playAgainButton;
	public Text endText;

	public PowerUpsManager powerupManager;
	public StarManager starManager;
	public ObstaclesManager obstaclesManager;

	public List<GameObject> frame;

	public static bool isPaused = false;

	public enum State{
		StartRound, Active, HitEnemy, HitPlayer, Win, GameOver
	}

	public State state;

	void Start(){

		scoreStr = scoreText.text;

		AddListeners ();

		InitGame ();
	}

	void AddListeners ()
	{
		player.HitEvent += Player_HitEvent;
		enemy.HitEvent += Enemy_HitEvent;
		enemy.ThrowStarEvent += OnThrowStar;
		player.ThrowStarEvent += OnThrowStar;
		roundAnimations.ForEach (x =>  {
			x.CompleteEvent += OnStartRoundAnimationComplete;
		});
		playAgainButton.onClick.AddListener(InitGame);
	}

	void RemoveListeners(){
		player.HitEvent -= Player_HitEvent;
		enemy.HitEvent -= Enemy_HitEvent;
		enemy.ThrowStarEvent -= OnThrowStar;
		player.ThrowStarEvent -= OnThrowStar;
		roundAnimations.ForEach (x =>  {
			x.CompleteEvent -= OnStartRoundAnimationComplete;
		});
		playAgainButton.onClick.RemoveListener(InitGame);

		LeanTween.cancel(gameObject);
	}

	void OnDestroy(){
		RemoveListeners();
	}

	void OnThrowStar (NinjaStar obj)
	{
		
	}

	void InitGame ()
	{
		messagePanel.SetActive(false);
		endGamePanel.SetActive(false);
		playerScore = 0;
		enemyScore = 0;
		player.Init();
		enemy.Init();

		currentRound = 0;
		starManager.Clear();

		LeanTween.delayedCall(gameObject, 0.5f, ShowNextRound);
	}

	IEnumerator ShowObstaclesOnInitRoundCoro(){
	
		obstaclesManager.ActivateObstaclesGroupByType();
		yield return new WaitForSeconds(1f);
		ShowNextRound();
	}

	void ShowNextRound(){
		state = State.StartRound;
		messagePanel.SetActive(false);
		roundAnimations[currentRound].gameObject.SetActive(true);
		switch(currentRound){
			case 0:
				SoundManager.PlayRoundOne();
				break;
			case 1:
				SoundManager.PlayRoundTwo();
				break;
			case 2:
				SoundManager.PlayFinalRound();
				break;

		}
	}

	void OnStartRoundAnimationComplete ()
	{
		
		LeanTween.delayedCall(gameObject, 1f, () => {
			roundAnimations[currentRound].gameObject.SetActive(false);
			StartNextRound();
		});
	}

	void StartNextRound(){
		currentRound++;
		starManager.InitRound();
		obstaclesManager.ActivateObstaclesGroupByType();

		Resume();
	}

	public float hitAnimationTime = 2f;

	void OnHit (NinjaController hitNinja)
	{
		SoundManager.PlayHitSound();
		Pause();
		starManager.Clear();
		hitNinja.ShowHitAnimation(hitAnimationTime);

		LeanTween.delayedCall(gameObject, hitAnimationTime, () => {
			ShowHitText();

			if (playerScore >= 2){
				LeanTween.delayedCall(gameObject, 1.5f, Win);
			}else if (enemyScore >= 2){
				LeanTween.delayedCall(gameObject, 1.5f, GameOver);
			}else{
				LeanTween.delayedCall(gameObject, 1.5f, ShowNextRound);
			}
		});
	}

	void ShowHitText(){

		messagePanel.SetActive(true);
		scoreText.text = scoreStr.Replace("%", playerScore.ToString()).Replace("$", enemyScore.ToString());
	}

	void ShowGameOverText(){
		endGamePanel.SetActive(true);
		endText.text = "Game Over";
	}

	void ShowWinText(){
		endGamePanel.SetActive(true);
		endText.text = "You Win!";
	}

	void Pause ()
	{
		isPaused = true;
		enemy.Pause ();
		player.Pause ();
		powerupManager.Pause();
		starManager.Pause();
	}

	void Resume(){
		isPaused = false;
		enemy.Resume();
		player.Resume();
		powerupManager.Resume();
		starManager.Resume();

		state = State.Active;
	}

	void GameOver(){
		state = State.GameOver;
		ShowGameOverText();
	}

	void Win(){
		state = State.Win;
		ShowWinText();
	}

	void Player_HitEvent (NinjaStar obj)
	{
		state = State.HitPlayer;
		enemyScore++;

		OnHit (player);
	}

	void Enemy_HitEvent (NinjaStar obj)
	{
		state = State.HitEnemy;
		playerScore++;

		OnHit (enemy);
	}

	public void ShowErrorFrame(){
		
		frame.ForEach(x => {
			x.SetActive(true);

		});

		LeanTween.delayedCall(gameObject, 0.2f, () => {
			frame.ForEach(x => {
				x.SetActive(false);

			});

		} );

	}

}
