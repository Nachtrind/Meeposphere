using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public class Tile : IHeapItem<Tile>
{

	bool walkable;

	[XmlElement]
	public string xS;
	[XmlElement]
	string yS;
	[XmlElement]
	string zS;
	List<int> neighbours = new List<int> ();
	int mode;
	Tile parent;
	bool inList;
	int gCost;
	int hCost;
	int heapIndex;

	float x;
	float y;
	float z;

	public Tile ()
	{

	}

	public Tile (Vector3 _worldPos)
	{
		this.x = _worldPos.x;
		this.y = _worldPos.y;
		this.z = _worldPos.z;

		MakeStrings ();
		
		inList = false;
	}

	public List<int> Neighbours {
		get {
			return neighbours;
		}
		set {
			neighbours = value;
		}
	}

	public Vector3 WorldPos {
		get {

				x = float.Parse(this.xS);
				y = float.Parse(this.yS);
				z = float.Parse(this.zS);

			return new Vector3 (x, y, z);
		}
	}

	public bool Walkable {
		get {
			return walkable;
		}
		set {
			walkable = value;
		}
	}

	public Tile Parent {
		get {
			return parent;
		}

		set {
			parent = value;
		}
	}

	public bool InList {
		get {
			return inList;
		}
		set {
			inList = value;
		}
	}

	public int Mode {
		get {
			return mode;
		}
		set {
			mode = value;
		}
	}

	public int GCost {
		get {
			return gCost;
		}
		set {
			gCost = value;
		}
	}

	public int HCost {
		get {
			return hCost;
		}
		set {
			hCost = value;
		}
	}

	public int FCost ()
	{
		return (gCost + hCost);
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public string Xs {
		get {
			return this.xS;
		}
		set {
			xS = value;
		}
	}

	public string Ys {
		get {
			return this.yS;
		}
		set {
			yS = value;
		}
	}

	public string Zs {
		get {
			return this.zS;
		}
		set {
			zS = value;
		}
	}


	public void MakeStrings ()
	{
		this.xS = this.x.ToString ();
		this.yS = this.y.ToString ();
		this.zS = this.z.ToString ();
	}

	public int CompareTo (Tile _compTile)
	{
		int compare = FCost ().CompareTo (_compTile.FCost ());
		if (compare == 0) {
			compare = hCost.CompareTo (_compTile.HCost);

		}
		return -compare;
	}

}