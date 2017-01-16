using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowTimeScale : MonoBehaviour {

	Text txt;

	void Start(){
		txt = GetComponent<Text>();
	}

	void Update(){
		txt.text = Time.timeScale.ToString("0.00");
	}


}
