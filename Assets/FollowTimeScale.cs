using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace TabTale
{
public class FollowTimeScale : MonoBehaviour {

	Text txt;

	void Start(){
		txt = GetComponent<Text>();
	}

	void Update(){
		txt.text = Time.timeScale.ToString("0.00");
	}


}
}