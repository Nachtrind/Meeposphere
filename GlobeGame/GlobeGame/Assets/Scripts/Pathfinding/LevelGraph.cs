using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class LevelGraph {

	//the basic Graph with all nodes an their neighbours. No walkables set.
	List<Tile> basicGraph;

	//Graph completely set with walkables.
	List<Tile> walkableGraph;



	////////////////////
	//GETTER & SETTER //
	////////////////////
	public List<Tile> WalkableGraph {
		get {
			return walkableGraph;
		}
		set{
			walkableGraph = value;
		}
	}

	public List<Tile> BasicGraph {
		get {
			return basicGraph;
		}

		set{
			basicGraph = value;
		}
	}
}
