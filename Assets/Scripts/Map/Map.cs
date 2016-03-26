using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoSingleton<Map> {

	public TextAsset MapAsset;
	public GameObject MapTileTemplate;
	public Texture2D MapTileSeenTexture;
	public Texture2D MapTileReachedTexture;
	public float MapScale = 1.0f;

	enum MapFlag{
		Hidden,
		Seen,
		Reached,
	};
		
	struct ElementTile{
		public ElementTile(MapElement source, GameObject tile){
			_Source = source;
			_Tile = tile;
		}
		public MapElement _Source { get; private set;}
		public GameObject _Tile {get; private set;}
	};
		
	class MapTile{
		public MapTile(MapFlag flag, GameObject tile){
			_Tile = tile;
			_MapFlag = flag;
			_Tile.SetActive(flag > MapFlag.Seen);
		}

		public void SetFlag(MapFlag flag, Texture2D texture){
			if (_MapFlag >= flag) {
				return;
			}
			_MapFlag = flag;
			_Tile.GetComponent<MeshRenderer> ().material.mainTexture = texture;
			_Tile.gameObject.SetActive(true);
		}

		public GameObject _Tile{ get; private set; }
		public MapFlag _MapFlag;
	};
		
	List<ElementTile> _ElementTiles;
	Camera _UICamera;
	bool _Initialised = false;
	MapTile[,] LoadedMap;
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
		this.gameObject.SetActive(false);

		if (LoadedMap == null) {
			LoadMapAsset ();
		}
		_Initialised = true;
	}

	void LoadMapAsset(){
		string[] rows_ = MapAsset.text.Split (new[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		string[] columns_ = rows_ [0].Split (new[]{';'}, System.StringSplitOptions.RemoveEmptyEntries );

		_Width = rows_.Length;
		_Height = columns_.Length;

		LoadedMap = new MapTile[_Width, _Height];

		for (int i = 0; i < rows_.Length; ++i) {
			columns_ = rows_[i].Split(new[]{';'}, System.StringSplitOptions.RemoveEmptyEntries);
			for (int j = 0; j < columns_.Length; ++j) {
				if (int.Parse (columns_ [j]) == 0) {
					GameObject tile_ = GameObject.Instantiate (MapTileTemplate);
					LoadedMap [i, j] = new MapTile (MapFlag.Hidden, tile_);
					tile_.transform.parent = transform;
					tile_.transform.localRotation = Quaternion.identity;
					tile_.name = "Tile_" + i + "_" + j;
					tile_.transform.localPosition = Vector3.up * (j - _Width / 2.0f + 0.5f) + Vector3.right * (i - _Height / 2.0f + 0.5f); 
				} else {
					LoadedMap [i, j] = null;
				}
			}
		}

		float scaleX_ = _UICamera.orthographicSize / _Width * _UICamera.aspect * 2.0f;
		float scaleY_ = _UICamera.orthographicSize / _Height * 2.0f;

		transform.localScale = Vector3.one * Mathf.Min (scaleX_, scaleY_) * MapScale;
	}

	public bool CanWalk(Vector3 destination){
		int x_ = (int)(destination.x + 0.5f);
		int y_ = (int)(destination.z + 0.5f);
		return (LoadedMap [x_, y_] != null);
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
			_ElementTiles.Add(new ElementTile(source, GameObject.Instantiate(MapTileTemplate)));
			index_ = _ElementTiles.Count - 1;
			_ElementTiles [index_]._Tile.transform.parent = this.transform;
			_ElementTiles [index_]._Tile.transform.localScale = Vector3.one;
			_ElementTiles [index_]._Tile.name = source.gameObject.name;
			_ElementTiles [index_]._Tile.GetComponent<MeshRenderer> ().material.mainTexture = _ElementTiles [index_]._Source.Icon;
		}

		_ElementTiles [index_]._Tile.transform.localPosition = Quaternion.AngleAxis(90, Vector3.left) * _ElementTiles [index_]._Source.transform.position + new Vector3(-_Width/2.0f + 0.5f, -_Height/2.0f + 0.5f,0);
		_ElementTiles [index_]._Tile.transform.localRotation = Quaternion.AngleAxis (_ElementTiles[index_]._Source.transform.localEulerAngles.y, Vector3.back);

		if (source.Visibility <= 0) {
			return;
		}

		int x_ = (int)(source.transform.localPosition.x + 0.5f);
		int y_ = (int)(source.transform.localPosition.z + 0.5f);
		float lookX_ = Mathf.Sin(source.transform.localEulerAngles.y * Mathf.PI / 180.0f);
		float lookY_ = Mathf.Cos(source.transform.localEulerAngles.y * Mathf.PI / 180.0f);

		if (LoadedMap [x_, y_] == null) {
			Debug.Log ("ASSERT: character reached out of map bounds ("+x_+", "+y_+").");
			return;
		}

		LoadedMap [x_, y_].SetFlag (MapFlag.Reached, MapTileReachedTexture);

		for (int i = 1; i < source.Visibility; ++i) {
			int dstX_ = x_ + (int)(lookX_ * i);
			int dstY_ = y_ + (int)(lookY_ * i);
			if (dstX_ < 0 || dstX_ >= _Width || dstY_ < 0 || dstY_ >= _Height || LoadedMap [dstX_, dstY_] == null) {
				break;
			} else {
				LoadedMap [dstX_, dstY_].SetFlag (MapFlag.Seen, MapTileSeenTexture);	
			}
		}
	}
}
