using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NinjaMenu : MonoBehaviour {

	public Button btn;
	public Dropdown dropDown;

	// Use this for initialization
	void Start () {
		btn.onClick.AddListener(GotoNextScene);
	}

	void GotoNextScene(){
		var index = dropDown.value;

		var text = dropDown.options[index].text;

		if (text.StartsWith("B"))
			ObstaclesManager.initObstacleType = ObstaclesManager.ObstacleType.BBTan;
		else
			ObstaclesManager.initObstacleType = ObstaclesManager.ObstacleType.Wheels;

		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
