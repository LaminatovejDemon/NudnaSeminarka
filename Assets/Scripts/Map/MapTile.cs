using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour {
	public Texture2D Icon;
	public Map.TileType Type;

	void Start(){
		Map.Instance.UpdateTile (gameObject, Type, Icon);
	}
}
