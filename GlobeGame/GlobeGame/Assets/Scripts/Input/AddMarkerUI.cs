using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AddMarkerUI : MonoBehaviour
{

	bool placingMarker;
	GameObject currentNewMarker;
	Image markerImg;
	public Color normal;
	public Color placing;
	EventTrigger eventTrigger = null;

	public void AddMarker ()
	{
		if (!placingMarker) {
			placingMarker = true;

			//Vector3 spawnPos = Camera.main.ScreenToWorldPoint(markerImg.transform.position);
			//spawnPos = new Vector3 (spawnPos.x, spawnPos.y - 100.0f, spawnPos.z);
			currentNewMarker = (GameObject)Instantiate (Resources.Load ("Marker"));
			currentNewMarker.transform.position = new Vector3 (0, 0, 0);
			markerImg.color = placing;
			currentNewMarker.GetComponent<MeepleGravity>().gravity = GameManager.Instance.globe.GetComponent<GlobeGravity>();
		}


	}

	// Use this for initialization
	void Start ()
	{

		placingMarker = false;
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (t.name.Equals ("MarkerButton")) {
				this.markerImg = t.GetComponent<Image> ();
			}
		}

		eventTrigger = gameObject.GetComponent<EventTrigger> ();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		if (Input.touchCount > 0 && placingMarker) {
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			if (Physics.Raycast (ray, out hit, float.MaxValue, 1 << 12)) {
				currentNewMarker.GetComponent<Rigidbody> ().isKinematic = false;
				currentNewMarker.transform.position = hit.point;
				placingMarker = false;
				markerImg.color = normal;
			}
		}

		if (Input.GetMouseButtonDown (0) && placingMarker) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, float.MaxValue, 1 << 12)) {
				currentNewMarker.GetComponent<Rigidbody> ().isKinematic = false;
				currentNewMarker.transform.position = hit.point;
				placingMarker = false;
				markerImg.color = normal;
			}
		}
		
	}

}
