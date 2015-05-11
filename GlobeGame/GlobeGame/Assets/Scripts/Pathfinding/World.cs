using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class World
{

	Utilities help = new Utilities ();
	float toAdd = 0.0f;
	float angleToAdd = 90.0f;
	int neighboursToCheck = 4;

	public List<Tile> GenerateWalkables (LevelGraph _graph, GameObject _world)
	{

		List<Tile> basic = _graph.BasicGraph;
		List<Tile> walkable = new List<Tile> ();

		//only Hit Obstacles
		int layerMask = 1 << 11;

		foreach (Tile t in basic) {
			Vector3 gravityUp = (t.WorldPos - _world.transform.position).normalized;
			RaycastHit hit;
			if (Physics.Raycast (t.WorldPos + (gravityUp*-2), t.WorldPos + gravityUp*2, out hit, Mathf.Infinity, layerMask)) {
				t.Walkable = false;
			}
			walkable.Add(t);
		}//END foreach

		return walkable;
	}

	public LevelGraph CreateLevelGraph (GameObject _world)
	{
		LevelGraph graph = new LevelGraph ();

		List<Tile> nodesOnly = new List<Tile> ();

		//collect all nodes
		Mesh mesh = _world.GetComponent<MeshFilter> ().mesh;
		foreach (Vector3 vertex in mesh.vertices) {
			Tile tile = new Tile (_world.transform.TransformPoint (vertex));
			tile.Walkable = true;
			nodesOnly.Add (tile);
		}

		graph.BasicGraph = this.GenerateBasicNodes (_world);

		//TODO: Create Walkable Graph
		graph.WalkableGraph = graph.BasicGraph;

		return graph;
	}

	public List<Tile> GenerateBasicNodes (GameObject _world)
	{

		List<Tile> returnList = new List<Tile> ();

		GameObject littleHelper = new GameObject ();

		Mesh mesh = _world.GetComponent<MeshFilter> ().mesh;


		//collects all Vertices and creates Tiles
		foreach (Vector3 vertex in mesh.vertices) {
			Tile tile = new Tile (_world.transform.TransformPoint (vertex));
			tile.Walkable = true;
			returnList.Add (tile);
		}


		foreach (Tile currentTile in returnList) {


			for (int i = 0; i < neighboursToCheck; i++) {

				bool foundTile = false;
			
				//rotate Helper, reset Distance
				littleHelper.transform.position = currentTile.WorldPos;
				littleHelper.transform.Rotate (new Vector3 (0, i * angleToAdd, 0), Space.Self);
				toAdd = 0.0f;
			
				while (!foundTile) {
				
					toAdd += 0.1f;
				
					Vector3 searchPoint = littleHelper.transform.position + (littleHelper.transform.forward.normalized * toAdd);				
					Vector3 next = help.NearestVertexTo (searchPoint, _world);
				
					//Nearest Vertex isn't self
					if (currentTile.WorldPos != next) {

						//check all Tiles positions to find the right one
						foreach (Tile ti in returnList) {
						
							if (ti.WorldPos == next) {
								bool inList = false;

								//check if Tile is already in List
								foreach (int neigh in currentTile.Neighbours) {
									if (ti.WorldPos == returnList [neigh].WorldPos) {
										inList = true;
										break;
									}
								}

								if (!inList) {
									currentTile.Neighbours.Add (returnList.IndexOf (ti));
									foundTile = true;
									break;
								}
							}
						}
					} 

					if (toAdd > 15) {
						foundTile = true;
					}
				}//END while
			}//END for
		}//END foreach
		return returnList;
	}

}
