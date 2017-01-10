using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public void DoLoadScene(int index){
		Application.LoadLevel(index);
	}
}
