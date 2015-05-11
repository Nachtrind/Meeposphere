using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SavableTile {

	public bool walkable;
	public StupidVector pos;
	public List<StupidVector> neighbours = new List<StupidVector>();
	
	public SavableTile (Vector3 _worldPos, List<Tile> _neigh, bool _walkable)
	{
		pos = new StupidVector(_worldPos);
		walkable = _walkable;

		foreach(Tile t in _neigh){
			neighbours.Add(new StupidVector(t.WorldPos));
		}
	}

}
