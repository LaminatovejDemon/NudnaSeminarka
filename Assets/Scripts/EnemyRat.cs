using UnityEngine;
using System.Collections;

public class EnemyRat : MonoBehaviour {
	
	float RatTimeDif;
	int RatX;
	int RatY;
	int RatZ;
	float RatAngle;
	bool RatMove;
	bool RatRotate;
	Vector3 the_return;
	Vector3 krysaeulers = Vector3.zero;

	bool CanAttack = false;
	float RatAttackInterval = 2f;
	float RatAttackTime;
	bool RatAttackGO = false;
	string RatStatus;
	int RatRandomMoveDirection;

	PlayerController playerController;
	float damage = 15f;
	public float health = 75f;

	//pathfinding variables
	float PathfindingBreak = 2f;
	float PathfindingTime;
	bool DoPathfinding;
	int [,] BludisteField = new int[30,30];
	int [,] PathfindingField = new int[30, 30];
	public int PathfindingDepth;
	private static int PlayerX;
	private static int PlayerZ;
	int RatPathDistance;


	//pristup na dalsi skripty
	public MainCamera_Movement PathfindingScript;
	public pole_01 PoleScript;

	void ClearField()
	{
		for(int i = 0; i < 30; i++)
		{
			for(int ii= 0; ii <30; ii++)
			{
				PathfindingField[i,ii] = 100;
			}
		}

	}

	void Pathfinding()
	{
		PathfindingTime = Time.time;
//		Debug.Log ("DOING PATHFINDING");
		DoPathfinding = false;
		ClearField ();
		BludisteField = PoleScript.TestField;
		RatPathDistance = 100;

		PlayerX = Mathf.RoundToInt(PathfindingScript._PlayerPosition.x);
		PlayerZ = Mathf.RoundToInt(PathfindingScript._PlayerPosition.z);
		//Debug.Log ("HRACJENA: " + PlayerX + "," + PlayerZ);
		PathfindingField[PlayerX,PlayerZ] =0;

		for (int i = 0; i < PathfindingDepth; i++)
		{
			//Debug.Log("hodnota i je: " +i);
			for(int ii = 1; ii < 29; ii++)
			{
				for(int iii= 1; iii <29; iii++)
				{
					if (PathfindingField[ii,iii]== i)
					{
						//Debug.Log(" shoda existuje shoda existuje shoda existuje shoda existuje shoda existuje shoda existuje shoda existuje");
						if ((BludisteField[ii+1,iii]==0) && (PathfindingField[ii+1,iii]==100)) { PathfindingField[ii+1,iii]=i+1; }
						if ((BludisteField[ii-1,iii]==0) && (PathfindingField[ii-1,iii]==100)) { PathfindingField[ii-1,iii]=i+1; }
						if ((BludisteField[ii,iii+1]==0) && (PathfindingField[ii,iii+1]==100)) { PathfindingField[ii,iii+1]=i+1; }
						if ((BludisteField[ii,iii-1]==0) && (PathfindingField[ii,iii-1]==100)) { PathfindingField[ii,iii-1]=i+1; }
					}


				}
			}
		}

		RatPathDistance = PathfindingField [RatX, RatZ];

		//Debug.Log ("PFING DISTANCE JE " + RatPathDistance);

		}

