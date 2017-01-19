using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowZoomAmount : MonoBehaviour {

	Text txt;
	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = "Zoom: " + Camera.main.orthographicSize.ToString("0.00");
	}
}
