using UnityEngine;
using System.Collections;

public class vesselPlayer : MonoBehaviour {
	Vector3 screenHealthBarDisplay = new Vector3();
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
	// Use this for initialization
	void Start () {
	
	}
	
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


	void OnTriggerEnter (Collider col)
	{	
		if (col.gameObject.tag == "city" ) 
		{
			Debug.Log("City on range");
			cityOnRange=true;
		}
	}
	void OnTriggerExit (Collider col)
	{	
		if (col.gameObject.tag == "city" ) 
		{
			Debug.Log("City on range");
			cityOnRange=false;
		}
	}
}
