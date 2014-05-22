using UnityEngine;
using System.Collections;

public class Colisioncube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		Debug.Log("Colison");
		if (col.gameObject.tag == "Player") 
		{
			Debug.Log("Colison with player");
		}
	}
}
