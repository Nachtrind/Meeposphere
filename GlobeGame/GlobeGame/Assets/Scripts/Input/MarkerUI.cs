using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarkerUI : MonoBehaviour
{

	Utilities helper = new Utilities ();
	int currentMode = 0;

	//Color tints
	public Color call;
	public Color push;

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
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("Activate")) {
				t.GetComponent<Image> ().color = this.call;
			}

		}
	}

	public void SetToPush ()
	{
		this.currentMode = 1;
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("Activate")) {
				t.GetComponent<Image> ().color = this.push;
			}
			
		}
	}

}
