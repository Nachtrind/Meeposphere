using UnityEngine;
using System.Collections;

public class MeepleGravity : MonoBehaviour
{

	private Transform meepleTrans;
	public GlobeGravity gravity;
	public Rigidbody rigid;

	// Use this for initialization
	void Start ()
	{
		rigid = GetComponent<Rigidbody> ();
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		rigid.useGravity = false;
		meepleTrans = transform;	

		gravity = GameObject.FindGameObjectWithTag ("Globe").GetComponent<GlobeGravity> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		gravity.Gravitate (meepleTrans);
	}
}
