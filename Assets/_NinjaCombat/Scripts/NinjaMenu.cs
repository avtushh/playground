using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NinjaMenu : MonoBehaviour {

	public Button btn;
	public Dropdown dropDown;
	public Toggle toggle;
	public InputField inputField;

	// Use this for initialization
	void Start () {
		btn.onClick.AddListener(GotoNextScene);
		inputField.text = GameSettings.startingStars.ToString();
	}

	void OnValueChanged(string text){
		GameSettings.startingStars = int.Parse(text);
	}

	void GotoNextScene(){
		var index = dropDown.value;

		var text = dropDown.options[index].text;

		if (text.StartsWith("B"))
			ObstaclesManager.initObstacleType = ObstaclesManager.ObstacleType.BBTan;
		else
			ObstaclesManager.initObstacleType = ObstaclesManager.ObstacleType.Wheels;


		GameSettings.ShowAim = toggle.isOn;
		GameSettings.startingStars = int.Parse(inputField.text);

		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	void OnShowAimToggleChanged(bool val){
		GameSettings.ShowAim = val;

	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
