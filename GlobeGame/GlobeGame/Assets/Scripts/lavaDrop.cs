using UnityEngine;
using System.Collections;

public class lavaDrop : MonoBehaviour {

	public Vector3 originalposition;
	public Vector3 direction;
	public float speed;
	public Quaternion originalrotation;

	public int i;
	// Use this for initialization
	void Start () {

    	originalposition = this.GetComponent<Transform>().transform.position;
		originalrotation = this.GetComponent<Transform> ().transform.rotation;
		direction = Vector3.zero - originalposition;
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<Rigidbody>().velocity = direction.normalized *speed;

	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag.Equals ("Meeple")) {
			GetComponent<MeepleController> ().HitByLava ();
			} if (collision.collider.tag.Equals ("liquid")) {
			//GetComponent<BoxCollider>().enabled = false;

			GetComponent<Transform>().transform.position = originalposition;
			GetComponent<Transform>().transform.rotation = originalrotation;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				//GetComponent<BoxCollider>().enabled = true;
				//i = 0 ;}
		



	}
}
}