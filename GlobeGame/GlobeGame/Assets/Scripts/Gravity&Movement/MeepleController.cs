﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	Quaternion targetRot;
	List<Vector3> currentPath;
	AStar star = new AStar ();
	Utilities help = new Utilities ();
	bool reachedTarget;
	public bool active;

	void OnDrawGizmos ()
	{

		/*foreach (Vector3 vec in currentPath) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere (vec, 0.2f);
		}*/

	}


	// Use this for initialization
	void Start ()
	{
		active = true; //for Testing, later it has to be false by default

		mTrans = transform;
		target = mTrans.position;

		rigid = GetComponent<Rigidbody> ();
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		rigid.useGravity = false;

		reachedTarget = true;

		//acceptable Distance is Meeple width
		acceptableDistance = GetComponent<MeshFilter> ().mesh.bounds.size.x * mTrans.localScale.x * 3;
		gotCalledByPlayer = false;

	}

	void Update ()
	{
		if (currentPath != null) {
			if (currentPath.Count > 0 && reachedTarget) {
				target = currentPath [0];
				currentPath.RemoveAt (0);
				reachedTarget = false;
				mTrans.rotation = Quaternion.LookRotation (target, mTrans.up);
				gravity.Gravitate (mTrans);
			} else if (currentPath.Count == 0 && reachedTarget) {
				gotCalledByPlayer = false;
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		gravity.Gravitate (mTrans);
		float currentDistance = Vector3.Distance (mTrans.position, target);

		if (gotCalledByPlayer) {
			if (currentDistance > acceptableDistance) {
				rigid.MovePosition (mTrans.position + mTrans.forward * speed * Time.deltaTime);
			} else {
				reachedTarget = true;
			}
		}
	}

	public void CalcNewPath (List<Tile> _graph, Tile _end, GameObject _globe)
	{
		this.currentPath = star.FindPath (_graph, help.GetClickedTile (transform.position, _graph, _globe), _end, _globe);
		target = currentPath [0];
		currentPath.RemoveAt (0);
		gotCalledByPlayer = true;
		reachedTarget = false;
		mTrans.rotation = Quaternion.LookRotation (target, mTrans.up);
		gravity.Gravitate (mTrans);
	}


}
