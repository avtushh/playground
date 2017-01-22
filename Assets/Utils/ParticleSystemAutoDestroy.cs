using UnityEngine;
using System.Collections;
using System;

public class ParticleSystemAutoDestroy : MonoBehaviour 
{
	private ParticleSystem ps;

	public event Action OnFinishedPlaying = () => {};

	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Update() 
	{
		if(ps)
		{
			if(!ps.IsAlive())
			{
				OnFinishedPlaying();
				Destroy(gameObject);
			}
		}
	}
}