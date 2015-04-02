using UnityEngine;
using System.Collections;

public class MainCamera_Movement : MonoBehaviour
{
	public pole_01 targetpolecondition_01;
	public float mousex;
	public float mousey;
	public Vector3 _TargetPosition = new Vector3 (1, 0.5f, 1);
	Vector3 _CameraPosition;
	public Vector3 _PlayerPosition = new Vector3 (1, 0.5f, 1);
	public float _CameraDistance = 1.0f;
	//public string _MouseStatus;
	public float _MouseTap; //Time when mouse tap happens
	
	// Use this for initialization
	
	
	void Upmovement ()
	{
		//_MouseStatus = "disabled";
		_TargetPosition += Quaternion.AngleAxis(_TargetAngle,Vector3.up) * new Vector3(0, 0, 1);
		int i = (int) (_TargetPosition.x+0.5f);
		int ii = (int) (_TargetPosition.z+0.5f);
		_PlayerPosition = _TargetPosition;
		
		if (targetpolecondition_01.TestField[i,ii]==1)
		{
			_TargetPosition -= Quaternion.AngleAxis(_TargetAngle,Vector3.up) * new Vector3(0, 0, 1);
			_PlayerPosition = _TargetPosition;
		}
	}
	
	
	void Downmovement ()
	{
		//_MouseStatus = "disabled";
		_TargetPosition -= Quaternion.AngleAxis(_TargetAngle,Vector3.up) *  new Vector3 (0, 0, 1);
		int i = (int) (_TargetPosition.x+0.5f);
		int ii = (int) (_TargetPosition.z+0.5f);
		_PlayerPosition = _TargetPosition;
		
		if (targetpolecondition_01.TestField[i,ii]==1)
		{
			_TargetPosition += Quaternion.AngleAxis(_TargetAngle,Vector3.up) *  new Vector3 (0, 0, 1);
			_PlayerPosition = _TargetPosition;
			
		}
	}
	
	void Leftturning ()
	{
		//_MouseStatus = "disabled";
		_TargetAngle -= 90;
	}
	
	void Rightturning ()
	{
		//_MouseStatus = "disabled";
		_TargetAngle += 90;
	}
	
	void Start ()
	{
		_TargetPosition = new Vector3 (1, 0.4f, 1);
		_PlayerPosition = new Vector3 (1, 0.4f, 1);
		_CameraPosition = Camera.main.transform.position;
		//_MouseStatus = "disabled";
		
		
	}
	
	float _TargetAngle;
	Vector3 _ActualEulers = Vector3.zero;
	float distancedrag;
	bool distanceon = true;
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log ("MOUSE STATUS JE" + _MouseStatus);
		
		//if (Input.anyKeyDown)
		//{
		//	Debug.Log("pozice kamery je:" + _TargetPosition);
		//}
		
		Vector3 mouse1 = new Vector3(10,25,15);
		Vector3 mouse2 = new Vector3(22, 1, 42);
		float magnitude_ = (mouse2 - mouse1).magnitude;
		
		
		if (Input.GetMouseButtonDown(0))
		{
			mousex = (Input.mousePosition.x);
			mousey = (Input.mousePosition.y);
			//Debug.Log("Pressed left click at" + mousex + "," + mousey);
			distanceon = true;
			
		}


				
		if (distanceon) 
		{
			distancedrag = Mathf.Sqrt ((Input.mousePosition.x - mousex) * (Input.mousePosition.x - mousex) + (Input.mousePosition.y - mousey) * (Input.mousePosition.y - mousey));
			//Debug.Log("Distance je: "+distancedrag);
		}

		if (Input.GetMouseButtonUp(0) && (distanceon == true) && (distancedrag < 29.9999f)) 
		    {
			_MouseTap = Time.time;
			distanceon = false;
			distancedrag = 0f;
		}

		
		
