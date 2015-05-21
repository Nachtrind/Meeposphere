using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{

	Image callImg;
	public float callRadius;
	public bool meeplesInReach;
	Utilities helper = new Utilities ();

	// Use this for initialization
	void Start ()
	{
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("End")) {
				this.callImg = t.GetComponent<Image> ();
			}
		}
		callRadius = 19.5f;	
		meeplesInReach = false;
		this.callImg.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		List<MeepleController> meeples = new List<MeepleController> ();
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, callRadius);
		foreach (Collider col in hitColliders) {
			if (col.tag.Equals ("Meeple")) {
				meeples.Add (col.GetComponent<MeepleController> ());
			}
		}
		Debug.Log (meeples.Count);
		if (meeples.Count > 0) {
			meeplesInReach = true;
			this.callImg.enabled = true;
		} else {
			meeplesInReach = false;
			this.callImg.enabled = false;
		}
	}

	public void CallMeeplesHome ()
	{
		if (meeplesInReach) {
			foreach (MeepleController meep in GameManager.Instance.activeMeeples) {
				meep.CalcNewPath (GameManager.Instance.lGraph.WalkableGraph, this.helper.GetClickedTile (this.transform.position, GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe), GameManager.Instance.globe);
			}
		}
	}

}