using UnityEngine;
using System.Collections;

public class Light_Intensity : MonoBehaviour {

	//public Texture PochodenTexture;
	public Animator _PochodenAnimator;
	public float _LightDuration = 1.0F;
	public int id;
	public float _LightIntensity = 0.2F;
	float StartTime;
	public float PochodenLife = 1F;

	// Use this for initialization
	void Start () 
	{
		id = Animator.StringToHash("Pochoden_intensity");
		StartTime = Time.time;

		PochodenLife = 0F;
		_LightDuration = 0f;
		_PochodenAnimator.SetFloat(id,0f);

	}

	/*
	void OnGUI() 
		{
				if (!PochodenTexture) {
						Debug.LogError ("Please assign a texture on the inspector");
						return;
				}
				if (GUI.Button (new Rect (10, 10, 50, 50), PochodenTexture, GUIStyle.none)) {
						Debug.Log ("Clicked the button with TORCH");
						_PochodenAnimator.SetFloat(id,1f);
						PochodenLife = 1f;
						StartTime = Time.time;
				}
		}
	*/

	public void PochodenRefresh ()
	{
	
		_PochodenAnimator.SetFloat(id,1f);
		PochodenLife = 1f;
		_LightDuration = 1f;
		StartTime = Time.time;

	}


	// Update is called once per frame
	void Update () 
	{
	
		float phi = Time.time / _LightDuration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

		float phi2 = Time.time / _LightDuration * 1.637f * Mathf.PI;
		float amplitude2 = Mathf.Sin(phi2) * 0.2F + 0.2F;

//		Debug.Log ("Time-Startime" + (Time.time - StartTime));
//		Debug.Log("POCHODENLIFE: " + PochodenLife);

		if (((Time.time - StartTime) > 5f)&&(PochodenLife>0f)) //pochoden zacina ubyvat po 15sekundach
		{
			PochodenLife = 1f - 0.014f * (Time.time - StartTime -5f); //0.003 je mozna spravny cas pro final

		}
		

		if (PochodenLife>0) 
		{
			_PochodenAnimator.SetFloat(id,PochodenLife);
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			_PochodenAnimator.SetFloat(id,1f);
			PochodenLife = 1f;
			_LightDuration = 1f;
			StartTime = Time.time;
		}

		GetComponent<Light>().intensity = _LightIntensity + amplitude/7f + amplitude2/4f;
		GetComponent<Light>().range = 2.25f+1f * (_LightIntensity + amplitude/5f + amplitude2/3f)+PochodenLife;
		
	}
}
