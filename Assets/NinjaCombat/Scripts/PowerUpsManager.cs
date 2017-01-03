using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerUpsManager : MonoBehaviour {
	
	public Transform left, right, top, bottom;

	Rect _boardBounds;
	bool _paused;
	PowerUp _currentPowerUp = null;

	public GameObject powerupPrefab; 

	public int baseDelay = 8;
	public int variantDelay = 5;

	public void Pause(){
		
		_paused = true;
		if (_currentPowerUp != null){
			Destroy(_currentPowerUp.gameObject);
			_currentPowerUp = null;
			StopAllCoroutines();
		}
	}

	public void Resume(){

		if (!_paused){
			return;
		}

		_paused = false;


		StartCoroutine(CreatePowerUpsCoro());
	}

	void Start () {
		_boardBounds = new Rect(left.position.x, bottom.position.y, right.position.x - left.position.x, top.position.y - bottom.position.y);
		_paused = true;
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

	bool CollidesWithObstacles(Vector3 pos){
		var obstacles =	GameObject.FindGameObjectsWithTag("Obstacle").ToList();

		Vector2 pos2d = new Vector2(pos.x, pos.y);

		var collidedObstacle = obstacles.FirstOrDefault(obs => obs.GetComponent<Collider2D>().OverlapPoint(pos2d));

		return collidedObstacle != null;
	}

	void CreateRandomPowerUp ()
	{
		int creationAttempts = 0;

		while (creationAttempts < 5){
			
			float x = Random.Range(_boardBounds.xMin, _boardBounds.xMax);
			float y = Random.Range(_boardBounds.yMin, _boardBounds.yMax);

			Vector3 createPos = new Vector3(x,y,left.position.z);

			if (CollidesWithObstacles(createPos)){
				creationAttempts++;
			}else{
				var go = Instantiate(powerupPrefab, new Vector3(x, y, left.position.z ), Quaternion.identity) as GameObject;

				_currentPowerUp = go.GetComponent<PowerUp>();
				_currentPowerUp.HitEvent += _currentPowerUp_HitEvent;	
				return;
			}


		}

	}

	void _currentPowerUp_HitEvent ()
	{
		_currentPowerUp = null;
	}
}
