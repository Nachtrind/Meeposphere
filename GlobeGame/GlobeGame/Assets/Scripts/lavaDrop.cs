using UnityEngine;
using System.Collections;

public class lavaDrop : MonoBehaviour {

	public Vector3 originalposition;

	// Use this for initialization
	void Start () {
		
    	originalposition = this.GetComponent<Transform>().transform.position;

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag.Equals ("Meeple")) {
			GetComponent<MeepleController> ().HitByLava ();
			} if (collision.collider.tag.Equals ("Globe")) {
				originalposition =GetComponent<Transform>().transform.position ;
		}
	}
}
