using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public SpriteRenderer icon;

	public GameObject shield;

	public float throwSpeed = 10f;

	public bool isPaused = false;

	public bool isThrowing = false;

	public bool isHit = false;

	public int powerupDuration;

	public Vector3 orgScale;

	public PowerUp.PowerupType activePowerup = PowerUp.PowerupType.None;

	public string tagToHit;

	float _powerUpTime;

	protected List<NinjaStar> activeStars = new List<NinjaStar>();

	public event Action<NinjaStar> HitEvent = (star) => {};
	public event Action<NinjaStar> ThrowStarEvent = (star) => {};
	public event Action<NinjaStar> PickUpStarEvent = (star) => {};

	public int ActiveStarsCount{
		get{
			return activeStars.Count;
		}
	}

	#region behaviour methods

	void Start () {
		AddListeners();
		orgScale = icon.transform.localScale;
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (isPaused || isHit)
			return;

		if (other.gameObject.CompareTag("Bullet")){

			var star = other.gameObject.GetComponent<NinjaStar>();

			if (star == null){
				star = other.GetComponentInParent<NinjaStar>();
			}

			if (star == null)
				return;

			if (!star.isGrounded){
				if (star.tagToHit == gameObject.tag){
					//Debug.LogError("Hit " + star.tagToHit);
					star.Hit();
					isHit = true;
					HitEvent(star);
				}else{
					//PickUpStar (star);
				}
			}else{
				PickUpStar (star);
			}
		}
	}

	void PickUpStar (NinjaStar star)
	{
		star.Pickup ();
		star.transform.position = transform.position;
		star.transform.SetParent (icon.transform, true);
		activeStars.Add (star);
		PickUpStarEvent (star);
	}

	void Update(){
		if (!isPaused){
			UpdatePowerup ();
		}
	}

	#endregion

	#region powerup

	void UpdatePowerup ()
	{
		if (activePowerup != PowerUp.PowerupType.None) {
			_powerUpTime += Time.deltaTime;
			if (_powerUpTime >= powerupDuration) {
				DisactivatePowerup ();
			}
		}
	}

	void DisactivatePowerup ()
	{
		ToggleShield(false);
		activePowerup = PowerUp.PowerupType.None;
	}

	#endregion

	#region lifecycle

	public void Init(){
		Pause();

	}

	protected virtual void AddListeners(){
		
	}

	protected virtual void RemoveListeners(){
		
	}

	public virtual void Pause(){
		isPaused = true;
		PauseMove();
		ToggleShield(false);
		activeStars = new List<NinjaStar>();
		LeanTween.cancel(gameObject);
	}

	public virtual void Resume(){
		orgScale = icon.transform.localScale;
		isPaused = false;
		isHit = false;
		isThrowing = false;
		ResumeMove();
	}

	void OnDestroy(){
		RemoveListeners();
	}

	#endregion


	public void ShowHitAnimation (float hitAnimationTime)
	{
		StartCoroutine(ShowHitAnimationCoro(hitAnimationTime));
	}

	IEnumerator ShowHitAnimationCoro (float hitAnimationTime)
	{
		float time = 0;
		float blinkSpeed = 0.075f;

		while (time < hitAnimationTime){
			icon.enabled = !icon.enabled;
			yield return new WaitForSeconds(blinkSpeed);
			time += blinkSpeed;
		}
		icon.enabled = true;
	}

	public virtual void Die(){
		Destroy(gameObject);
	}

	#region throw

	protected bool canThrow(){
		return activeStars.Count > 0;
	}

	protected NinjaStar ThrowStar (Vector2 normalizedSwipeDir, float throwSpeed)
	{
		var throwXSpeed = normalizedSwipeDir.x * throwSpeed;
		var throwYSpeed = normalizedSwipeDir.y * throwSpeed;
		var star = DequeueStar();
		star.Throw(new Vector2(throwXSpeed, throwYSpeed));
		return star;
	}

	public NinjaStar ThrowRandomDirectionStar(float throwSpeed){
		var star = DequeueStar();
		star.ThrowRandomDirection(throwSpeed);
		return star;
	}

	NinjaStar DequeueStar(){
		var star = activeStars.Last();
		star.SetTarget(tagToHit);
		star.transform.SetParent(null);
		activeStars.Remove(star);
		ThrowStarEvent(star);
		return star;
	}

	protected void StartThrowAnimation (bool setPingPong = false)
	{
		if (!LeanTween.isTweening (icon.gameObject) && moveHoriz.IsPaused){

			var tween = LeanTween.scale(icon.gameObject, new Vector3(orgScale.x * 1.2f, orgScale.y * 0.8f, orgScale.z), 0.1f).setEase (LeanTweenType.easeInOutSine);

			if (setPingPong){
				tween.setLoopPingPong(1);
			}
		}
	}

	protected void EndThrowAnimation(){
		LeanTween.scale(icon.gameObject, orgScale, 0.1f).setEase (LeanTweenType.easeInOutSine);
	}

	#endregion

	protected void ResumeMove(){
		moveHoriz.Resume();
	}

	protected void PauseMove(){
		moveHoriz.Pause();

	}

	public void SetPowerUp (PowerUp.PowerupType powerUpType)
	{
		activePowerup = powerUpType;
		_powerUpTime = 0;

		switch(activePowerup){
			case PowerUp.PowerupType.Shield:
				ToggleShield(true);
				break;
		}
	}

	void ToggleShield (bool toggle)
	{
		shield.SetActive(toggle);
	}
}

