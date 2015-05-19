using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarkerUI : MonoBehaviour
{

	Utilities helper = new Utilities ();
	int currentMode = 0;
	bool mouseDown;

	//Color tints
	public float force;
	public Color call;
	public Color push;
	Image pushImg;
	Image activeImg;
	Image callImg;
	Image arrowImg;
	Vector3 center;
	float radius;
	public float forceRadius = 6.0f;
	Vector3 target;

	// Update is called once per frame
	void Start ()
	{
		
		target = new Vector3 (0, 0, 0);
		mouseDown = false;

		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("Activate")) {
				this.activeImg = t.GetComponent<Image> ();
			}
		}
		
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("Arrow")) {
				this.arrowImg = t.GetComponent<Image> ();
			}
		}

		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("Push")) {
				this.pushImg = t.GetComponent<Image> ();
			}
		}
		radius = Vector3.Distance (new Vector3 (0, 0, 0), pushImg.rectTransform.anchoredPosition3D);
		Debug.Log (radius);		
		force = 11.5f;
		forceRadius = 6.0f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (mouseDown) {

			Ray ray = GameManager.Instance.arCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				target = hit.point;
			}

			Vector3 pointOnCircle = transform.InverseTransformPoint (target).normalized * radius;
			pushImg.rectTransform.anchoredPosition3D = new Vector3 (pointOnCircle.x, pointOnCircle.y, pushImg.rectTransform.anchoredPosition3D.z);
			pushImg.transform.LookAt (new Vector3 (this.transform.position.x, 0, this.transform.position.z));	
			
		}
		
	}

	public void Activate ()
	{
		switch (currentMode) {

		case 0:
			CallMeeples ();
			break;

		case 1:
			PushMeeples ();
			break;
		}

	}

	public void CallMeeples ()
	{
		foreach (MeepleController meeple in GameManager.Instance.activeMeeples) {
			Debug.Log(this.helper.GetClickedTile (this.transform.position, 
			                                      GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe).WorldPos);
			meeple.CalcNewPath (GameManager.Instance.lGraph.WalkableGraph, 
			                    this.helper.GetClickedTile (this.transform.position, 
			                            GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe), 
			                    GameManager.Instance.globe);
		}
	}

	public void PushMeeples ()
	{
		Vector3 pushDirection = pushImg.transform.position - this.transform.position;

		Vector3 upDir = GetComponentInParent<Transform> ().forward * -1;
		//Debug.DrawRay (this.transform.position, upDir, Color.green, 5.0f);
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, forceRadius);

		pushDirection = pushDirection + upDir.normalized * 5.0f;
		foreach (Collider col in hitColliders) {
			if (col.tag.Equals ("Meeple")) {
				Debug.Log("Col was Meeple");
				col.attachedRigidbody.AddForce (pushDirection.normalized * force, ForceMode.Impulse);
				col.GetComponentInParent<MeepleController> ().PushedAway ();
			}
		}

	}

	public void SetToCall ()
	{
		this.currentMode = 0;
		this.activeImg.GetComponent<Image> ().color = this.call;

	}

	public void SetToPush ()
	{
		this.currentMode = 1;
		this.activeImg.GetComponent<Image> ().color = this.push;
			

	}

	public void ToggleMouseDown ()
	{
		mouseDown = !mouseDown;
		if (mouseDown) {
			this.currentMode = 1;
			this.activeImg.GetComponent<Image> ().color = this.push;
			arrowImg.enabled = true;
		} else {
			arrowImg.enabled = false;

		}
	}


}
