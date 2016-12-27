using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class NinjaGameManager : MonoBehaviour {
	
	public NinjaController player, enemy;

	//public LivesView playerLivesView, enemyLivesView;

	public List<TweenStartRoundAnimation> roundAnimations = new List<TweenStartRoundAnimation>();

	public List<ObstacleUnit> obstacles = new List<ObstacleUnit>();
	public List<ObstacleGroup> obstacleGroups = new List<ObstacleGroup>();

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

	void Start(){

		scoreStr = scoreText.text;

		AddListeners ();

		obstacles = FindObjectsOfType<ObstacleUnit>().ToList();
		if (obstacleGroups.Count() == 0)
			obstacleGroups = FindObjectsOfType<ObstacleGroup>().ToList();

		InitGame ();
	}

	void AddListeners ()
	{
		player.HitEvent += Player_HitEvent;
		enemy.HitEvent += Enemy_HitEvent;
		roundAnimations.ForEach (x =>  {
			x.CompleteEvent += OnStartRoundAnimationComplete;
		});
		playAgainButton.onClick.AddListener(InitGame);
	}

	void InitGame ()
	{
		messagePanel.SetActive(false);
		endGamePanel.SetActive(false);
		playerScore = 0;
		enemyScore = 0;
		player.Pause ();
		enemy.Pause ();
		powerupManager.Pause();
		currentRound = 0;
		obstacleGroups.ForEach(x => x.gameObject.SetActive(false));
		obstacles.ForEach(x => x.gameObject.SetActive(true));

		StartCoroutine(ShowObstaclesOnInitRoundCoro());


	}

	IEnumerator ShowObstaclesOnInitRoundCoro(){
	
		for (int i = 0; i < obstacleGroups.Count; i++) {
			yield return new WaitForSeconds(0.2f);
			obstacleGroups[i].gameObject.SetActive(true);
		}

		ShowNextRound();
	}

	void ShowNextRound(){
		messagePanel.SetActive(false);
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

	public float hitAnimationTime = 2f;

	void OnHit (NinjaController hitNinja)
	{
		Pause();
		RemoveAllStars();

		hitNinja.ShowHitAnimation(hitAnimationTime);

		LeanTween.delayedCall(hitAnimationTime, () => {
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

	void RemoveAllStars(){
		var stars = FindObjectsOfType<NinjaStar>();

		stars.ToList().ForEach(x => {
			Destroy(x.gameObject);
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
		enemy.Pause ();
		player.Pause ();
		powerupManager.Pause();
	}

	void Resume(){
		enemy.Resume();
		player.Resume();
		powerupManager.Resume();
	}

	void GameOver(){
		ShowGameOverText();
	}

	void Win(){
		ShowWinText();
	}

	void Player_HitEvent (NinjaStar obj)
	{
		enemyScore++;

		OnHit (player);
	}

	void Enemy_HitEvent (NinjaStar obj)
	{
		playerScore++;

		OnHit (enemy);
	}


}
