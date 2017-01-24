using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

	float deltaTime = 0.0f;

	public Text txt;

	float fps;

	void Start(){
		DontDestroyOnLoad(gameObject);

		StartCoroutine(PrintFPSCoro());
	}

	IEnumerator PrintFPSCoro(){
		while(true){
			PrintFPS ();

			yield return new WaitForSeconds(0.2f);
		}

	}

	void PrintFPS ()
	{
		txt.text = fps.ToString ("#");
	}

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		fps = (1.0f / deltaTime) * Time.timeScale;



		//PrintFPS();

	}

//	void OnGUI()
//	{
//		int w = Screen.width, h = Screen.height;
//
//		GUIStyle style = new GUIStyle();
//
//		Rect rect = new Rect(0, 0, w, h * 2 / 100);
//		style.alignment = TextAnchor.LowerRight;
//		style.contentOffset = new Vector2(-50, -20);
//		style.fontSize = h * 2 / 50;
//		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
//		float msec = deltaTime * 1000.0f;
//		float fps = 1.0f / deltaTime;
//		string text = fps.ToString ("#");
//		GUI.Label(rect, text, style);
//	}
}
