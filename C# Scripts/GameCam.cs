using UnityEngine;
using System.Collections;

public class GameCam : MonoBehaviour {
	//network GUI
	public string IP= "127.0.0.1";
	public int Port = 25001;

	int myPlayer;
	int numPlayers=1;
	//int playerReady = 0;
	bool gotPlayerNum;
	bool GameStart=false;
	int[] playersReady = new int[2];
	int turn =0;
	int maxTurns = 32;
	GameObject digSpot;





	void OnGUI()
	{
		if (Network.peerType ==  NetworkPeerType.Disconnected)
		{
			if(GUI.Button (new Rect (100,100,100,25) , "Join a Game"))
			{
				Network.Connect(IP,Port);
			}
			if(GUI.Button (new Rect (100,125,100,25) , "Host a Game"))
			{
				Network.InitializeServer(3,Port,true);
				this.myPlayer=1;
				this.gotPlayerNum=true;
				
			}
		}
		else {
			if (Network.peerType ==  NetworkPeerType.Client && !GameStart)
			{
				GUI.Label(new Rect(10,10,100,25),"Client");
				this.myPlayer=numPlayers;
				this.gotPlayerNum=true;
				GUI.Label(new Rect(10,35,100,25),"MyP"+this.myPlayer);
				GUI.Label(new Rect(100,55,150,25),"Choose your vessel");
				if (GUI.Button(new Rect(100,75,100,25),"Boat1"))	
				{
					//networkView.RPC("AskInstantiate",RPCMode.Server,myPlayer);
					//this.GameStart= !this.GameStart;
					networkView.RPC("AskReady",RPCMode.All,this.myPlayer);
					this.playersReady[this.myPlayer-1]= this.myPlayer;
				}
				
				
				if (GUI.Button(new Rect(100,125,100,25),"Logout"))
				{
					Network.Disconnect(250);
				}
				
			}
			if (Network.peerType  ==  NetworkPeerType.Server && !GameStart)
			{
				GUI.Label(new Rect(10,10,100,25),"Server");
				GUI.Label(new Rect(10,35,100,25),"Players: "+ this.numPlayers);
				GUI.Label(new Rect(100,55,150,25),"Choose your vessel");
				if (playersReady[this.numPlayers-1]==this.numPlayers)
					{

					if (GUI.Button(new Rect(100,75,100,25),"Boat1"))//ToDo Wait for player2 until instruction	
					{
						networkView.RPC("AskInstantiate",RPCMode.All);
						//Debug.Log("Asking turn");
						networkView.RPC("AskTurn",RPCMode.All);
						//this.GameStart= !this.GameStart;
						networkView.RPC("AskReady",RPCMode.All,this.myPlayer);
						
					}
					
					if (GUI.Button(new Rect(100,150,100,25),"Logout"))	
					{
						Network.Disconnect(250);
					}
				}
				
			}
			if (Player1!=null || Player2!=null){
				if (Player2!=null){
					Vector2	healthBarPositionP1 = Camera.main.WorldToScreenPoint (Player1.myBoat.transform.position);
					Vector2	healthBarPositionP2 = Camera.main.WorldToScreenPoint (Player2.myBoat.transform.position);
					GUI.Box(new Rect(healthBarPositionP1.x, healthBarPositionP1.y, 60, 20), Player1.currentHealth + "/" + Player1.maxHealth);
					GUI.Box(new Rect(healthBarPositionP2.x, healthBarPositionP2.y, 60, 20), Player2.currentHealth + "/" + Player2.maxHealth);

				}
				if (((myPlayer==1)&&(Player1.displayGUI))||((myPlayer==2)&&(Player2.displayGUI)))
				{
					if (Player2!=null){
					Player1.enemy=Player2.myBoat;
					Player2.enemy=Player1.myBoat;
					}
					GUI.Label(new Rect(10,35,100,25),"PlayerTurn"+this.turn);
					if(GUI.Button (new Rect (100,100,100,25) , "MOVE North")){
						networkView.RPC("MoveMyBoat",RPCMode.All, this.myPlayer,Vector3.forward);
						networkView.RPC("RotateMyBoat",RPCMode.All,this.myPlayer,new Vector3(0,0,0));


					}
					if (GUI.Button(new Rect(0,100,100,25), "West")){
						networkView.RPC("MoveMyBoat",RPCMode.All,this.myPlayer,Vector3.left);
						networkView.RPC("RotateMyBoat",RPCMode.All,this.myPlayer,new Vector3(0,-90,0));
					}
					if (GUI.Button(new Rect(200,100,100,25), "East")){
						networkView.RPC("MoveMyBoat",RPCMode.All,this.myPlayer,Vector3.right);
						networkView.RPC("RotateMyBoat",RPCMode.All,this.myPlayer,new Vector3(0,90,0));
					}
					if(GUI.Button (new Rect (100,125,100,25) , "MOVE South")){
						networkView.RPC("MoveMyBoat",RPCMode.All,this.myPlayer,Vector3.back);
						networkView.RPC("RotateMyBoat",RPCMode.All,this.myPlayer,new Vector3(0,180,0));
					
					}
					if(GUI.Button (new Rect (100,250,100,25) , "EndTurn")){
						networkView.RPC( "askEndTurn",RPCMode.All ,this.myPlayer);
						
					}
					if (Player1.canDig==true && Player1.gotKey && myPlayer==1)
					{
						if (GUI.Button(new Rect(500,100,100,25), "DIG!")){
							this.turn=maxTurns;
							networkView.RPC( "askEndTurn",RPCMode.All ,this.myPlayer);
						}
					}
					if (Player2!=null)
						if (Player2.canDig==true && Player2.gotKey && myPlayer==2)
						{
							if (GUI.Button(new Rect(500,100,100,25), "DIG!")){
								this.turn=maxTurns;
								networkView.RPC( "askEndTurn",RPCMode.All ,this.myPlayer);
						}

						}

					if (Player2!=null){
						if ((Player2.cityOnRange && myPlayer==2))//||((Player1.cityOnRange&&myPlayer==1)))
						{

							if (!Player2.gotUpgrade){
							if (GUI.Button(new Rect(300,225,100,50), "ApplyCard 2 \n Def+2"))
									networkView.RPC("askCard",RPCMode.All,this.myPlayer,2);
							if (GUI.Button(new Rect(400,225,100,50), "ApplyCard 1\n Atk+2"))
									networkView.RPC("askCard",RPCMode.All,this.myPlayer,1);
							}


							if (!Player2.gotKey && myPlayer==2)
								if (GUI.Button(new Rect(350,275,100,50), "FindTolken")){
									networkView.RPC("askTolken",RPCMode.All,this.myPlayer,Player2.city.hasKey);	
							}
							
							if (GUI.Button(new Rect(350,325,100,50), "Repair"))
								networkView.RPC("askRepair",RPCMode.All,this.myPlayer);
						}} 	if (((Player1.cityOnRange&&myPlayer==1)))//For Single player
					{
						//if (GUI.Button(new Rect(400,100,100,25), "BuyCards"))
						//	networkView.RPC("MoveMyBoat",RPCMode.All,this.myPlayer,Vector3.zero);
						if (!Player1.gotUpgrade){
						if (GUI.Button(new Rect(300,225,100,50), "ApplyCard 2 \n Def+2"))
							networkView.RPC("askCard",RPCMode.All,this.myPlayer,2);
						if (GUI.Button(new Rect(400,225,100,50), "ApplyCard 1\n Atk+2"))
							networkView.RPC("askCard",RPCMode.All,this.myPlayer,1);
						}

						if (!Player1.gotKey)
							if (GUI.Button(new Rect(350,275,100,50), "FindTolken"))
								networkView.RPC("askTolken",RPCMode.All,this.myPlayer,Player1.city.hasKey);

						if (GUI.Button(new Rect(350,325,100,50), "Repair"))
							networkView.RPC("askRepair",RPCMode.All,this.myPlayer);
					}

					if (Player1.enemyOnRange){
						if (GUI.Button(new Rect(300,100,100,25), "Attack!")){
							networkView.RPC( "askAttack",RPCMode.All ,this.myPlayer);

						}
						if (Player2.currentHealth==0 && myPlayer==1)
						{
							if (GUI.Button(new Rect(300,125,100,25), "Pillage"))
								networkView.RPC( "askPillage",RPCMode.All ,this.myPlayer);
						}
						if (Player1.currentHealth==0 && myPlayer==2)
						{
							if (GUI.Button(new Rect(300,125,100,25), "Pillage"))
								networkView.RPC( "askPillage",RPCMode.All ,this.myPlayer);
						}

					}

				}


				
			}
			
		}
	}
	[RPC]
	void AskReady(int _myPlayer)
	{
		if (Network.isServer) {
			networkView.RPC("Ready",RPCMode.All,_myPlayer);
			
		}
	}
	[RPC]
	void Ready(int _myPlayer)
	{
		if (Network.isServer) {
			this.playersReady [_myPlayer - 1] = _myPlayer;
		}
		
	}
	[RPC]
	void UpdatePlayers(int _number)//Called at each connection
	{
		this.numPlayers = _number;
	}
	[RPC]
	void AskInstantiate()
	{
		if (Network.isServer)
		{
			networkView.RPC("InstantiateBoat",RPCMode.All);
		}
	}
	[RPC]
	void InstantiateBoat()
	{
		{
			this.GameStart=true;
			if (playersReady[0] ==1){
				//Player1.myBoat = (GameObject)Instantiate(Pirate1 , startlocation1.position ,transform.rotation);
				tempPirate = (GameObject)Instantiate(Pirate1 , startlocation1.position ,transform.rotation);
				Player1 = tempPirate.GetComponent<vesselPlayer>();
				Player1.myBoat = tempPirate;
				//if (myPlayer==1) this.target= Player1.myBoat.transform;
			}
			
			
			if (playersReady[1] ==2){
				//Player2.myBoat = (GameObject)Instantiate(Pirate2 , startlocation2.position ,transform.rotation);
				tempPirate = (GameObject)Instantiate(Pirate2 , startlocation2.position ,transform.rotation);
				Player2 = tempPirate.GetComponent<vesselPlayer>();
				Player2.myBoat = tempPirate;
			}
		}
	}
	[RPC]
	void AskTurn ()
	{
		if (Network.isServer)
			networkView.RPC ("Turn", RPCMode.All);
	}
	[RPC]
	void Turn ()
	{
		if ((this.turn == 0)) {
			this.turn = 1;
			Player1.displayGUI=true;
			this.target = Player1.myBoat.transform;
		} else{if (this.numPlayers>1){
			this.turn=0;
			Player1.displayGUI=false;
			Player2.displayGUI=true;
			this.target = Player2.myBoat.transform;
			}
		}
	}

