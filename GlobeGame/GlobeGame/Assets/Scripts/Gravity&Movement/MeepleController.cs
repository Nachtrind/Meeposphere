using UnityEngine;
using System.Collections;

public class MeepleController : MonoBehaviour
{


	Rigidbody rigid;
	Vector3 target;
	Transform mTrans;
	float acceptableDistance = 0.2f;
	float speed = 1.5f;
	bool gotCalledByPlayer;

	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
		rigid = GetComponent<Rigidbody> ();
		target = mTrans.position;

		//acceptable Distance is Meeple width
		acceptableDistance = GetComponent<MeshFilter> ().mesh.bounds.size.x * mTrans.localScale.x;
		gotCalledByPlayer = false;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {

			gotCalledByPlayer = true;

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit, 100)) {
				target = hit.point;
				Vector3 forwardDir = target - mTrans.position;
				forwardDir.y = 0;
				mTrans.rotation = Quaternion.LookRotation (forwardDir);
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float currentDistance = Vector3.Distance (mTrans.position, target);
		if (currentDistance > acceptableDistance) {
			rigid.MovePosition (mTrans.position + mTrans.forward * speed * Time.deltaTime);
		}
	}






}
