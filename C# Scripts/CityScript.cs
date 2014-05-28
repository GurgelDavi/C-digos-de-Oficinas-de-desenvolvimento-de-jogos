using UnityEngine;
using System.Collections;

public class CityScript : MonoBehaviour {
	public bool hasKey = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void askCityKeyLoss ()
	{
		networkView.RPC( "globalLooseKey",RPCMode.All );
	}
	[RPC]
	void globalLooseKey (){
		if (Network.isServer)
			networkView.RPC("looseKey",RPCMode.All);
		}
	[RPC]
	void looseKey()
	{
		this.hasKey = false;
	}
}
