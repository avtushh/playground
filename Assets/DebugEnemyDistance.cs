using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TabTale;

public class DebugEnemyDistance : MonoBehaviour {

	public SRPlayer player;

	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		txt.text = player.closetsEnemyDistance.ToString("0.00");
	}
}
