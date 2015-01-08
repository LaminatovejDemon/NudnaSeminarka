using UnityEngine;
using System.Collections;

public class Create_Dungeon : MonoBehaviour {

	public Transform bordelContainer;
	public Transform prefabDungBox;
	Transform prefabDungBoxInstance;
	public pole_01 targetpole_01;

	// Use this for initialization
	void Start () 
	{
		/*
		for(int i = 0; i < 10; i++)
		{
			for(int ii= 0; ii <20; ii++)
			{
				//Debug.Log ("vypis Icek" + i + "," + ii);
				if (targetpole_01.TestField[i,ii]=="1")
				{
					prefabDungBoxInstance = Instantiate(prefabDungBox) as Transform;
					prefabDungBoxInstance.position = new Vector3(i,0.5f,ii);
					prefabDungBoxInstance.name = i + "_" + ii + "_instance";
					prefabDungBoxInstance.parent = bordelContainer;
				}
			}
		}
		*/

	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.D)) 
		{
			Debug.Log ("Dungeon se generuje");



			for(int i = 0; i < 10; i++)
			{
				for(int ii= 0; ii <10; ii++)
				{
								//Debug.Log ("vypis Icek" + i + "," + ii);
					if (targetpole_01.TestField[i,ii]=="1")
					{
					prefabDungBoxInstance = Instantiate(prefabDungBox) as Transform;
					prefabDungBoxInstance.position = new Vector3(i,0.5f,ii);
					prefabDungBoxInstance.name = i + "_" + ii + "_instance";
					prefabDungBoxInstance.parent = bordelContainer;
					}
				}
			}
		}
		*/
	
	}
}
