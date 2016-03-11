using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[Header("References")]
	public Ball ball;
	public GameObject bulletPrefab;

	public Transform leftWall, rightWall;
	public GameObject shield;

	[Header("Properties")]
	public float paddleSpeed = 1f;

	SpriteRenderer _spriteRenderer;
	Vector3 _orgPosition;

	float _panelWidth;
	float _leftEdge;
	float _rightEdge;

	bool _canFire = false;
	Color _orgColor;

	void Start ()
	{
		_orgPosition = transform.position;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_orgColor = _spriteRenderer.color;
		Reset ();
	}
	
	void Update ()
	{
		UpdatePanelMovement ();

		if (ball.IsAttached) {
			MoveBallWithPanel ();
		}

		if (_canFire && Input.GetMouseButtonDown(0)){
			Instantiate(bulletPrefab, transform.position - new Vector3(_panelWidth / 2,0,0), Quaternion.identity);
			Instantiate(bulletPrefab, transform.position + new Vector3(_panelWidth / 2,0,0), Quaternion.identity);
		}
	}

	public void Reset ()
	{
		transform.position = _orgPosition;
		ball.IsAttached = true;
		lastMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		_panelWidth = GetComponent<Collider2D> ().bounds.size.x;
		_leftEdge = leftWall.position.x + leftWall.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		_rightEdge = rightWall.position.x - rightWall.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		Abort();
	}
		
	Vector3 lastMousePos;

	void UpdatePanelMovement ()
	{
		float moveMentInputDelta = Input.GetAxis ("Horizontal");
		if (moveMentInputDelta != 0)
			MovePanel (moveMentInputDelta * paddleSpeed * Time.deltaTime);
		else {
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			Vector3 delta = worldMousePos - lastMousePos;
				
			MovePanel (delta.x);
				
			lastMousePos = worldMousePos;
		}
	}

	void MovePanel (float movement)
	{
		float targetX = transform.position.x + movement;

		if (targetX >= _rightEdge - _panelWidth / 2) {
			targetX = _rightEdge - _panelWidth / 2;
			movement = targetX - transform.position.x;
		} else if (targetX <= _leftEdge + _panelWidth / 2) {
			targetX = _leftEdge + _panelWidth / 2;
			movement = targetX - transform.position.x;
		}

		transform.Translate (movement, 0, 0);
	}

	void MoveBallWithPanel ()
	{
		var pos = ball.transform.position;
		pos.x = transform.position.x;
		ball.transform.position = pos;
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Bonus")){
			var bonus = other.GetComponent<Bonus>();

			SetBehaviourByBonusType(bonus.bonusType);

			GameObject.Destroy(bonus.gameObject);
		}

	}

	void SetBehaviourByBonusType(BonusType bonusType){
		switch(bonusType){
		case BonusType.Abort:
			Abort();
			break;
		case BonusType.Expand:
			ToggleFireMode(false);
			Expand();
			break;
		case BonusType.Fire:
			Shrink();
			ToggleFireMode(true);
			break;
		case BonusType.ForceField:
			ToggleFireMode(false);
			ToggleForceField(true);
			break;
		case BonusType.Slow:
			ToggleFireMode(false);
			ball.ToggleSlowMode(true);
			break;
		}
	}

	void Expand(){
		LeanTween.scaleX(gameObject, 1.5f, 0.5f);
	}

	void Shrink(){
		LeanTween.scaleX(gameObject, 1, 0.5f);
	}

	void ToggleFireMode(bool toggle){
		_canFire = toggle;

		if (toggle){
			_spriteRenderer.color = Color.red;
		}else{
			_spriteRenderer.color = _orgColor;
		}
	}

	void ToggleForceField(bool toggle){
		shield.SetActive(toggle);
	}


	void Abort(){
		Shrink();
		ToggleFireMode(false);
		ball.ToggleSlowMode(false);
		ToggleForceField(false);
	}


}
