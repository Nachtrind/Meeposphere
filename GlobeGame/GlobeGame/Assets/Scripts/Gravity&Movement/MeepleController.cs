using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour
{


	Rigidbody rigid;
	Vector3 target;
	Transform mTrans;
	float acceptableDistance = 0.5f;

	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
		rigid = GetComponent<Rigidbody> ();
		target = mTrans.position;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit, 100)) {
				target = hit.point;
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float currentDistance = Vector3.Distance (mTrans.position, target);
		if (currentDistance > acceptableDistance) {
			Vector3 forwardDir = target - mTrans.position;
			forwardDir.y = 0;
			mTrans.rotation = Quaternion.LookRotation (forwardDir);
		}
	}






}
