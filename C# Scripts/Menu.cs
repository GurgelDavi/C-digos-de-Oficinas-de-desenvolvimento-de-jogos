using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public string IP= "127.0.0.1";
	public int Port = 25001;
	int myPLayer=0;
	public GameObject BoatModel;
	public GameObject player1;
	public GameObject player2;
	public Transform location1;
	public Transform location2;

	void OnGUI()
	{
		if (Network.peerType ==  NetworkPeerType.Disconnected)
		{
			if(GUI.Button (new Rect (100,100,100,25) , "Start Client"))
			{
				Network.Connect(IP,Port);
				this.myPLayer=2;
			}
			if(GUI.Button (new Rect (100,125,100,25) , "Start Server"))
			{
				Network.InitializeServer(10,Port,true);
				this.myPLayer=1;
			}
		}
		else {
			if (Network.peerType ==  NetworkPeerType.Client)
			{
				GUI.Label(new Rect(100,100,100,25),"Client");

				if (GUI.Button(new Rect(100,125,100,25),"Logout"))
				{
					Network.Disconnect(250);
				}
				if (GUI.Button(new Rect(100,150,100,25),"ChangeColor"))
				{
					networkView.RPC("AskColor",RPCMode.Server);
				}
				if (GUI.Button(new Rect(100,175,100,25),"Instantiate"))
				{
					networkView.RPC("AskInstantiate",RPCMode.Server);
				}
			}
			if (Network.peerType ==  NetworkPeerType.Server)
			{
				GUI.Label(new Rect(100,100,100,25),"Server");
				GUI.Label(new Rect(100,125,100,25),"Connections:"+ Network.connections.Length);

				if (GUI.Button(new Rect(100,150,100,25),"Logout"))	
				{
					Network.Disconnect(250);
				}

			}
		}
	}
	[RPC]
	void ChangeColor()
	{
		player1.renderer.material.color = Color.blue;
	}
	[RPC]
	void AskColor()
	{
		if (Network.isServer){

			networkView.RPC("ChangeColor",RPCMode.All);
			Debug.Log("Server is Changing color");
		
		}
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
		GameObject player2 = (GameObject)Instantiate(player1 , location1.position ,transform.rotation);
	}
}

		
	
	
