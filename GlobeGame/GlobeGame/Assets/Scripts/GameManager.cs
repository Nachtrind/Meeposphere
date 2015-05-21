using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	private static GameManager instance;
	public List<MeepleController> activeMeeples;
	public List<MeepleController> allMeeples;
	public LevelGraph lGraph;
	public GameObject globe;
	public Camera arCam;
	Utilities help = new Utilities ();

	public bool placingMarker;

	public static GameManager Instance {
		get { return instance ?? (instance = new GameObject ("GameManager").AddComponent<GameManager> ()); }
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
		//Level2
		lGraph = help.LoadXMLFile ("Level2-Obstacles.xml");
//		Debug.Log ("Loaded Graph");
//		Debug.Log (lGraph.WalkableGraph.Count);
//		Debug.Log (lGraph.WalkableGraph[0].xS);
//		Debug.Log (lGraph.WalkableGraph [10].WorldPos);
		//DummyLevel
		//lGraph = help.LoadLevelGraph (globe, Application.dataPath + "/Scripts/Pathfinding/DummyLevelGraphTilesWithParents.lg");

	
	
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

		
	}



}