	[RPC]
	void AskMovement(int _MyPLayerNumber , Vector3 _position)
	{
				if (Network.isServer) {						
						networkView.RPC ("MoveMyBoat", RPCMode.All, _MyPLayerNumber, _position);
						
				}
		}
	[RPC]
	void MoveMyBoat(int _MyPLayerNumber , Vector3 _position)
	{
		if (_MyPLayerNumber == 1) {
			if ((((Player1.gameObject.transform.localPosition+_position).x>-23.43)&&
			     (Player1.gameObject.transform.localPosition+_position).x <27.95)&&
			    (Player1.gameObject.transform.localPosition+_position).z>-31&&
			    ((Player1.gameObject.transform.localPosition+_position).z<20.5487))
			this.Player1.gameObject.transform.localPosition +=_position;
			this.Player1.turnMoves--;
			if (Player1.turnMoves<=0 || Player1.currentHealth==0){
				networkView.RPC( "endTurn",RPCMode.All ,_MyPLayerNumber);

			}
		}
		if (this.Player2!=null)
		if (_MyPLayerNumber == 2 ) {
			if ((((Player2.gameObject.transform.localPosition+_position).x>-23.43)&&
			     (Player2.gameObject.transform.localPosition+_position).x <27.95)&&
			    (Player2.gameObject.transform.localPosition+_position).z>-31&&
			    ((Player2.gameObject.transform.localPosition+_position).z<20.5487))
			this.Player2.myBoat.gameObject.transform.position += _position;
			this.Player2.turnMoves--;

			if (Player2.turnMoves<=0 || Player2.currentHealth==0){
				networkView.RPC( "endTurn",RPCMode.All ,_MyPLayerNumber);
			}
		}
	}
	[RPC]
	void RotateMyBoat(int _MyPLayerNumber , Vector3 _position)
	{
		if (_MyPLayerNumber == 1) {
								Player1.transform.localEulerAngles = (_position);
				}
		if (_MyPLayerNumber == 2) {
			Player2.transform.localEulerAngles = ( _position);
		}
	}
	[RPC]
	void endTurn(int _MyPLayerNumber)
	{
		if (_MyPLayerNumber == 1) {
			//Debug.Log("endTurn");
			Player1.displayGUI=false;
			Player1.turnMoves = Player1.maxTurnMoves;
			if (Player2!=null){
				this.target = Player2.myBoat.transform;
				this.Player2.displayGUI=true;
			}else {
				Player1.displayGUI=true;
			}
		}
		if (_MyPLayerNumber == 2 ) {
			Player2.displayGUI=false;
			Player2.turnMoves= Player2.maxTurnMoves;
			this.target = Player1.myBoat.transform;
			this.Player1.displayGUI=true;
		}
		if (this.turn >= this.maxTurns) {
			Network.Disconnect();
			Debug.Log("EndGame");
			Application.LoadLevel ("Apresentacao");
				}
		this.turn++;
	}
	[RPC]
	void askEndTurn (int _MyPlayerNumber)
	{
		if (Network.isServer)
		networkView.RPC( "endTurn",RPCMode.All ,_MyPlayerNumber);
	}
	[RPC]
	void askAttack(int _MyPlayerNumber)
	{
		if (Network.isServer)
			networkView.RPC( "Attack",RPCMode.All ,_MyPlayerNumber);
	}
	[RPC]
	void Attack(int _MyPlayerNumber)
	{
		if (_MyPlayerNumber == 1 && (Player2.currentHealth!=0))
				if (Player2.firePower >= Player1.defense) {
						Player2.currentHealth--;
				}
				else {
						Player2.currentHealth -= (Player1.firePower - Player2.defense);
				}
		if (_MyPlayerNumber == 2)
		if (Player1.firePower >= Player2.defense && (Player1.currentHealth!=0)) {
						Player1.currentHealth--;
				}
				else {
						Player1.currentHealth -= (Player2.firePower - Player1.defense);
				}
		networkView.RPC( "endTurn",RPCMode.All ,_MyPlayerNumber);

	}
	[RPC]
	void askRepair (int _MyPlayerNumber)
	{
		if (Network.isServer) {
			networkView.RPC("Repair",RPCMode.All, _MyPlayerNumber);
				}
	}
	[RPC]
	void Repair (int _MyPlayerNumber)
	{
		if (_MyPlayerNumber == 1){
			Player1.currentHealth=Player1.maxHealth;
		}
	
		if (_MyPlayerNumber == 2){
			Player2.currentHealth=Player2.maxHealth;
		}
		//networkView.RPC( "endTurn",RPCMode.All ,_MyPlayerNumber);
	
	}
	[RPC]
	void askTolken (int _MyPlayerNumber ,bool _cityKey)
	{
			if (Network.isServer) {
			networkView.RPC("GetTolken",RPCMode.All, _MyPlayerNumber, _cityKey);
			}

	}
	[RPC]
	void GetTolken(int _MyPlayerNumber , bool _cityKey){
		if (_MyPlayerNumber == 1 && _cityKey){
			Player1.gotKey=true;
			Player1.city.askCityKeyLoss();
		} else
		if (_MyPlayerNumber == 2 && _cityKey){
			Player2.gotKey=true;
			Player2.city.askCityKeyLoss();
		}
		networkView.RPC( "endTurn",RPCMode.All ,_MyPlayerNumber);
		}
	[RPC]
	void askPillage (int _MyPlayerNumber)
	{
		if (Network.isServer) {
			networkView.RPC( "Pillage",RPCMode.All ,_MyPlayerNumber);
				}
	}
	[RPC]
	void Pillage (int _MyPlayerNumber)
	{
		if (_MyPlayerNumber == 1) {
			if (Player2.gotKey)	{
				Player2.gotKey = false;
				Player1.gotKey = true;
			}
		}
		if (_MyPlayerNumber == 2) {
			if (Player1.gotKey)	{
			Player1.gotKey = false;
			Player2.gotKey = true;
			}
		}

	}
	[RPC]
	void askCard(int _MyPlayerNumber , int cardNumber)
	{
		networkView.RPC( "CardApply",RPCMode.All ,_MyPlayerNumber ,cardNumber);
	}

