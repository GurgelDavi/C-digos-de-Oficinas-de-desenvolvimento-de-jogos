using UnityEngine;
using System.Collections;

public class PiratePlayer : MonoBehaviour {
	Vector3 screenHealthBarDisplay = new Vector3();
	public Camera camera;
	public GameObject myBoat ;
	public GameObject enemy =null ;
	bool enemyOnRange = false;
	public bool cityOnRange;
	public bool displayGUI = false;
	public bool canDig=false;
	float range = 5 ;
	int firePower = 1;
	int defense = 1 ;
	//int movesPerTurn = 1;
	int Health = 5;
	int resources = 10;
	public int turnMoves = 5;

	void OnGUI()
	{
		if (displayGUI)
		{
			if(GUI.Button (new Rect (100,100,100,25) , "MOVE Foward")){
				this.myBoat.transform.position += Vector3.forward;
				this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
			}
			if (GUI.Button(new Rect(0,100,100,25), "Left")){
				this.myBoat.transform.position += Vector3.left;
				this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
			}
			if (GUI.Button(new Rect(200,100,100,25), "Right")){
				this.myBoat.transform.position += Vector3.right;
				this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
			}
			if(GUI.Button (new Rect (100,125,100,25) , "MOVE Backwards")){
				this.myBoat.transform.position -= Vector3.forward;
				this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
			}
			if (enemyOnRange){
				if (GUI.Button(new Rect(300,100,100,25), "Attack!"))
					;
			}
			if (cityOnRange)
			{
				if (GUI.Button(new Rect(400,100,100,25), "BuyCards"))
					this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
				if (GUI.Button(new Rect(400,125,100,25), "Sell"))
					this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
				if (GUI.Button(new Rect(400,150,100,25), "ApplyCard"))
					this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();
				if (GUI.Button(new Rect(400,175,100,25), "FindTolken"))
					this.turnMoves--;
				if (turnMoves <= 0)
					EndTurn();

			}
			if (canDig)
			{
				if (GUI.Button(new Rect(500,100,100,25), "DIG!"))
					;
			}

		}
	}
	void OnTriggerEnter (Collider col)
	{

		if (col.gameObject.tag == "city" ) 
		{
			Debug.Log("City on range");
			cityOnRange=true;
		}
	}

	
	// Use this for initialization	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (enemy != null) {
						float distance = Vector3.Distance (transform.position, this.enemy.transform.position);
						Debug.Log (distance);
						if (distance <= this.range) {
								this.enemyOnRange = true;
						} else
								this.enemyOnRange = false;
				}
	}
	void EndTurn ()
	{
		//toDo End Turn Method
		this.displayGUI = !displayGUI;
	}
	[RPC]
	void AskServerForMovement(int _direction)
	{
		if (Network.isServer)
		{
			networkView.RPC("MoveBoat",RPCMode.All,_direction);
		}
	}


	[RPC]
	void MoveBoat(int _action)
	{
		if (_action==1) 
		{

		}
		if (_action==2) 
		{
			
		}
		if (_action==3) 
		{
			
		}
		if (_action==4) 
		{
			
		}
	}
	
}


