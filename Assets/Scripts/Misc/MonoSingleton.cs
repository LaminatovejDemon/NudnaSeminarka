using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour
	where T : MonoSingleton<T> 
{
	private static T _instance = null;

	static string GetTag(){
		return typeof(T).ToString();
	}

	public static T Instance
	{
		get{
			if (_instance == null) {
				string tag_ = GetTag();
				GameObject gameLogic_ = GameObject.FindWithTag (tag_);
				if (gameLogic_ == null) {
					gameLogic_ = (GameObject)GameObject.Instantiate (Resources.Load (tag_));
					gameLogic_.name = tag_;
				}
				_instance = gameLogic_.GetComponent<T> ();

				if (_instance == null) {
					_instance = gameLogic_.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}
}
