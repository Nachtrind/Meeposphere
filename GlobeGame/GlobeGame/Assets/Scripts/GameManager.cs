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
		lGraph = help.LoadLevelGraph (globe, Application.dataPath + "/Scripts/Pathfinding/Level2.lg");
		Debug.Log (lGraph.WalkableGraph.Count);
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
		
	}

}
