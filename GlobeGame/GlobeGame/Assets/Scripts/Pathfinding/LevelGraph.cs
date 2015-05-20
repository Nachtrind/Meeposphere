using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("LevelGraph")]
public class LevelGraph {

	//the basic Graph with all nodes an their neighbours. No walkables set.
	[XmlArray("BasicGraph")]
	[XmlArrayItem("BasicItem")]
	List<Tile> basicGraph;

	//Graph completely set with walkables.
	[XmlArray("WalkableGraph")]
	[XmlArrayItem("WalkItem")]
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
