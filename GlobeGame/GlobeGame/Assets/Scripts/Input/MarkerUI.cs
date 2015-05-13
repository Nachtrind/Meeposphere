using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarkerUI : MonoBehaviour
{

	Utilities helper = new Utilities ();
	int currentMode = 0;
	bool mouseDown;

	//Color tints
	public Color call;
	public Color push;
	Image pushImg;
	Image activeImg;
	Image callImg;
	Image arrowImg;
	Vector3 center;
	float radius;

	// Update is called once per frame
	void Start ()
	{
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

		Debug.Log ("Local Pos: " + this.transform.localPosition + "WorldPos:  " + this.transform.position + "   pushimg local pos: " + pushImg.transform.localPosition + "   pushimg world pos: " + pushImg.transform.localPosition);

		radius = Vector3.Distance (this.transform.localPosition, transform.InverseTransformPoint (pushImg.transform.position));
		Debug.Log ("Radius is : " + radius);
	}

	// Update is called once per frame
	void Update ()
	{
		if (mouseDown) {

			Ray ray = GameManager.Instance.arCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			Vector3 target = new Vector3 (0, 0, 0);
			if (Physics.Raycast (ray, out hit, 100)) {
				target = hit.point;
			}

			if (Vector3.Distance (pushImg.transform.position, target) == this.radius) {
				pushImg.transform.position = target;
			} else {
				float localY = pushImg.transform.localPosition.y; 
				Vector3 newPos = target - center;
				newPos = Vector3.Normalize (newPos) * radius;
				pushImg.transform.position = newPos;
				pushImg.transform.localPosition = new Vector3 (pushImg.transform.localPosition.x, localY, pushImg.transform.localPosition.z); 

			}
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
			meeple.CalcNewPath (GameManager.Instance.lGraph.WalkableGraph, 
			                    this.helper.GetClickedTile (this.transform.position, 
			                            GameManager.Instance.lGraph.WalkableGraph, GameManager.Instance.globe), 
			                    GameManager.Instance.globe);
		}
	}

	public void PushMeeples ()
	{

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
			arrowImg.enabled = true;
		} else {
			arrowImg.enabled = false;

		}
	}


}
