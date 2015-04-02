using UnityEngine;
using System.Collections;

public class Button03_MAP_clicked : MonoBehaviour {

	public Camera UICam;
	
	void OnMouseDown ()
	{
		Debug.Log ("BUTTON MAP");
		Camera.main.enabled = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
