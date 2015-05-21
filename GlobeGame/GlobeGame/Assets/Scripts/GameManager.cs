using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	private static GameManager instance;
	public List<MeepleController> activeMeeples;
	public List<MeepleController> allMeeples;
	public int savedMeeples;
	public LevelGraph lGraph;
	public GameObject globe;
	public Camera arCam;
	Utilities help = new Utilities ();
	public bool placingMarker;

	public static GameManager Instance {
		get { return instance ?? (instance = new GameObject ("GameManager").AddComponent<GameManager> ()); }
	}


	void OnDrawGizmos ()
	{
		foreach (Tile t in lGraph.WalkableGraph) {
			if (t.Walkable) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere (t.WorldPos, 0.3f);
			} else {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere (t.WorldPos, 0.3f);
			}
		}
	}

	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad (this); //saving GM between scenes
		allMeeples = new List<MeepleController> ();
		activeMeeples = new List<MeepleController> ();

		GameObject[] meeples = GameObject.FindGameObjectsWithTag ("Meeple");
		foreach (GameObject meeple in meeples) {
			allMeeples.Add (meeple.GetComponent<MeepleController> ());
		}

		foreach (MeepleController mc in allMeeples) {
			if (mc.active) {
				activeMeeples.Add (mc);
			}
		}

		globe = GameObject.FindGameObjectsWithTag ("Globe") [0];

		LoadLevel1 ();
	
	
		//Set Main Camera
		Camera[] cams = Camera.allCameras;
		foreach (Camera cam in cams) {
			if (cam.tag.Equals ("MainCamera")) {
				this.arCam = cam;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (allMeeples.Count < 1) {
			GameObject[] meeples = GameObject.FindGameObjectsWithTag ("Meeple");
			foreach (GameObject meeple in meeples) {
				allMeeples.Add (meeple.GetComponent<MeepleController> ());
			}
			
			foreach (MeepleController mc in allMeeples) {
				if (mc.active) {
					activeMeeples.Add (mc);
				}
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				help.GetClickedTile(hit.point, lGraph.WalkableGraph, globe).Walkable = true;
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				help.GetClickedTile(hit.point, lGraph.WalkableGraph, globe).Walkable = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			help.SaveXMLFile(Application.dataPath + "/Scripts/Pathfinding/Level1.xml", lGraph);
			Debug.Log("Saved.");
		}

	}

	private void LoadLevel1 ()
	{
		lGraph = help.LoadXMLFile ("Level1");
	}

	private void LoadLevel2 ()
	{
		lGraph = help.LoadXMLFile ("Level2");
	}

}
