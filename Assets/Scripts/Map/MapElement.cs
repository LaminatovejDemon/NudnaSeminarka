using UnityEngine;
using System.Collections;

public class MapElement : MonoBehaviour {
	public Texture Icon;
	Vector3 _position = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

	void Update () {
		if (transform.position != _position) {
			_position = transform.position;
			Map.Instance.UpdateElement (this);
		}
	}
}
