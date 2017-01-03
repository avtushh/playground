using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NinjaStarsCounter : MonoBehaviour
{

	public Text starCountText;

	public PlayerNinjaController player;

	public NinjaGameManager GM;

	int currentStars = -1;

	void State ()
	{
	}


	void Update ()
	{
		switch (GM.state) {
			case NinjaGameManager.State.Active:
				if (currentStars != player.ActiveStarsCount) {
					currentStars = player.ActiveStarsCount;
					if (currentStars > 0) {
						starCountText.color = Color.white;
						starCountText.text = "x" + currentStars;
					} else {
						starCountText.color = Color.yellow;
						starCountText.text = " get more stars!";
					}
				
				}
				break;
			case NinjaGameManager.State.HitPlayer:
				starCountText.text = " OUCH!";
				break;
			case NinjaGameManager.State.HitEnemy:
				starCountText.text = " YEAH!";
				break;
			case NinjaGameManager.State.GameOver:
				starCountText.text = " Game Over";
				break;
			case NinjaGameManager.State.Win:
				starCountText.text = " VICTORY!";
				break;
			case NinjaGameManager.State.StartRound:
				starCountText.text = " Get Ready";
				break;
		}


	}
}
