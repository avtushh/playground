using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TabTale
{
	public class Grinder : ShapeHolder
	{

		public SpriteRenderer sprRenderer;
		public Sprite sprNormal, sprBloody;

		public void OnKillPlayer ()
		{
			sprRenderer.sprite = sprBloody;
		}

		protected override void OnKilled(){
			base.OnKilled();
			sprRenderer.enabled = false;
			GetComponentInChildren<Collider2D>().enabled = false;

		}

	}
}