using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{

	Camera arCam;
	public GameObject globe;
	Utilities help = new Utilities ();
	bool vectorSet = false;
	Tile clicked;
	List<Tile> returnList = new List<Tile> ();
	World world = new World();
	LevelGraph lGraph;

	public GameObject testMeeple;
	List<Vector3> path;

	void OnDrawGizmos ()
	{
		if (vectorSet) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (clicked.WorldPos, 0.1f);


			foreach(Vector3 vec in path){
				Gizmos.color = Color.red;
				Gizmos.DrawSphere (vec, 0.1f);
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		//Set Main Camera
		Camera[] cams = Camera.allCameras;
		foreach (Camera cam in cams) {
			if (cam.tag.Equals ("MainCamera")) {
				this.arCam = cam;
			}
		}

		//lGraph = world.CreateLevelGraph (globe);
		//Debug.Log (lGraph.BasicGraph.Count);
		//help.SaveLevelGraph (lGraph, Application.dataPath + "/Scripts/Pathfinding/DummyLevelGraphTilesWithParents.lg");
		//Debug.Log ("Loading from: " + Application.dataPath +"/Scripts/Pathfinding/DummyLevelGraphTilesWithParents.lg");
		lGraph = help.LoadLevelGraph (globe, Application.dataPath + "/Scripts/Pathfinding/DummyLevelGraphTilesWithParents.lg");
		Debug.Log(lGraph.BasicGraph.Count);
		Debug.Log(lGraph.WalkableGraph.Count);

	}
	
	// Update is called once per frame
	void Update ()
	{

		
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = this.arCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				Vector3 target = hit.point;
				clicked = help.GetClickedTile (target, lGraph.BasicGraph, globe);
				vectorSet = true;
				AStar star = new AStar();

				//path = star.FindShortestPath(lGraph.BasicGraph, help.GetClickedTile(testMeeple.transform.position, lGraph.BasicGraph, globe), clicked);
				path = star.FindPath(lGraph.BasicGraph, help.GetClickedTile(testMeeple.transform.position, lGraph.BasicGraph, globe), clicked, globe);
			}
		}
	}
}
		