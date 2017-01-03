using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ObstacleUnit : MonoBehaviour {

	public GameObject particlesPrefab;

	public int initHitPoints = 1;

	public Text uiHitPoints;

	int _hitpoints;

	Color _orgColor;
	SpriteRenderer _spriteRenderer;
	Collider2D _collider;

	public static List<Color> RandomColors;

	public static int GetActiveObstaclesCount(){
		var obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList();

		return obstacles.Count(x => x.GetComponent<Collider2D>().enabled);
	}

	public bool IsEnabled(){
		if (_collider == null)
			return true;

		return _collider.enabled;
	}

	void Awake(){
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		_orgColor = _spriteRenderer.color;
		_collider = GetComponent<Collider2D>();
	}

	void OnDestroy(){
		LeanTween.cancel(gameObject);
	}

	void MoveUpDown(){
		LeanTween.moveY (gameObject, transform.position.y * 1.05f, 0.5f).setEase (LeanTweenType.easeInOutSine).setLoopPingPong ();
	}

//	static ObstacleUnit(){
//		if (RandomColors == null){
//			RandomColors = new List<Color>();
//			for(int i = 0; i < 20; i++){
//				var color = Random.ColorHSV(0.4f, 1.0f, 1, 1, 0.6f, 1,1,1);
//				color.a = 1;
//				RandomColors.Add(color);
//			}
//		}
//	}

	void Hide ()
	{
		var alphatween = LeanTween.value (gameObject, SetAlpha, 1, 0, 0.2f);

		if (GameSettings.reincartantBBTanObstacles){
			alphatween.setOnComplete(Reincarnate);
		}

		uiHitPoints.text = "";
		_collider.enabled = false;
	}

	void Reincarnate(){
		LeanTween.value (gameObject, SetAlpha, 0, 1, GameSettings.timeToReincarnateBBTan).setOnComplete(UpdatePoints).setEase(LeanTweenType.easeInExpo);
	}


	public void Hit(){

		_hitpoints--;
		UpdateHitpointsText();
		HighlightColor();

		if (_hitpoints == 0){
			CriticleHit();
		}
	}

	public void CriticleHit(){
		Hide ();
		//gameObject.SetActive(false);
		Instantiate(particlesPrefab, transform.position, Quaternion.identity);
	}

	void SetAlpha(float val){

		if (_spriteRenderer == null){
			_spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		var c = _spriteRenderer.color;
		c.a = val;
		_spriteRenderer.color = c;
	}

	void UpdatePoints ()
	{
		initHitPoints = Random.Range (1, 10);
		_hitpoints = initHitPoints;
		UpdateHitpointsText ();
		_collider.enabled = true;
		SetAlpha(1);
	}

	void OnEnable(){
		LeanTween.cancel(gameObject);
		UpdatePoints ();
		LeanTween.delayedCall(gameObject, Random.Range(0.1f, 0.5f), MoveUpDown);
	}

	void UpdateHitpointsText ()
	{
		uiHitPoints.text = _hitpoints.ToString ();
	}

	void HighlightColor(){
		var spriteRenderer = GetComponent<SpriteRenderer> ();
		var c = _orgColor;

		c.g *= 0.7f;

		spriteRenderer.color = c;

		LeanTween.delayedCall(gameObject, 0.1f, () => spriteRenderer.color = _orgColor);
		
	}
}
