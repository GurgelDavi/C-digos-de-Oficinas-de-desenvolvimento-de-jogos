using UnityEngine;
using System.Collections;

public class LookNorth : MonoBehaviour {
	public GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("magneticNorth");
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.LookAt (target.transform);
		transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
	
	}
}
