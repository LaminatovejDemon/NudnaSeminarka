using UnityEngine;
using System.Collections;

public class MainCamera_Movement : MonoBehaviour
{
	// inpector properties
	public float RotationTime = 0.25f;
	public float MovementTime = 0.3f;
	public float CameraDistance = 0.5f;

	pole_01 targetpolecondition_01;
	float mousex;
	float mousey;
	public Vector3 _TargetPosition { get; private set;}
	Vector3 _CameraPosition;
	public Vector3 _PlayerPosition {get; private set;}
	//public string _MouseStatus;
	public float _MouseTap {get; private set;}

	void MoveForward (){
		_TargetPosition += Quaternion.AngleAxis(_TargetAngle,Vector3.up) * new Vector3(0, 0, 1);
		GeneralHelpers.Instance.Lerp (_CameraPosition, _TargetPosition, MovementTime, OnPositionChange);
	}

	void MoveBackwards (){
		_TargetPosition -= Quaternion.AngleAxis(_TargetAngle,Vector3.up) *  new Vector3 (0, 0, 1);
		GeneralHelpers.Instance.Lerp (_CameraPosition, _TargetPosition, MovementTime, OnPositionChange);
	}
	
	void TurnLeft (){
		while (_TargetAngle >= 360) {
			_TargetAngle -= 360;
		}
		while (_TargetAngle < 0) {
			_TargetAngle += 360;
		}
		_TargetAngle = _TargetAngle - 90;

		GeneralHelpers.Instance.Lerp (Camera.main.transform.eulerAngles.y, _TargetAngle, RotationTime, OnEulerAngleYChange);
	}
	
	void TurnRight (){
		while (_TargetAngle >= 360) {
			_TargetAngle -= 360;
		}
		while (_TargetAngle < 0) {
			_TargetAngle += 360;
		}
		_TargetAngle += 90;

		GeneralHelpers.Instance.Lerp (Camera.main.transform.eulerAngles.y, _TargetAngle, RotationTime, OnEulerAngleYChange);
	}
	
	void Start (){
		_TargetPosition = new Vector3 (1, 0.4f, 1);
		_PlayerPosition = _TargetPosition;
		_CameraPosition = Camera.main.transform.position;
	}
	
	float _TargetAngle = 0;
	float distancedrag = 0;
	bool distanceon = false;
	
	// Update is called once per frame
	void Update (){		
		if (Input.GetMouseButtonDown(0)){
			mousex = (Input.mousePosition.x);
			mousey = (Input.mousePosition.y);
			//Debug.Log("Pressed left click at" + mousex + "," + mousey);
			distanceon = true;
			
		}
			
		if (distanceon) {
			distancedrag = Mathf.Sqrt ((Input.mousePosition.x - mousex) * (Input.mousePosition.x - mousex) + (Input.mousePosition.y - mousey) * (Input.mousePosition.y - mousey));
			//Debug.Log("Distance je: "+distancedrag);
		}

		if (Input.GetMouseButtonUp(0) && (distanceon == true) && (distancedrag < 29.9999f)) {
			_MouseTap = Time.time;
			distanceon = false;
			distancedrag = 0f;
		}
			
		if (distancedrag > 30) {
			//_MouseStatus = "slide";
			float mousexx = (Input.mousePosition.x);
			float mouseyy = (Input.mousePosition.y);
			//Debug.Log("Released left click at" + mousexx + "," + mouseyy);
			
			float mousexdifference = mousexx-mousex;
			float mouseydifference = mouseyy-mousey;
			
			if ((mousexdifference > 0) && (Mathf.Abs(mousexdifference) > Mathf.Abs(mouseydifference))){
				//Debug.Log("OTACIME SE DOPRAVA");
				TurnRight ();
			}
			if ((mousexdifference < 0) && (Mathf.Abs(mousexdifference) > Mathf.Abs(mouseydifference))){
				//Debug.Log("OTACIME SE DOLEVA");
				TurnLeft ();
			}

			if ((mouseydifference > 0) && (Mathf.Abs(mouseydifference) > Mathf.Abs(mousexdifference))){
				//Debug.Log("JDEME DOPREDU");
				MoveForward ();
			}
			if ((mouseydifference < 0) && (Mathf.Abs(mouseydifference) > Mathf.Abs(mousexdifference))){
				//Debug.Log("JDEME DOZADU");
				MoveBackwards ();
			}
			
			mousex = (Input.mousePosition.x);
			mousey = (Input.mousePosition.y);
			distancedrag = 0f;
			distanceon = false;
			
		}

		if (Input.GetKeyDown ("up")) {
			MoveForward ();

		}
		
		if (Input.GetKeyDown ("down")) {
			MoveBackwards ();
		}
		
		
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			TurnLeft ();
		}
		
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			TurnRight ();
		}

		Camera.main.transform.position = _CameraPosition + Camera.main.transform.rotation * Vector3.back * CameraDistance;
	}

	void OnEulerAngleYChange(float value){
		Vector3 temp_ = Camera.main.transform.eulerAngles;
		temp_.y = value;
		Camera.main.transform.eulerAngles = temp_;
	}

	void OnPositionChange(Vector3 value){
		_CameraPosition = value;
	}
}
