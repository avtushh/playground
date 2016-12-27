using UnityEngine;
using System.Collections;

public class PowerUpsManager : MonoBehaviour {
	
	public Transform left, right, top, bottom;

	Rect _boardBounds;
	bool _paused = false;
	PowerUp _currentPowerUp = null;

	public GameObject powerupPrefab; 

	public int baseDelay = 8;
	public int variantDelay = 5;

	public void Pause(){
		_paused = true;
		if (_currentPowerUp != null){
			Destroy(_currentPowerUp.gameObject);
			_currentPowerUp = null;
		}
	}

	public void Resume(){
		_paused = false;
	}

	void Start () {
		_boardBounds = new Rect(left.position.x, bottom.position.y, right.position.x - left.position.x, top.position.y - bottom.position.y);

		StartCoroutine(CreatePowerUpsCoro());
	}

	IEnumerator CreatePowerUpsCoro ()
	{
		while(true){
			if (_paused || _currentPowerUp != null){
				yield return null;
			}else{
				int delay = baseDelay + Random.Range(-variantDelay, variantDelay+1);
				yield return new WaitForSeconds(delay);
				CreateRandomPowerUp();
			}
		}
	}

	void CreateRandomPowerUp ()
	{
		float x = Random.Range(_boardBounds.xMin, _boardBounds.xMax);
		float y = Random.Range(_boardBounds.yMin, _boardBounds.yMax);
		var go = Instantiate(powerupPrefab, new Vector3(x, y, left.position.z ), Quaternion.identity) as GameObject;

		_currentPowerUp = go.GetComponent<PowerUp>();
		_currentPowerUp.HitEvent += _currentPowerUp_HitEvent;
	}

	void _currentPowerUp_HitEvent ()
	{
		_currentPowerUp = null;
	}
}