	[RPC]
	void CardApply(int _MyPlayerNumber , int _CardNumber){
		if (_MyPlayerNumber == 1) {
			if (_CardNumber ==1)	{
				Player1.firePower=5;
				
			}else 
			{
				Player1.defense=4;
			}
			Player1.gotUpgrade=true;
		}
		if (_MyPlayerNumber == 2) {
			if (_CardNumber==1){
				Player2.firePower=5;
			}else
			{
				Player2.defense=4;
			}
			Player2.gotUpgrade=true;
		}
		}

	vesselPlayer Player1 ; 
	vesselPlayer Player2 ;
	
	GameObject tempPirate;
	
	public GameObject Pirate1;
	public GameObject Pirate2;
	
	public Transform startlocation1;
	public Transform startlocation2;


	// Update is called once per frame
	void Update () {
				if (Network.isServer) {
						if ((Network.connections.Length + 1) != this.numPlayers) {
								Debug.Log ("Updating Players!");
								networkView.RPC ("UpdatePlayers", RPCMode.All, (Network.connections.Length + 1));
						}
				}
		if (Player1 != null) {	
						if (Player1.gotKey == true && myPlayer==1)
								digSpot.gameObject.SetActive (true);
				}
		if (Player2!=null){
			if (Player2.gotKey == true && myPlayer ==2)
				digSpot.gameObject.SetActive(true);

			}

//				if (Network.isServer && ((this.playersReady [0] != 0) && this.playersReady [1] != 0)) {
//						networkView.RPC ("AskInstantiate", RPCMode.All, this.playersReady [0]);
//						networkView.RPC ("AskInstantiate", RPCMode.All, this.playersReady [1]);
//						networkView.RPC ("AskTurn", RPCMode.All);
//						//this.GameStart= !this.GameStart;
//				}
		}









