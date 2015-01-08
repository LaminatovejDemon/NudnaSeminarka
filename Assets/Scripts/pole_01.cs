using UnityEngine;
using System.Collections;

public class pole_01 : MonoBehaviour {

	int [,] MainField = new int[30,30];
	public int [,] TestField = new int[30,30];
	//string [] dataLines = new string[10];
	public TextAsset bludisteFile;

	//int [,] MyDoubleSpaceField = new int[10, 20];

	// Use this for initialization
	void Start () 
	{
		// HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST
		// HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST
		// HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST
		// HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST
		// HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST HOMOTEST

		string[] dataLines = bludisteFile.text.Split('\n');


		for(int i = 0; i < 30; i++)
		{
			//Debug.Log("Creating enemy number: " + dataLines[i]);
			for(int ii= 0; ii <30; ii++)
			{
				TestField[i,ii] = int.Parse(dataLines[i].Substring(ii*2,1));
				//MainField[i,ii]=dataLines

			}
		}

		/*	for(int y = 0; y < 30; y++)
			{
				for(int yy= 0; yy <30; yy++)
				{
					Debug.Log(TestField[y,yy]);

				}
			}
			*/
			

		/*
		string[] dataLines = bludisteFile.text.Split('\n');
		string[][] dataPairs = new string[dataLines.Length][];

		int lineNum = 0;
		foreach (string line in dataLines)
		{
			dataPairs[lineNum++] = line.Split(';');
		}
		
		Debug.Log(dataPairs[2][1]);  // prints "fish"
		Debug.Log(dataPairs[3][0]);  // prints "goat"
		*/

	}

	float	_XPole;
	float   _Ypole;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.anyKeyDown)
		{
			_XPole = Camera.main.GetComponent<MainCamera_Movement>()._TargetPosition.x;
			_Ypole = Camera.main.GetComponent<MainCamera_Movement>()._TargetPosition.z;
			//Debug.Log("hodnota pole na " + _XPole + "," + _Ypole + "je:" + TestField[(int)_XPole,(int)_Ypole]);

			//TestField[i,ii]
		}


	}
}