		//		if (distancedrag > 50)
		//		{
		//			Debug.Log("POSUNULI JSME SE O 50 JEDNOTEK PYCO");
		//		}
		//		Debug.Log ("hodnota X JE: " + mousex);
		
		//		if (Input.GetMouseButtonUp(0))
		if (distancedrag > 30) 
		{
			//_MouseStatus = "slide";
			float mousexx = (Input.mousePosition.x);
			float mouseyy = (Input.mousePosition.y);
			//Debug.Log("Released left click at" + mousexx + "," + mouseyy);
			
			float mousexdifference = mousexx-mousex;
			float mouseydifference = mouseyy-mousey;
			
			if ((mousexdifference > 0) && (Mathf.Abs(mousexdifference) > Mathf.Abs(mouseydifference)))
			{
				//Debug.Log("OTACIME SE DOPRAVA");
				Rightturning ();
			}
			if ((mousexdifference < 0) && (Mathf.Abs(mousexdifference) > Mathf.Abs(mouseydifference)))
			{
				//Debug.Log("OTACIME SE DOLEVA");
				Leftturning ();
			}
			if ((mouseydifference > 0) && (Mathf.Abs(mouseydifference) > Mathf.Abs(mousexdifference)))
			{
				//Debug.Log("JDEME DOPREDU");
				Upmovement ();
			}
			if ((mouseydifference < 0) && (Mathf.Abs(mouseydifference) > Mathf.Abs(mousexdifference)))
			{
				//Debug.Log("JDEME DOZADU");
				Downmovement ();
			}
			
			mousex = (Input.mousePosition.x);
			mousey = (Input.mousePosition.y);
			distancedrag = 0f;
			distanceon = false;
			
		}
		
		
		
		if (Input.GetKeyDown ("up")) 
		{
			Upmovement ();
			/*
			_TargetPosition += Quaternion.AngleAxis(_TargetAngle,Vector3.up) * new Vector3(0, 0, 1);
			int i = (int) (_TargetPosition.x+0.5f);
			int ii = (int) (_TargetPosition.z+0.5f);

			if (targetpolecondition_01.TestField[i,ii]=="1")
			{
				_TargetPosition -= Quaternion.AngleAxis(_TargetAngle,Vector3.up) * new Vector3(0, 0, 1);

			}
*/
			
		}
		
		if (Input.GetKeyDown ("down")) 
		{
			Downmovement ();
			
			/*
			_TargetPosition -= Quaternion.AngleAxis(_TargetAngle,Vector3.up) *  new Vector3 (0, 0, 1);
			int i = (int) (_TargetPosition.x+0.5f);
			int ii = (int) (_TargetPosition.z+0.5f);
			if (targetpolecondition_01.TestField[i,ii]=="1")
			{
				_TargetPosition += Quaternion.AngleAxis(_TargetAngle,Vector3.up) *  new Vector3 (0, 0, 1);
			
			}
			*/
		}
		
		
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			Leftturning ();
			//			_TargetAngle -= 90;
		}
		
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			Rightturning ();
			//			_TargetAngle += 90;
		}
		
		
		if (_ActualEulers.y != _TargetAngle) 
		{
			_ActualEulers.y += (_TargetAngle - _ActualEulers.y) * Time.deltaTime * 5;
			
			if (Mathf.Abs (_ActualEulers.y - _TargetAngle) < 0.5f) 
			{
				_ActualEulers.y = _TargetAngle;
			}
			
			Camera.main.transform.eulerAngles = _ActualEulers;
		}
		
		
		if (_CameraPosition != _TargetPosition) 
		{
			_CameraPosition += (_TargetPosition - _CameraPosition) * Time.deltaTime * 5;
			
			if ((_CameraPosition - _TargetPosition).magnitude < 0.05f) 
			{
				_CameraPosition = _TargetPosition;
			}
		}
		
		Camera.main.transform.position = _CameraPosition + Camera.main.transform.rotation * Vector3.back * _CameraDistance;
	}
}
