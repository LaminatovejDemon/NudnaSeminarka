using UnityEngine;
using System.Collections;

public class Button03_Position : MonoBehaviour {

	public Camera UICam;

	void OnMouseDown ()
	{
		Debug.Log ("BUTTON MAP");
	}

	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () {

		Vector3 worldPoint = UICam.ScreenToWorldPoint(new Vector3(240,Screen.height-70, 0));
		transform.position = worldPoint;

	
	}
}
