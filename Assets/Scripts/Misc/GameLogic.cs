using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoSingleton<GameLogic> {
	public AnimationCurve InterpolationCurve;
	public AnimationCurve InvalidMovement;

	public delegate void OnValueChanged(float value);
	public delegate void OnVector3Changed(Vector3 value);


	public enum CurveType{
		ValidMovement,
		InvalidMovement,
	};

	class LerpValueData{
		public LerpValueData(float sourceValue, float targetValue, float length, OnValueChanged onValueChanged, CurveType curveType){
			_OnValueChanged = onValueChanged;
			_SourceValue = sourceValue;
			_TargetValue = targetValue;
			_Length = length;
			_Time = 0;
			_CurveType = curveType;
		}
		public OnValueChanged _OnValueChanged;
		public float _Length;
		public float _Time;
		public float _SourceValue;
		public float _TargetValue;
		public CurveType _CurveType;
	};

	class LerpVectorData{
		public LerpVectorData(Vector3 sourceValue, Vector3 targetValue, float length, OnVector3Changed onValueChanged, CurveType curveType){
			_OnValueChanged = onValueChanged;
			_SourceValue = sourceValue;
			_TargetValue = targetValue;
			_Length = length;
			_Time = 0;
			_CurveType = curveType;
		}

		public OnVector3Changed _OnValueChanged;
		public float _Length;
		public float _Time;
		public Vector3 _SourceValue;
		public Vector3 _TargetValue;	
		public CurveType _CurveType;
	}

	private List<LerpValueData> _LerpValueList;
	private List<LerpVectorData> _LerpVectorList;

	public void Lerp (Vector3 source, Vector3 target, float length, OnVector3Changed callback, CurveType curveType = CurveType.ValidMovement){
		if (_LerpVectorList == null) {
			_LerpVectorList = new List<LerpVectorData>();
		}

		int data_ = _LerpVectorList.FindIndex(x => x._OnValueChanged == callback);
		if (data_ >= 0) {
			_LerpVectorList.RemoveAt (data_);
		}

		_LerpVectorList.Add(new LerpVectorData(source, target, length, callback, curveType));
	}

	public void Lerp(float source, float target, float length, OnValueChanged callback, CurveType curveType = CurveType.ValidMovement){
		if (_LerpValueList == null) {
			_LerpValueList = new List<LerpValueData>();
		}
		int data_ = _LerpValueList.FindIndex(x => x._OnValueChanged == callback);
		if (data_ >= 0) {
			_LerpValueList.RemoveAt (data_);
		}

		_LerpValueList.Add(new LerpValueData(source, target, length, callback, curveType));
	}
				
	void Update () {
		
		if (_LerpValueList != null) {
			for (int i = 0; i < _LerpValueList.Count; ++i) {
				_LerpValueList [i]._Time = Mathf.Min (1.0f, _LerpValueList [i]._Time + Time.deltaTime / _LerpValueList [i]._Length);

				if (_LerpValueList [i]._Time == 1.0f) {
					_LerpValueList [i]._OnValueChanged (_LerpValueList [i]._TargetValue);
					_LerpValueList.RemoveAt (i--);
				} else {
					float value_ = (_LerpValueList [i]._TargetValue - _LerpValueList [i]._SourceValue) * (_LerpValueList[i]._CurveType == CurveType.ValidMovement ? InterpolationCurve.Evaluate (_LerpValueList [i]._Time) : InvalidMovement.Evaluate(_LerpValueList[i]._Time)) + _LerpValueList [i]._SourceValue; 
					_LerpValueList [i]._OnValueChanged (value_);
				}
			}
		}

		if (_LerpVectorList != null) {
			for (int i = 0; i < _LerpVectorList.Count; ++i) {
				_LerpVectorList [i]._Time = Mathf.Min (1.0f, _LerpVectorList [i]._Time + Time.deltaTime / _LerpVectorList [i]._Length);

				Vector3 value_ = (_LerpVectorList [i]._TargetValue - _LerpVectorList [i]._SourceValue) * (_LerpVectorList[i]._CurveType == CurveType.ValidMovement ? InterpolationCurve.Evaluate (_LerpVectorList [i]._Time) : InvalidMovement.Evaluate(_LerpVectorList[i]._Time)) + _LerpVectorList [i]._SourceValue; 
				_LerpVectorList [i]._OnValueChanged (value_);

				if (_LerpVectorList [i]._Time == 1.0f) {
					_LerpVectorList.RemoveAt (i--);
				}
			}
		}
	}
}
