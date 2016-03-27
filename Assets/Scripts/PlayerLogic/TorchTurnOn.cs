using UnityEngine;
using System.Collections;

public class TorchTurnOn : MonoBehaviour {

	public Light_Intensity LightScript;
	public MainCamera_Movement PositioningScript;

	void OnMouseDown ()
	{
		//Debug.Log (Vector3.Distance(PositioningScript._PlayerPosition,transform.position));
		//1.5 trigger distance in outtext
		if (Vector3.Distance (PositioningScript._TargetPosition, transform.position) < 1.5f) 
		{
			LightScript.PochodenRefresh();
		}

	}
}
