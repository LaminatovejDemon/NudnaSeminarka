using UnityEngine;
using System.Collections;

public class Button02_Position : MonoBehaviour {

	public Camera UICam;

	void OnMouseDown ()
	{
		Debug.Log ("BUTTON TORCHLIGHT PRESSED");
	}

	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () {

		Vector3 worldPoint = UICam.ScreenToWorldPoint(new Vector3(155,Screen.height-64, 0));
		transform.position = worldPoint;

	
	}
}
