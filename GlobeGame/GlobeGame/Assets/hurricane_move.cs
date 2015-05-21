using UnityEngine;
using System.Collections;

public class hurricane_move : MonoBehaviour {

	public Vector3 originalposition;
	public float move_time;
	public float speed;
	public GameObject target;
	public GameObject start;

	public Vector3 targetposition;
	public Vector3 direction;
	public Vector3 backdirection;


	// Use this for initialization
	void Start () {

		originalposition = start.GetComponent<Transform>().position;
		targetposition = target.GetComponent<Transform>().position;
		direction = targetposition - originalposition;
		backdirection = originalposition - targetposition;
	
	}
	//private void Normalize(direction);
	//{}

	// Update is called once per frame
	void Update () {

		transform.Rotate (0,100*Time.deltaTime,0);

		if(Mathf.PingPong(Time.time,move_time) < move_time/2){
			GetComponent<Rigidbody>().velocity = direction.normalized *speed;}
		else {GetComponent<Rigidbody>().velocity = backdirection.normalized *speed;}

		 //GetComponent<BoxCollider>().
//		GetComponent<Rigidbody> ().angularVelocity =(0, 100, 0);
	}
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag.Equals("Meeple")) {
			GetComponent<MeepleController>().HitByHurricane();

		}
		}
}
