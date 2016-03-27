using UnityEngine;
using System.Collections;

public class ElementDoor : MapTile {
	public Texture2D OpenedIcon;
	bool isOpened = false;
	float _TimeStamp = -1;
	Vector3 _DownPos = Vector3.zero;

	void OnMouseDown() {
		_TimeStamp = Time.time;
		_DownPos = Input.mousePosition;
	}

	void OnMouseUp() {
		if ( Vector3.Distance (Camera.main.transform.position, transform.position) >= 2.0f 
			|| Time.time - _TimeStamp > 0.15f 
			|| (Input.mousePosition - _DownPos).magnitude > 10.0f ) {
			return;
		}

		Debug.Log ((Input.mousePosition - _DownPos).magnitude);

		isOpened = !isOpened;
		GetComponent<Animator>().SetTrigger(isOpened ? "Open" : "Close");
		Map.Instance.UpdateTile(gameObject, isOpened ? Map.TileType.DoorOpened : Map.TileType.DoorClosed, isOpened ? OpenedIcon : Icon);
	}
}
