using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForce : MonoBehaviour
{

	public float force = 1000;
	// Use this for initialization
	void Start ()
	{
		var r = GetComponent<Rigidbody>();
		var v = Random.onUnitSphere;
		v = new Vector3(Mathf.Abs(v.x),Mathf.Abs(v.y),Mathf.Abs(v.z));
		v *= force;		
		r.AddForce(v);
	}
	
}
