using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {

	Text uiText;

	public static DebugText Instance;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		uiText = GetComponent<Text>();
	}

	void SetTextInner(string text){
		uiText.text = text;
	}

	public static void SetText(string text){
		//Instance.SetTextInner(text);
	}
}
