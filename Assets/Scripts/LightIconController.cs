﻿using UnityEngine;
using System.Collections;

public class LightIconController : MonoBehaviour 
{
	public Light_Intensity LightIntensityScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Vector3 worldPoint = UICam.ScreenToWorldPoint(new Vector3(100,Screen.height-70, 0));
		//transform.position = worldPoint;
		particleEmitter.minSize = LightIntensityScript.PochodenLife/3;
		particleEmitter.maxSize = 0.8f*LightIntensityScript.PochodenLife;
		particleEmitter.localVelocity = new Vector3(0,0.3f+LightIntensityScript.PochodenLife/1.75f,0);

	}
}
