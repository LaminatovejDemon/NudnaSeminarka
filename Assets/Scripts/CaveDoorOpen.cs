using UnityEngine;
using System.Collections;

public class CaveDoorOpen : MonoBehaviour {

	bool isOpened;
	Animator DoorAnimator;
	public pole_01 PoleScript;
	public MainCamera_Movement PositioningScript;


	// Use this for initialization
	void Start () 
	{
		isOpened = false;
		DoorAnimator = GetComponent<Animator>();
	}

	void OnMouseDown()
	{
		if (
			!isOpened 
		    && Vector3.Distance (PositioningScript._PlayerPosition, transform.position) < 1.5f
		    && (Time.time - PositioningScript._MouseTap) < 1f)
		{
			Debug.Log("OTVIRAME DVERE");
			DoorAnimator.SetTrigger("Open");
			isOpened = true;
			PoleScript.TestField[7,7]=0;
		} else if (
			isOpened
			&& Vector3.Distance (PositioningScript._PlayerPosition, transform.position) < 1.5f
			&& ((Time.time - PositioningScript._MouseTap) < 1f))
		{
			Debug.Log("ZAVIRAME DVERE");
			DoorAnimator.SetTrigger("Close");
			isOpened = false;
			PoleScript.TestField[7,7]=1;
		}

	}


	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ( Time.time - PositioningScript._MouseTap);
	
	}
}
