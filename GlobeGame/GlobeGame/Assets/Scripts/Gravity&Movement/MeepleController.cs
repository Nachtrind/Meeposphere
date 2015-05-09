using UnityEngine;
using System.Collections;

public class MeepleController : MonoBehaviour
{
	public GlobeGravity gravity;
	public Rigidbody rigid;
	Vector3 target;
	Transform mTrans;
	float acceptableDistance = 0.2f;
	float speed = 6.5f;
	Vector3 pushback;

	bool gotCalledByPlayer;
	bool hitObstacle;
	float obstacleTimer = 0.1f;

	Camera arCam;
	Quaternion targetRot;

	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
		target = mTrans.position;

		rigid = GetComponent<Rigidbody> ();
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		rigid.useGravity = false;


		//acceptable Distance is Meeple width
		acceptableDistance = GetComponent<MeshFilter> ().mesh.bounds.size.x * mTrans.localScale.x * 3;
		gotCalledByPlayer = false;

		//Set Main Camera
		Camera[] cams = Camera.allCameras;
		foreach (Camera cam in cams) {
			if (cam.tag.Equals ("MainCamera")) {
				this.arCam = cam;
			}
		}
	}

	void Update ()
	{
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

		if (currentDistance > acceptableDistance) {
			rigid.MovePosition (mTrans.position + mTrans.forward * speed * Time.deltaTime);

			//Check in Front
			Ray ray = new Ray (mTrans.position, mTrans.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 1)) {
				hitObstacle = true;
				pushback = mTrans.forward.normalized * -50;
				Debug.Log ("hitObstacle");
				target = transform.position;
				gotCalledByPlayer = false;
			}
		}

		if (hitObstacle && obstacleTimer > 0) {

			rigid.AddForce (pushback);
			obstacleTimer -= Time.deltaTime;
		} else if (hitObstacle && obstacleTimer <= 0) {
			hitObstacle = false;
			obstacleTimer = 0.1f;
		}

		gravity.Gravitate (mTrans);


	}






}
