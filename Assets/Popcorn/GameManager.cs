using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public static GameManager GM {
		get {
			if (GM == null) {
				GM = GameObject.FindObjectOfType<GameManager> ();
			}

			return GM;
		}
		private set {

		}
	}

	public GameObject gameOverText;
	public Text livesText;
	public Text scoreText;

	public Ball ball;
	public PlayerController controller;
	public BrickManager bm;

	public enum GameState
	{
		Idle,
		Running,
		Fail,
		GameOver
	}

	GameState _gameState;

	int _lives = 3;
	int _score = 0;

	void Start ()
	{
		_lives = 3;
		_score = 0;
		SetGameState(GameState.Idle);
		UpdateLivesText();
		UpdateScoreText();
		ball.OnBallLaunch += HandleOnBallLaunch;
		ball.OnBallTouchFloor += HandleOnBallTouchFloor;
		bm.OnBrickHit += HandleHitBrick;
		bm.OnClearBoard += HandleClearBoard;
	}

	void Restart(){
		Time.timeScale = 1f;
		ball.Reset();
		controller.Reset();
	}

	void UpdateLivesText ()
	{
		livesText.text = "Lives: " + _lives.ToString();
	}

	void HandleOnBallTouchFloor ()
	{ 
		_lives--;
		UpdateLivesText();
		if (_lives > 0)
			SetGameState(GameState.Fail);
		else
			SetGameState(GameState.GameOver);
	}

	void HandleOnBallLaunch ()
	{
		SetGameState(GameState.Running);
	}

	void HandleHitBrick(Brick brick){
		_score += brick.GetValue() * 100;
		UpdateScoreText();
	}

	void HandleClearBoard(){
		
	}

	void UpdateScoreText(){
		scoreText.text = "Score: " + _score.ToString();
	}

	public void SetGameState (GameState state)
	{
		_gameState = state;
		switch (state) {
		case GameState.Idle:
			gameOverText.SetActive(false);
			break;
		case GameState.Running:
			break;
		case GameState.Fail:
			Time.timeScale = 0.5f;
			LeanTween.delayedCall(1f, Restart);
			break;
		case GameState.GameOver:
			Time.timeScale = 0f;
			gameOverText.SetActive(true);
			break;
		}

	}




}
