using UnityEngine;
using System.Collections;

public class Button03_MAP_clicked : MonoBehaviour {

	public Camera UICam;
	bool _MapDisplayed;
	
	void OnMouseDown ()
	{
		_MapDisplayed = !_MapDisplayed;
		Debug.Log ("BUTTON MAP" + _MapDisplayed);
		Map.Instance.DisplayMap(_MapDisplayed);
	}
}
