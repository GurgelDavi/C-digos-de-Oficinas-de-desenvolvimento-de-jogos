using UnityEngine;
using System.Collections;

public class vesselPlayer : MonoBehaviour {
	Vector3 screenHealthBarDisplay = new Vector3();
	public GameObject myBoat ;
	public GameObject enemy =null ;
	public bool enemyOnRange = false;
	public bool cityOnRange;
	public bool displayGUI = false;
	public bool canDig=false;
	public float range = 5 ;
	public int firePower = 3;
	public int defense = 2	 ;
	//int movesPerTurn = 1;
	public int maxHealth = 3;
	public int currentHealth = 3;
	public int resources = 10;
	public int turnMoves = 4;
	public int maxTurnMoves = 4;
	public GameObject closecity;
	public CityScript city;
	public bool gotUpgrade = false;
	public bool gotKey=false;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if (enemy != null) {
			float distance = Vector3.Distance (transform.position, this.enemy.transform.position);
			Debug.Log (distance);
			if (distance <= this.range) {
				this.enemyOnRange = true;
				//Debug.Log("Onrange"+enemyOnRange);
			} else
				this.enemyOnRange = false;
		}
	}


	void OnTriggerEnter (Collider col)
	{	
		Debug.Log("Colided!");
		if (col.gameObject.tag == "city" ) 
		{
			closecity=col.gameObject;
			city = closecity.GetComponent<CityScript>();
			Debug.Log("City on range");
			cityOnRange=true;
		}
		if (col.gameObject.tag == "digSpot" ) 
		{
			this.canDig=true;

			Debug.Log("City on range");
		}
	}
//	void OnTriggerStay(Collider col)
//	{	
//		if (col.gameObject.tag == "city" ) 
//		{
//			Debug.Log("TriggerStayColided!");
//			Debug.Log("City on range");
//			cityOnRange=true;
//		}
//	}
	void OnTriggerExit (Collider col)
	{	
		if (col.gameObject.tag == "city" ) 
		{
			Debug.Log("City on range");
			cityOnRange=false;
		}
		if (col.gameObject.tag == "digSpot" ) 
		{
			this.canDig=false;
		}
	}
}
