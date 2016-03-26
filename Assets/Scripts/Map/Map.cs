using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoSingleton<Map> {

	public TextAsset MapAsset;
	public GameObject MapTile;
	public float MapScale = 1.0f;
	struct ElementTile{
		public ElementTile(MapElement source, GameObject tile){
			_Source = source;
			_Tile = tile;
		}
		public MapElement _Source { get; private set;}
		public GameObject _Tile {get; private set;}
	};

	List<ElementTile> _ElementTiles;
	Camera _UICamera;
	bool _Initialised = false;
	int [,] LoadedMap;
	int _Width;
	int _Height;

	void Initialise(){
		if (_Initialised) {
			return;
		}

		if (_UICamera == null) {
			foreach (Camera c in Camera.allCameras) {
				if (c.orthographic) {
					_UICamera = c;
					break;
				}
			}
		}

		transform.parent = _UICamera.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		if (LoadedMap == null) {
			LoadMapAsset ();
		}
	}

	void LoadMapAsset(){
		string[] rows_ = MapAsset.text.Split (new[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		string[] columns_ = rows_ [0].Split (new[]{';'}, System.StringSplitOptions.RemoveEmptyEntries );

		_Width = rows_.Length;
		_Height = columns_.Length;

		LoadedMap = new int[_Width, _Height];

		for (int i = 0; i < rows_.Length; ++i) {
			columns_ = rows_[i].Split(new[]{';'}, System.StringSplitOptions.RemoveEmptyEntries);
			for (int j = 0; j < columns_.Length; ++j) {
				LoadedMap [i, j] = int.Parse(columns_ [j]);

				if (LoadedMap [i, j] == 0) {
					GameObject tile_ = GameObject.Instantiate (MapTile);
					tile_.transform.parent = transform;
					tile_.transform.localRotation = Quaternion.identity;
					tile_.name = "Tile_" + i + "_" + j;
					tile_.transform.localPosition = Vector3.up * (j-_Width/2.0f + 0.5f) + Vector3.right * (i-_Height/2.0f + 0.5f); 
				}
			}
		}

		float scaleX_ = _UICamera.orthographicSize / _Width * _UICamera.aspect * 2.0f;
		float scaleY_ = _UICamera.orthographicSize / _Height * 2.0f;

		transform.localScale = Vector3.one * Mathf.Min (scaleX_, scaleY_) * MapScale;
	}

	public void DisplayMap(bool state){
		Initialise ();

		this.gameObject.SetActive(state);
	}

	public void UpdateElement(MapElement source){
		Initialise ();

		int index_ = -1;
		if (_ElementTiles == null) {
			_ElementTiles = new List<ElementTile> ();
		} else {
			index_ = _ElementTiles.FindIndex (x => x._Source == source);
		}
		if (index_ < 0) {
			_ElementTiles.Add(new ElementTile(source, GameObject.Instantiate(MapTile)));
			index_ = _ElementTiles.Count - 1;
			_ElementTiles [index_]._Tile.transform.parent = this.transform;
			_ElementTiles [index_]._Tile.transform.localScale = Vector3.one;
			_ElementTiles [index_]._Tile.name = source.gameObject.name;
			_ElementTiles [index_]._Tile.GetComponent<MeshRenderer> ().material.mainTexture = _ElementTiles [index_]._Source.Icon;
		}

		_ElementTiles [index_]._Tile.transform.localPosition = Quaternion.AngleAxis(90, Vector3.left) * _ElementTiles [index_]._Source.transform.position + new Vector3(-_Width/2.0f + 0.5f, -_Height/2.0f + 0.5f,0);
		_ElementTiles [index_]._Tile.transform.localRotation = Quaternion.AngleAxis (_ElementTiles[index_]._Source.transform.localEulerAngles.y, Vector3.back);
	}
}
