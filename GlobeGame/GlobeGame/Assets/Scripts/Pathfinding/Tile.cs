using UnityEngine;
using System.Collections;

public class Tile
{

	bool walkable;
	float size;
	float posX;
	float posY;

	// 1  2  3
	// 4  5  6
	// 7  8  9
	//Each node can have up to 8 neighbours, 5 is self
	Tile[] neighbours = new Tile[9];

	public Tile (float _x, float _y, float _size, bool _walkable)
	{

		this.posX = _x;
		this.posY = _y;
		this.size = _size;
		this.walkable = _walkable;

	}

}
