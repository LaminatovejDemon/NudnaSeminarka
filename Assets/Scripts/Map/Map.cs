using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoSingleton<Map> {

	public TextAsset MapAsset;
	public GameObject MapTileTemplate;
	public Texture2D MapTilePath;
	public float MapScale = 1.0f;

	enum MapFlag{
		Hidden,
		Seen,
		Reached,
	};

	public enum TileType{
		Block,
		Path,
		DoorClosed,
		DoorOpened,
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
		public MapTile(MapFlag flag, GameObject tile, TileType type){
			_Tile = tile;
			_MapFlag = flag;
			_TileType = type;
			_Tile.SetActive(flag > MapFlag.Seen);
		}

		public void SetFlag(MapFlag flag){
			if (_MapFlag >= flag) {
				return;
			}
			_MapFlag = flag;
			switch (flag) {
			case MapFlag.Hidden:
				_Tile.gameObject.SetActive (false);
				break;
			
			case MapFlag.Seen:
				_Tile.gameObject.SetActive (true);
				_Tile.GetComponent<MeshRenderer> ().material.SetColor("_TintColor", new Color(0.3f, 0.3f, 0.3f));
				break;

			case MapFlag.Reached:
				_Tile.gameObject.SetActive (true);
				_Tile.GetComponent<MeshRenderer> ().material.SetColor("_TintColor", Color.white);
				break;
			}

			_Tile.gameObject.SetActive(true);
		}

		public GameObject _Tile{ get; private set; }
		public MapFlag _MapFlag;
		public TileType _TileType;
	};
		
	List<ElementTile> _ElementTiles;
	Camera _UICamera;
	MapTile[,] LoadedMap;
	int _Width;
	int _Height;

	void Initialise(){
		if (LoadedMap != null) {
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
				if (int.Parse (columns_ [j]) == (int)TileType.Path) {
					CreateTile (i, j, TileType.Path);
				} else {
					LoadedMap [i, j] = null;
				}
			}
		}

		float scaleX_ = _UICamera.orthographicSize / _Width * _UICamera.aspect * 2.0f;
		float scaleY_ = _UICamera.orthographicSize / _Height * 2.0f;

		transform.localScale = Vector3.one * Mathf.Min (scaleX_, scaleY_) * MapScale;
	}

	void CreateTile(int x, int y, TileType type){
		GameObject tile_ = GameObject.Instantiate (MapTileTemplate);
		LoadedMap [x, y] = new MapTile (MapFlag.Hidden, tile_, type);
		tile_.transform.parent = transform;
		tile_.transform.localRotation = Quaternion.identity;
		tile_.transform.localScale = Vector3.one;
		tile_.name = type.ToString() + "_" + x + "_" + y;
		tile_.transform.localPosition = Vector3.up * (y - _Width / 2.0f + 0.5f) + Vector3.right * (x - _Height / 2.0f + 0.5f); 
	}

	public bool CanWalk(Vector3 destination){
		int x_ = (int)(destination.x + 0.5f);
		int y_ = (int)(destination.z + 0.5f);

		if (LoadedMap [x_, y_] == null) {
			return false;
		}

		return (LoadedMap [x_, y_]._TileType == Map.TileType.DoorOpened || LoadedMap [x_, y_]._TileType == Map.TileType.Path);
	}

	public void DisplayMap(bool state){
		Initialise ();

		this.gameObject.SetActive(state);
	}
		
	public void UpdateTile(GameObject source, TileType targetType, Texture2D texture = null) {
		Initialise ();

		int x_ = (int)(source.transform.position.x + 0.5f);
		int y_ = (int)(source.transform.position.z + 0.5f);
	
		if (LoadedMap [x_, y_] == null) {
			CreateTile (x_, y_, targetType);
		} else {
			LoadedMap [x_, y_]._TileType = targetType;
		}
		LoadedMap [x_, y_]._Tile.transform.localRotation = Quaternion.AngleAxis (source.transform.eulerAngles.y, Vector3.forward);

		if (texture != null) {
			LoadedMap [x_, y_]._Tile.GetComponent<MeshRenderer> ().material.mainTexture = texture;
		}

		UpdateElement (Camera.main.GetComponent<MapElement> ());
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

		LoadedMap [x_, y_].SetFlag (MapFlag.Reached);

		UncoverVisibleMap (source.Visibility, x_, y_, lookX_, lookY_);
	}

	void UncoverVisibleMap(int depth, int x, int y, float lookX, float lookY)
	{
		for (int i = 1; i < depth; ++i) {
			int x_ = x + (int)(lookX * i);
			int y_ = y + (int)(lookY * i);
			if (x_ < 0 || x_ >= _Width || y_ < 0 || y_ >= _Height || LoadedMap[x_, y_] == null ) {
				return;
			} else {
				LoadedMap [x_, y_].SetFlag (MapFlag.Seen);	

				switch (LoadedMap [x_, y_]._TileType) {
				case TileType.DoorClosed:
				case TileType.Block:
					return;
				default:
					break;
				}
			}
		}
	}
}
