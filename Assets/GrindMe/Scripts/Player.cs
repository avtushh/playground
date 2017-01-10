﻿using UnityEngine;
using System.Collections;

namespace Patterns
{
	[RequireComponent (typeof(Controller2D))]
	public class Player : MonoBehaviour
	{
		public float jumpHeight = 4;
		public float timeToJumpApex = .4f;


		float accelarationTimeAirBorne = .2f;
		float accelarationTimeGrounded = .1f;
		float moveSpeed = 6;

		float gravity;
		float jumpVelocity;
		float velocityXSmoothing;
		Vector3 velocity;

		Controller2D controller;

		void Start ()
		{
			controller = GetComponent<Controller2D> ();

			gravity = -(2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
			jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);


		}

		void Update(){

			if (controller.collisions.above || controller.collisions.below){
				velocity.y = 0;
			}

			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below){
				velocity.y = jumpVelocity;
			}



			float targetVelocityX = input.x * moveSpeed;

			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below?accelarationTimeGrounded:accelarationTimeAirBorne));

			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);
		}

	 
	}

}
