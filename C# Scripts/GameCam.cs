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
				Network.InitializeServer(10,Port,true);
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
				if (((myPlayer==1)&&(Player1.displayGUI))||((myPlayer==2)&&(Player1.displayGUI)))
				{
					if (Player2!=null){
					Player1.enemy=Player2.myBoat;
					Player2.enemy=Player1.myBoat;
					}
					GUI.Label(new Rect(10,35,100,25),"PlayerTurn"+this.turn);
					if(GUI.Button (new Rect (100,100,100,25) , "MOVE Foward")){
						MoveMyBoat(this.myPlayer,Vector3.forward);
						
					}
					if (GUI.Button(new Rect(0,100,100,25), "Left")){
						MoveMyBoat(this.myPlayer,Vector3.left);
					}
					if (GUI.Button(new Rect(200,100,100,25), "Right")){
						MoveMyBoat(this.myPlayer,Vector3.right);
					}
					if(GUI.Button (new Rect (100,125,100,25) , "MOVE Backwards")){
						MoveMyBoat(this.myPlayer,Vector3.back);
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
		Debug.Log ("Turno: "+this.turn);
		if ((this.turn == 0)) {
			this.turn = 1;
			Player1.displayGUI=true;
			this.target = Player1.myBoat.transform;
		} else{
			this.turn=0;
			Player1.displayGUI=false;
			Player2.displayGUI=true;
			this.target = Player2.myBoat.transform;
			Debug.Log("Turno PLayer 2");
		}
	}

	void MoveMyBoat(int _MyPLayerNumber , Vector3 _position)
	{
		if (_MyPLayerNumber == 1) {
			this.Player1.myBoat.gameObject.transform.position += _position;
			this.Player1.turnMoves--;
			if (Player1.turnMoves<=0)
				endTurn(_MyPLayerNumber);
		}
		if (_MyPLayerNumber == 2) {
			this.Player2.myBoat.gameObject.transform.position += _position;
			this.Player2.turnMoves--;
			if (Player2.turnMoves<=0)
				endTurn(_MyPLayerNumber);
		}
	}
	void endTurn(int _MyPLayerNumber)
	{
		if (_MyPLayerNumber == 1) {
			Player1.displayGUI=false;
			Player1.turnMoves = 5;
		}
		if (_MyPLayerNumber == 2) {
			Player2.displayGUI=false;
			Player2.turnMoves= 5;
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
