using UnityEngine;
using System.Collections;

public class FollowObjectIfPassCenter : MonoBehaviour {

    public Transform objectToFollow;

    bool _isMoving;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        var viewPortPos = Camera.main.WorldToViewportPoint(objectToFollow.position);

        if (viewPortPos.y > 0.5f)
        {
           
            var pos = Camera.main.transform.position;

            pos.y = Mathf.Lerp(Camera.main.transform.position.y, objectToFollow.position.y, Time.deltaTime);

            Camera.main.transform.position = pos;
        }

        return;

            if (!_isMoving)
        {
                 if (viewPortPos.y > 0.5f)
            {

                LeanTween.moveY(gameObject, objectToFollow.transform.position.y, 0.5f).setEase(LeanTweenType.easeInSine).setOnComplete(() => _isMoving = false);
    
            }
        }
       
	}
}
