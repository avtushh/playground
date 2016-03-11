using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BrickManager : MonoBehaviour {

	public event Action<Brick> OnBrickHit = (b) => {};
	public event Action OnClearBoard = () => {};

	public Transform leftWall, rightWall;

	public GameObject brickPrefab;

	List<Brick> _bricks;

	void Start () {
		
		StartCoroutine(SpawnBricks());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator SpawnBricks(){
		var leftWallBounds = leftWall.GetComponent<Collider2D>().bounds;
		var rightWallBounds = rightWall.GetComponent<Collider2D>().bounds;

		GameObject obj = GameObject.Instantiate(brickPrefab) as GameObject;
		var brickBounds = obj.GetComponent<Collider2D>().bounds;
		float brickWidth = brickBounds.size.x;
		float brickHeight = brickBounds.size.y;

		Destroy (obj);

		float padding = brickWidth / 10;

		float boardWidth = rightWallBounds.min.x - leftWallBounds.max.x - brickWidth;
		int numColumns = (int)(boardWidth / brickWidth) - 1;

		float boardTop = leftWallBounds.max.y - leftWallBounds.size.x * 3;
		float boardLeft = leftWallBounds.max.x;

		float x = boardLeft + boardWidth / 2;
		float xRight = x;
		float xLeft = x;
		float y = boardTop;

		_bricks = new List<Brick>();

		for (int row = 0; row < 7; row++) {
			int ciks = UnityEngine.Random.Range(0, numColumns / 2 + 1);
			ciks *= 2;
			for (int col = 0; col < ciks; col++){
				obj = GameObject.Instantiate(brickPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
				obj.transform.SetParent(transform);
				var brick = obj.GetComponent<Brick>();
				brick.OnHit += HandleBrickHit;
				_bricks.Add(brick);
				if (col % 2 == 1){
					xLeft -= brickWidth + padding;
					x = xLeft;
				}else{
					xRight += brickWidth + padding;
					x = xRight;
				}
				yield return new WaitForEndOfFrame();
			}
			x = boardLeft + boardWidth / 2;
			xRight = x;
			xLeft = x;
			y -= brickHeight + padding*1.5f;
		}
	}

	void HandleBrickHit(Brick brick){

		OnBrickHit(brick);

		_bricks.Remove(brick);

		if (_bricks.Count == 0){
			OnClearBoard();
		}
	}
}