	//Orbit Script
	public Transform target;
	static float distance = 5.0f;
	private float xSpeed = 120.0f;
	private float ySpeed = 120.0f;
	
	private float yMinLimit = -20f;
	private float yMaxLimit = 80f;
	
	private float distanceMin = 10f;
	private float distanceMax = 15f;


	
	private float x = 0.0f;
	private float y = 0.0f;

	private float resetTime=0.01f;
	private float resetTimer =0.0f;

	private bool reseting = true;

	
	// Use this for initialization
	void Start () {
		//Player1.gameObject.AddComponent<vesselPlayer> ();
		//Player2.gameObject.AddComponent<vesselPlayer> ();
		this.playersReady [0] = 1;
		this.playersReady [1] = 0;
		digSpot = GameObject.FindGameObjectWithTag ("digSpot");
		digSpot.gameObject.SetActive(false);

		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	void LateUpdate () {
			if (target){
			if (reseting && Input.GetMouseButton (0)) {
				reseting = false;
				resetTimer=0.0f;
				//Debug.Log(reseting);
			}else { //Reset
				resetTimer += Time.deltaTime;
				if(resetTimer >= resetTime) reseting = true;
				//Debug.Log(resetTimer);
			}

			if (!reseting) {
				x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);
				
				Quaternion rotation = Quaternion.Euler(y, x, 0);
				
				distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
				
				RaycastHit hit;
				if (Physics.Linecast (target.position, transform.position, out hit)) {
					distance -=  hit.distance;
				}
				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + target.position;
				
				transform.rotation = rotation;
				transform.position = position;
				
			}
			
	}
}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
