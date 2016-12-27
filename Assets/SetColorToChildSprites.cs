using UnityEngine;
using System.Collections;
using System.Linq;

public class SetColorToChildSprites : MonoBehaviour {

	public Color color;

	public bool random;

	void Awake(){
		var sprites = transform.GetComponentsInChildren<SpriteRenderer>();

		if (random)
			color = GetRandomColor();

		for (int i = 0; i < sprites.Length; i++) {
			sprites[i].color = color;
		}
	}

	Color GetRandomColor(){
		Color color = new Color();

		color.r = Random.Range(0,1f);
		color.g = Random.Range(0,1f);
		color.b = Random.Range(0,1f);
		color.a = 1;

		return color;
	}
}
