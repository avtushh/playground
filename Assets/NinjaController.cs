using UnityEngine;
using System.Collections;
using System;

public class NinjaController : MonoBehaviour {

	public MoveHoriz moveHoriz;

	public GameObject ninjaStarPrefab;

	public SpriteRenderer icon;

	public float throwSpeed = 10f;

	public bool isPaused = false;

	public bool isThrowing = false;

	public int powerupDuration;

	public Vector3 orgScale;

	public PowerUp.PowerupType activePowerup = PowerUp.PowerupType.None;

	public GameObject shield;

	float _powerUpTime;

	public string tagToHit;

	public event Action<NinjaStar> HitEvent = (star) => {};
	public event Action<NinjaStar> ThrowStarEvent = (star) => {};

	void Start () {
		AddListeners();
		orgScale = icon.transform.localScale;
	}

	void Update(){
		if (!isPaused){
			UpdatePowerup ();
		}
	}

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

	void OnDestroy(){
		RemoveListeners();
	}

	protected virtual void AddListeners(){
		
	}

	protected virtual void RemoveListeners(){
		
	}

	public virtual void Pause(){
		isPaused = true;
		PauseMove();
		ToggleShield(false);
	}

	public virtual void Resume(){
		orgScale = icon.transform.localScale;
		isPaused = false;
		ResumeMove();
	}

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

	protected NinjaStar ThrowStar (Vector2 normalizedSwipeDir, float throwSpeed)
	{
		var star = CreateStar ();

		var throwXSpeed = normalizedSwipeDir.x * throwSpeed;
		var throwYSpeed = normalizedSwipeDir.y * throwSpeed;
		star.Throw(new Vector2(throwXSpeed, throwYSpeed));
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

	public NinjaStar ThrowRandomDirectionStar(float throwSpeed){
		var star = CreateStar ();
		star.ThrowRandomDirection(throwSpeed);
		ThrowStarEvent(star);
		return star;
	}

	NinjaStar CreateStar ()
	{
		var starGo = Instantiate (ninjaStarPrefab, transform.position, Quaternion.identity) as GameObject;
		var star = starGo.GetComponent<NinjaStar> ();
		star.SetTarget (tagToHit);
		star.IsFireball = activePowerup == PowerUp.PowerupType.FireBall;
		return star;
	}

	protected void ResumeMove(){
		moveHoriz.Resume();
	}

	protected void PauseMove(){
		moveHoriz.Pause();

	}

	void OnTriggerEnter2D(Collider2D other) {

		if (isPaused)
			return;
		
		if (other.gameObject.CompareTag("Bullet")){

			var star = other.gameObject.GetComponent<NinjaStar>();

			if (star.tagToHit == gameObject.tag){
				//Debug.LogError("Hit " + star.tagToHit);
				HitEvent(star);
				star.Hit();
			}
		}
	}

	public void SetPowerUp (PowerUp.PowerupType powerUpType)
	{
		activePowerup = powerUpType;
		_powerUpTime = 0;

		switch(activePowerup){
			case PowerUp.PowerupType.Shield:
				ToggleShield(true);
				break;
			case PowerUp.PowerupType.Split:
				break;
			case PowerUp.PowerupType.FireBall:
				break;
		}
	}

	void ToggleShield (bool toggle)
	{
		shield.SetActive(toggle);
	}
}

