using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Utilities
{


	public void SaveLevelGraph (LevelGraph _graph, string _path)
	{

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (_path);
		bf.Serialize (file, _graph);
		Debug.Log ("Saved Graph at: " + _path);
		file.Close ();
	}

	/*
	private SavableGraph CreateSavableGraph (LevelGraph _graph)
	{

		SavableGraph returnGraph = new SavableGraph ();
		List<SavableTile> nodesOnly = new List<SavableTile> ();
		List<SavableTile> basic = new List<SavableTile> ();
		List<SavableTile> walk = new List<SavableTile> ();

		foreach (Tile t in _graph.NodesOnly) {
			SavableTile sTile = new SavableTile (t.WorldPos, t.Neighbours, t.Walkable);
			nodesOnly.Add(sTile);
		}

		foreach (Tile t in _graph.BasicGraph) {
			SavableTile sTile = new SavableTile (t.WorldPos, t.Neighbours, t.Walkable);
			basic.Add (sTile);
		}

		foreach (Tile t in _graph.WalkableGraph) {
			SavableTile sTile = new SavableTile (t.WorldPos, t.Neighbours, t.Walkable);
			walk.Add (sTile);
		}

		returnGraph.nodesOnly = nodesOnly;
		returnGraph.basicGraph = basic;
		returnGraph.walkGraph = walk;

		return returnGraph;


	}
*/



	public LevelGraph LoadLevelGraph (GameObject _globe, string _path)
	{

		LevelGraph returnGraph = new LevelGraph ();

		if (File.Exists (_path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (_path, FileMode.Open);
			returnGraph = (LevelGraph)bf.Deserialize (file);
			file.Close ();
		} else {
			Debug.LogError("PATH DOESN'T EXIST");
		}

		returnGraph = Resources.LoadAssetAtPath (_path, LevelGraph);

		return returnGraph;
	}

	/*
	private LevelGraph CreateTileGraph (SavableGraph _graph, GameObject _globe)
	{

		LevelGraph returnGraph = new LevelGraph ();
		List<Tile> nodesOnly = new List<Tile>();
		List<Tile> basic = new List<Tile> ();
		List<Tile> walk = new List<Tile> ();

		foreach (SavableTile st in _graph.nodesOnly) {
			Tile t = new Tile (new Vector3 (st.pos.x, st.pos.y, st.pos.z));
			nodesOnly.Add(t);
		}

		Debug.Log ("Savable Tiles in basic: " + _graph.basicGraph.Count);
		foreach (SavableTile st in _graph.basicGraph) {
			Tile t = new Tile (new Vector3 (st.pos.x, st.pos.y, st.pos.z));
			t.Walkable = st.walkable;
			t.Neighbours = new List<Tile>();
			foreach (StupidVector neigh in st.neighbours) {
				t.Neighbours.Add(this.GetClickedTile(new Vector3(neigh.x, neigh.y, neigh.z), nodesOnly,_globe));
			}
		}

		//TODO: WALKABLE
		
		returnGraph.NodesOnly = nodesOnly;
		returnGraph.BasicGraph = basic;
		returnGraph.WalkableGraph = walk;

		return returnGraph;

	}*/

	public Vector3 NearestVertexTo (Vector3 _point, GameObject _obj)
	{ 

		Vector3 locPoint = _obj.transform.InverseTransformPoint (_point);

		Mesh mesh = _obj.GetComponent<MeshFilter> ().mesh;
		float minDistance = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;
	
		// scan all vertices to find nearest
		foreach (Vector3 vertex in mesh.vertices) {
			Vector3 distanceVec = locPoint - vertex;
			float distance = distanceVec.sqrMagnitude;
		
			if (distance < minDistance) {
				minDistance = distance;
				nearestVertex = vertex;
			}
		}
	
		// convert nearest vertex back to world space
		return _obj.transform.TransformPoint (nearestVertex);
	}

	//returns the index of the Tile that has been clicked
	public Tile GetClickedTile (Vector3 _clickHit, List<Tile> _tiles, GameObject _world)
	{
		Vector3 tilePos = this.NearestVertexTo (_clickHit, _world);
		foreach (Tile t in _tiles) {
			if (t.WorldPos == tilePos) {
				return t;
			}
		}

		return null;

	}


	//rotates an object on the surface of the globe
	//TODO: code cleanup
	public void RotateOnGlobeSurface (GameObject _obj, GameObject _globe)
	{

		Vector3 gravityUp = (_obj.transform.position - _globe.transform.position).normalized;
		Vector3 meepleUp = _obj.transform.up;

		Quaternion targetRot = Quaternion.FromToRotation (meepleUp, gravityUp) * _obj.transform.rotation;
		_obj.transform.rotation = Quaternion.Slerp (_obj.transform.rotation, targetRot, 50 * Time.deltaTime);


	}


}