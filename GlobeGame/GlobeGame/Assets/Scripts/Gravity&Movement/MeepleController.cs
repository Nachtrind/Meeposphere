using UnityEngine;
using System.Collections;

public class MeepleController : MonoBehaviour
{
	public GlobeGravity gravity;

	public Rigidbody rigid;
	Vector3 target;
	Transform mTrans;
	float acceptableDistance = 0.2f;
	float speed = 4.5f;
	bool gotCalledByPlayer;
	Camera arCam;
	Quaternion targetRot;

	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
		rigid = GetComponent<Rigidbody> ();
		target = mTrans.position;


		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		rigid.useGravity = false;




		//acceptable Distance is Meeple width
		acceptableDistance = GetComponent<MeshFilter> ().mesh.bounds.size.x * mTrans.localScale.x * 3;
		gotCalledByPlayer = false;
		Camera[] cams = Camera.allCameras;
		foreach (Camera cam in cams) {
			if (cam.tag.Equals ("MainCamera")) {
				this.arCam = cam;
			}
		}
		Debug.Log ("CAMERA: " + this.arCam);
	}

	void Update ()
	{

		//Debug: Show forward direction
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 5;
		Debug.DrawRay (transform.position, forward, Color.blue);


		if (Input.GetMouseButtonDown (0)) {

			gotCalledByPlayer = true;

			Ray ray = this.arCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit, 100)) {
				target = hit.point;
				mTrans.rotation = Quaternion.LookRotation (target, mTrans.up);
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{		
		float currentDistance = Vector3.Distance (mTrans.position, target);

		Debug.Log ("target: " + target + "/Current Position" + mTrans.position);
		Debug.Log ("CurrentDistance: " + currentDistance + "/" + acceptableDistance);


		if (currentDistance > acceptableDistance) {
			rigid.MovePosition (mTrans.position + mTrans.forward * speed * Time.deltaTime);
		}


		gravity.Gravitate (mTrans);
	}






}
