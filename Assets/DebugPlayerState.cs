using UnityEngine;
using System.Collections;
using TabTale;
using UnityEngine.UI;

public class DebugPlayerState : MonoBehaviour {

	public SRPlayer player;

	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = player.CurrState.ToString();
	}
}
