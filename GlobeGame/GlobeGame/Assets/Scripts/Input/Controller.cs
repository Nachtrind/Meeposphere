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
	World world = new World ();
	LevelGraph lGraph;
	List<Tile> walkables = new List<Tile> ();
	public GameObject testMeeple;
	List<Vector3> path;

	void OnDrawGizmos ()
	{
		/*foreach (Tile t in lGraph.WalkableGraph) {
			if (t.Walkable) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere (t.WorldPos, 0.3f);
			} else {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere (t.WorldPos, 0.3f);
			}
		}*/
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

		/*lGraph = help.LoadXMLFile("Level2X-8.xml");
		LevelGraph graphi = new LevelGraph ();
		graphi.BasicGraph = lGraph.BasicGraph;
		graphi.WalkableGraph = world.GenerateWalkables (lGraph, globe);
		help.SaveXMLFile(Application.dataPath + "/Scripts/Pathfinding/Level2Fix-8.xml", graphi);

		Debug.Log ("Graphi" + graphi.WalkableGraph.Count);*/
/*
		lGraph = help.LoadXMLFile ("Level2Fix-8.xml");
		Debug.Log (lGraph.WalkableGraph.Count);
		lGraph.WalkableGraph = world.GenerateWalkables (lGraph, globe);
		lGraph.WalkableGraph = world.AlternativeGenerateWalkables (lGraph, globe);
		help.SaveXMLFile (Application.dataPath + "/Scripts/Pathfinding/Level2-Obstacles.xml", lGraph);*/


		/*lGraph = world.CreateLevelGraph (globe);
		Debug.Log (lGraph.BasicGraph.Count);
		help.SaveLevelGraph (lGraph, Application.dataPath + "/Scripts/Pathfinding/Level2.lg");
		Debug.Log ("Loading from: " + Application.dataPath +"/Scripts/Pathfinding/Level2.lg");
		lGraph = help.LoadLevelGraph (globe, Application.dataPath + "/Scripts/Pathfinding/Level2.lg");
		Debug.Log (lGraph.BasicGraph.Count);
		Debug.Log (lGraph.WalkableGraph.Count);
		walkables = lGraph.WalkableGraph;
		walkables = world.GenerateWalkables (lGraph, globe);
		lGraph.WalkableGraph = walkables;
		help.SaveLevelGraph (lGraph, Application.dataPath + "/Scripts/Pathfinding/Level2.lg");

		lGraph = world.CreateLevelGraph (globe);
		walkables = world.GenerateWalkables (lGraph, globe);
		help.SaveXMLFile (Application.dataPath + "/Scripts/Pathfinding/Level2X-8.xml", lGraph);
		Debug.Log (lGraph.BasicGraph.Count);
		Debug.Log (lGraph.WalkableGraph.Count);*/
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
				testMeeple.GetComponent<MeepleController> ().CalcNewPath (lGraph.WalkableGraph, clicked, globe);
				//AStar star = new AStar ();

				//path = star.FindShortestPath(lGraph.BasicGraph, help.GetClickedTile(testMeeple.transform.position, lGraph.BasicGraph, globe), clicked);
				//path = star.FindPath (lGraph.WalkableGraph, help.GetClickedTile (testMeeple.transform.position, lGraph.BasicGraph, globe), clicked, globe);
			}
		}
	}
}
		