	void KrysaMovement() 
	{
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (RatX, RatY, RatZ),0.8f * Time.deltaTime);
		//animation.Play ("walk");
	}


	void KrysaRotation()
		{

	
		krysaeulers.y += (RatAngle - krysaeulers.y) * Time.deltaTime * 3;
		


			//animation.Play ("walk");
			if (Mathf.Abs (krysaeulers.y - RatAngle) < 3.5f) 
					{
					krysaeulers.y = RatAngle;
					RatRotate = false;

					if (RatAngle == -180) 
						{
						RatAngle = 180;
						krysaeulers.y = 180;
						}
						
					if (RatAngle == 360) 
						{
						RatAngle = 0;
						krysaeulers.y = 0;
						}

					}
		
				transform.eulerAngles = krysaeulers;
		}

	
	// Use this for initialization
	void Start () 
	{
		DoPathfinding = true;
		RatMove = false;
		RatRotate = false;
		RatAngle = 0;
		RatX = 6;
		RatY = 0;
		RatZ = 7;
		transform.position = new Vector3(RatX, RatY, RatZ);
		transform.eulerAngles = new Vector3(0, RatAngle, 0);
		GetComponent<Animation>().Play ("idle");
		BludisteField = PoleScript.TestField;
		RatStatus = "moving";
		playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
		
	}
	
	// Update is called once per frame
	
	
	void Update () 
	{

		if (health > 0) {
			// PATHFINDING SE DELA VZDY V INTERVALU PATHFINDINGBREAK
			if (!DoPathfinding)
			{
				if ((PathfindingTime+PathfindingBreak)<Time.time)
				{
					DoPathfinding = true;
				}
			}


			if (DoPathfinding) 
			{
				Pathfinding ();

				if ((RatPathDistance < 100) && (RatPathDistance>1))
					{
					
						if ((PathfindingField [RatX, RatZ-1]) == (RatPathDistance - 1)) 
						{

							if (RatAngle == -90) RatAngle = -180;
							if (RatAngle !=-180) RatAngle = 180;
							RatZ -= 1;
						}
					
						if ((PathfindingField [RatX, RatZ+1]) == (RatPathDistance - 1)) 
						{
							if (RatAngle == 270) RatAngle = 360;
							if (RatAngle != 360) RatAngle = 0;
							RatZ += 1;
						}

						if ((PathfindingField [RatX-1, RatZ]) == (RatPathDistance - 1)) 
						{
							if (RatAngle == 180) RatAngle = 270;
							if (RatAngle != 270) RatAngle = -90;
							RatX -= 1;
						}

						if ((PathfindingField [RatX+1, RatZ]) == (RatPathDistance - 1)) 
						{
							RatAngle = 90;
							RatX += 1;
						}

						//Debug.Log("kryse prideluji uhel" + RatAngle);

					}

			
				
			}

			// KDYZ MA KRYSA JINOU POZICI, TAK ANIMACE NA KOREKTNI POZICI
			if (transform.position !=new Vector3(RatX,0,RatZ)) 
			{
				KrysaMovement();
				GetComponent<Animation>().Play ("walk");
			}

			// KDYZ KRYSA DOJDE NAPUL CESTY V PATHFINDINGU TAK ZNOVA PATHFINDING - KRYSA SE PAK POHYBUJE PLYNULE A NE PO SKOCICH
			if ((transform.position == new Vector3(RatX,0,RatZ)) && (RatPathDistance>1) && (RatPathDistance<100)) 
			{
				RatStatus = "sneaking";
				DoPathfinding = true;
			}

			if (RatPathDistance == 100) 
			{
				RatStatus = "moving";
			}

			// KDYZ MA KRYSA JINOU ROTACI, TAK ANIMACE NA KOREKTNI ROTACI
			if (krysaeulers.y != RatAngle) 
			{
				KrysaRotation ();
				GetComponent<Animation>().Play ("walk");
			}


			//OTACIME KRYSU NA HRACE KDYZ JE POBLIZ
			if (RatPathDistance == 1) 
			{
				if ((RatX == PlayerX) && (RatZ == PlayerZ+1))
				{
					if (RatAngle == -180) CanAttack=true;
					if (RatAngle == 180) CanAttack=true;
					if (RatAngle == -90) RatAngle = -180;
					if (RatAngle !=-180) RatAngle = 180;
				}

				if ((RatX == PlayerX) && (RatZ == PlayerZ-1))
				{
					if (RatAngle == 0) CanAttack=true;
					if (RatAngle == 360) CanAttack=true;
					if (RatAngle == 270) RatAngle = 360;
					if (RatAngle != 360) RatAngle = 0;
				}

				if ((RatX == PlayerX+1) && (RatZ == PlayerZ))
				{
					if (RatAngle == 270) CanAttack=true;
					if (RatAngle == -90) CanAttack=true;
					if (RatAngle == 180) RatAngle = 270;
					if (RatAngle != 270) RatAngle = -90;
				}

				if ((RatX == PlayerX-1) && (RatZ == PlayerZ))
				{
					if (RatAngle == 90) CanAttack=true;
					RatAngle = 90;
				}
			}

			//KDYZ JE KRYSA NA MISTE A NEUTOCI, TAK IDLE ANIMACE - zatim se nemuze stat
			if ((transform.position == new Vector3(RatX,0,RatZ)) && (krysaeulers.y == RatAngle) && (!RatAttackGO)) 
			{
				GetComponent<Animation>().Play ("idle");
			}	

			// KDYZ MUZE ZAUTOCIT TAK ZAUTOCI
			if ((CanAttack) && (!RatAttackGO))
			{
				RatStatus = "attacking";
				RatAttackTime = Time.time;
				RatAttackGO = true;
			}

			// KDYZ ZAUTOCIL/A/O/I TAK PROBIHA UTOK
			if ((RatAttackGO) && ((Time.time - RatAttackTime)<(RatAttackInterval/2)))
			{
				GetComponent<Animation>().Play("4LegsBiteAttack");
			}

			if ((RatAttackGO) && ((Time.time - RatAttackTime)>(RatAttackInterval/2)))
			{
				DoDamage(); 
				RatAttackGO = false;
				CanAttack = false;
			}

			// KRYSA JE MOVING A JE NA MISTE (ratxz = position) SE ZACNE POHYBOVAT NAHODNE NEKAM JINAM
			if ((RatStatus == "moving") && (transform.position == new Vector3(RatX,0,RatZ)))
			{

				RatRandomMoveDirection = Random.Range(0,4);
//				Debug.Log (RatRandomMoveDirection);

				if ((RatRandomMoveDirection==0) && (BludisteField[RatX-1,RatZ]==0)) 
				{ 
					RatX -= 1;
					RatAngle = -90;
					if (krysaeulers.y == 180) { RatAngle =270;}
				}

				if ((RatRandomMoveDirection==1) && (BludisteField[RatX+1,RatZ]==0)) 
				{ 
					RatX += 1;
					RatAngle = 90;
				}

				if ((RatRandomMoveDirection==2) && (BludisteField[RatX,RatZ+1]==0)) 
				{ 
					RatZ += 1;
					RatAngle = 0;
				}

				if ((RatRandomMoveDirection==3) && (BludisteField[RatX,RatZ-1]==0)) 
				{ 
					RatZ -= 1;
					RatAngle = 180;
					if (krysaeulers.y == -90) { RatAngle =-180;}
				}

//				Debug.Log("EULERS JE ROVNO" + krysaeulers.y);
//				Debug.Log("KANGLE JE ROVNO" + RatAngle);


			}

			//KrysaMovement ();

			//KrysaRotation ();

		}

				/*
				
		if (Input.GetKeyDown("up"))
		{
			RatMove = true;
			float rad = RatAngle * Mathf.Deg2Rad;
			RatX += Mathf.RoundToInt(Mathf.Sin(rad));
			RatZ += Mathf.RoundToInt(Mathf.Cos(rad));
			Debug.Log ("uhel krysy je" + RatAngle);
			Debug.Log ("pozice krysy je" + RatX + "," + RatZ);
		}
		
		
		if (RatMove) 
		{
			KrysaMovement();
			
			if (transform.position == new Vector3 (RatX, RatY, RatZ)) 
			{
				RatMove = false;
				animation.Play("idle");
			}
			
		}
		
		

		
		
		if (Input.GetKeyDown("right"))
		{
			RatAngle = RatAngle + 90;
			RatRotate = true;
			Debug.Log ("uhel krysy je" + RatAngle);
			Debug.Log ("pozice krysy je" + RatX + "," + RatZ);
			//transform.eulerAngles = new Vector3(0, RatAngle, 0);
		}
		
		if (Input.GetKeyDown("left"))
		{
			RatAngle = RatAngle - 90;
			RatRotate = true;
			Debug.Log ("uhel krysy je" + RatAngle);
			Debug.Log ("pozice krysy je" + RatX + "," + RatZ);
			//transform.eulerAngles = new Vector3(0, RatAngle, 0);
		}
		
		if (krysaeulers.y != RatAngle) 
			{
				KrysaRotation ();
			}

						
		}
		*/

	} //tu konci update

	void DoDamage () {
		if (CanAttack && RatPathDistance == 1) {
			playerController.GetHit (damage);
			Debug.Log ("Player Health: " + playerController.GetHealth());
		}
	}

	void OnMouseDown () {
		if (CanAttack && health > 0 ) {
			health -= playerController.GetPlayerDamage();
			Debug.Log ("Enemy Health: " + health);
			if (health <= 0) {
				GetComponent<Animation>().Play("4LegsDeath");
			}
		}
	}

}
