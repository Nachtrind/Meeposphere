using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeepleController : MonoBehaviour
{
	Transform mTrans;
	public GlobeGravity gravity;
	public Rigidbody rigid;
	public bool active;
	public bool idle;
	bool dead;

	//Stuff after being pushed
	public bool gotPushed;
	float pushTimer;
	float pushTime = 2.0f;

	//Stuff for material change
	public Material m_active;
	public Material m_asleep;
	public Material m_panic;
	public Material m_dead;
	Renderer meepleRenderer;

	//Collision with things
	Vector3 pushback;

	//Stuff for moving around
	public float acceptableDistance = 1.0f;
	public float tolerableDistance = 3.0f;
	float speed;
	float walkSpeed = 6.5f;
	float idleSpeed = 1.5f;
	float idleTime = 1.2f;
	float idleTimer = 0;
	Vector3 target;
	Vector3 lastTarget;
	bool gotCalledByPlayer;
	bool hitObstacle;
	float obstacleTimer = 0.5f;
	Quaternion targetRot;
	List<Vector3> currentPath;
	AStar star = new AStar ();
	Utilities help = new Utilities ();
	bool reachedTarget;

	//Stuff for Pausing, Unpausing
	Vector3 savedVelocity;
	Vector3 savedAngularVelocity;

	/*
	void OnDrawGizmos ()
	{
		foreach (Vector3 vec in currentPath) {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere (vec, 0.3f);
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (target, 0.3f);

	}*/
	
	
	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
		target = mTrans.position;
		lastTarget = target;

		rigid = GetComponent<Rigidbody> ();
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		rigid.useGravity = false;

		reachedTarget = true;

		//acceptable Distance is Meeple width
		//acceptableDistance = GetComponent<MeshFilter> ().mesh.bounds.size.x * mTrans.localScale.x;
		gotCalledByPlayer = false;

		this.meepleRenderer = GetComponentInChildren<Renderer> ();

		Debug.Log (meepleRenderer);
		//this.FindRandomPoint ();

		if (active) {
			this.SetActive ();
		} else {
			this.SetAsleep ();
		}

		dead = false;

	}

	void Update ()
	{
		if (!gotPushed && !dead) {
			//Pathfollowing
			if (currentPath != null) {
				if (currentPath.Count > 0 && reachedTarget) {
					target = currentPath [0];
					lastTarget = currentPath [0]; //save target for idle-behaviour
					currentPath.RemoveAt (0);
					reachedTarget = false;
//					Debug.Log("Set New Step");
					//mTrans.rotation = Quaternion.LookRotation (target, mTrans.up);
					gravity.Gravitate (mTrans);
				} else if (currentPath.Count == 0 && reachedTarget) {
					gotCalledByPlayer = false;
					idle = true;
				}
			}

			//Idling
			if (idle && idleTimer > idleTime && reachedTarget) {
				speed = idleSpeed;
				//this.FindRandomPoint ();
				idleTimer = 0.0f;

			} else if (!gotCalledByPlayer) {
				idleTimer += Time.deltaTime;
			}
		} else if (pushTimer > pushTime) {
			if (IsGrounded () && !dead) {
				gotPushed = false;
				SetActive ();
			}
		} else {
			pushTimer += Time.deltaTime;
		}
		
	}


	// Update is called once per frame
	void FixedUpdate ()
	{

		float currentDistance = Vector3.Distance (mTrans.position, target);
		mTrans.LookAt (target);
		if (!hitObstacle && !dead) {
			if (!gotPushed) {
				if (gotCalledByPlayer || idle) {
					if (currentDistance > acceptableDistance) {
						rigid.MovePosition (mTrans.position + mTrans.forward * speed * Time.deltaTime);
					} else {
						reachedTarget = true;
					}
				}
			} 
		} else {
			if (obstacleTimer > 0.5f) {
				hitObstacle = false;
				if (currentDistance < tolerableDistance && !dead) {
					target = transform.position;
					this.SetActive ();
				}
			} else {
				obstacleTimer += Time.deltaTime;
			}

		}

		//only check for Meeples
		if (active) {
			int layerMask = 1 << 14;
			Collider[] hitColliders = Physics.OverlapSphere (mTrans.position, 0.5f, layerMask);
			if (hitColliders.Length > 0) {
				Debug.Log ("hit meeple");
				foreach (Collider col in hitColliders) {
					if (!col.transform.GetComponent<MeepleController> ().active) {
						if (currentPath.Count > 0) {
							col.transform.GetComponent<MeepleController> ().WakeUp (currentPath [currentPath.Count - 1]);
						} else {
							col.transform.GetComponent<MeepleController> ().WakeUp (this.target);
						}
					}
				}
			}
		}
		
		//Check in Front
		/*
		if (!hitObstacle && !gotPushed) {
			Ray ray = new Ray (mTrans.position, mTrans.forward);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit, 0.5f)) {
				pushback = mTrans.forward.normalized * -3;
				hitObstacle = true;
				Debug.Log ("hitObstacle");
				this.rigid.AddForce (pushback, ForceMode.Impulse);
				target = transform.position;
				obstacleTimer = 0.0f;
				this.SetPaniced ();
			
			}
		}*/

		
		gravity.Gravitate (mTrans);
	}

	public void CalcNewPath (List<Tile> _graph, Tile _end, GameObject _globe)
	{
		this.currentPath = star.FindPath (_graph, help.GetClickedTile (transform.position, _graph, _globe), _end, _globe);
		target = currentPath [0];
		currentPath.RemoveAt (0);
		gotCalledByPlayer = true;
		idle = false;
		reachedTarget = false;
		mTrans.LookAt (target);;
		gravity.Gravitate (mTrans);
		speed = walkSpeed;
	}

	private void FindRandomPoint ()
	{
		Vector3 newDir = new Vector3 (Random.Range (-10.0f, 10.0f), 0.0f, Random.Range (-10.0f, 10.0f)).normalized;
		Vector3 newTarget = mTrans.position + newDir * Random.Range (-1.2f, 1.2f);
		Tile targetTile = help.GetClickedTile (newTarget, GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe);
		newTarget = targetTile.WorldPos;
		float distance = Vector3.Distance (lastTarget, newTarget);
		while (distance > tolerableDistance || !targetTile.Walkable) {

			newDir = new Vector3 (Random.Range (-10.0f, 10.0f), 0.0f, Random.Range (-10.0f, 10.0f)).normalized;
			newTarget = mTrans.position + newDir * Random.Range (-1.2f, 1.2f);
			newTarget = help.GetClickedTile (newTarget, GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe).WorldPos;
			distance = Vector3.Distance (lastTarget, newTarget);
		}
		target = newTarget;
		//mTrans.rotation = Quaternion.LookRotation (target, mTrans.up);
		reachedTarget = false;

	}

	public void WakeUp (Vector3 _target)
	{
		this.SetActive ();
		this.active = true;
		Tile targetTile = help.GetClickedTile (_target, GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe);
		this.CalcNewPath (GameManager.Instance.lGraph.WalkableGraph, targetTile, GameManager.Instance.globe);
		this.gameObject.layer = 9;
		GameManager.Instance.activeMeeples.Add (this);
	}

	public void PushedAway ()
	{
		gotPushed = true;
		SetPaniced ();
		pushTimer = 0.0f;
	}

	public void Pause ()
	{
		savedVelocity = rigid.velocity;
		savedAngularVelocity = rigid.angularVelocity;
		rigid.isKinematic = true;
	}

	public void Resume ()
	{
		rigid.isKinematic = false;
		rigid.AddForce (savedVelocity, ForceMode.VelocityChange);
		rigid.AddTorque (savedAngularVelocity, ForceMode.VelocityChange);
	}

	private void SetPaniced ()
	{
		Material[] mats = new Material[]{m_panic};
		meepleRenderer.materials = mats;
	}

	private void SetActive ()
	{
		Material[] mats = new Material[]{m_active};
		meepleRenderer.materials = mats;
	}

	private void SetAsleep ()
	{
		Material[] mats = new Material[]{m_asleep};
		meepleRenderer.materials = mats;
	}

	private void SetDead ()
	{
		Material[] mats = new Material[]{m_dead};
		meepleRenderer.materials = mats;
	}

	private bool IsGrounded ()
	{

		bool grounded = false;

		Debug.DrawRay (mTrans.position + (mTrans.up * 1.0f), mTrans.up * -1.0f, Color.red, 20.0f);
		RaycastHit hit;

		if (Physics.Raycast (mTrans.position + (mTrans.up * 1.0f), mTrans.up * -1.0f, out hit, 1.0f)) {
			target = mTrans.position;
			lastTarget = mTrans.position;
			grounded = true;
			Debug.Log ("Is Grounded");
		}
		return grounded;
	}

	private void Dead ()
	{
		this.SetDead ();
		dead = true;
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.tag.Equals ("death")) {
			this.Dead ();
		}
		
	}

}
