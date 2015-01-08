using UnityEngine;
using System.Collections;

public class CaveDoorOpen : MonoBehaviour {

	string DoorStatus;
	Animator DoorAnimator;
	public pole_01 PoleScript;
	public MainCamera_Movement PositioningScript;


	// Use this for initialization
	void Start () 
	{
		DoorStatus = "Closed";
		DoorAnimator = GetComponent<Animator>();

	}

	void OnMouseDown()
	{
		if ((DoorStatus == "Closed")&&(Vector3.Distance (PositioningScript._PlayerPosition, transform.position) < 1.5f))
		{
			Debug.Log("OTVIRAME DVERE");
			DoorAnimator.SetTrigger("Open");
			DoorStatus = "Opening";
			PoleScript.TestField[7,7]=0;

		}

		if ((DoorStatus == "Opened")&&(Vector3.Distance (PositioningScript._PlayerPosition, transform.position) < 1.5f))
		{
			Debug.Log("ZAVIRAME DVERE");
			DoorAnimator.SetTrigger("Close");
			DoorStatus = "Closing";
			PoleScript.TestField[7,7]=1;

		}

		if (DoorStatus == "Closing") 
		{
			DoorStatus = "Closed";
		}
		if (DoorStatus == "Opening") 
		{
			DoorStatus = "Opened";
		}



	}


	// Update is called once per frame
	void Update () 
	{
	
	}
}
