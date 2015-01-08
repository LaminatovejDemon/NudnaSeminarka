using UnityEngine;
using System.Collections;

public class Button01_Position : MonoBehaviour {

	public Camera UICam;

	void OnMouseDown ()
	{
		Debug.Log ("BUTTON PRESSED");
	}

	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () {

		Vector3 worldPoint = UICam.ScreenToWorldPoint(new Vector3(70,Screen.height-70, 0));
		transform.position = worldPoint;

	
	}
}
