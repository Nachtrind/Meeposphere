using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class World
{

	public List<GameObject> gridStartingPoints;

	// Use this for initialization
	public void GenerateWorld (float _tileSize)
	{
		foreach (GameObject obj in gridStartingPoints) {
			GenerateGraph (obj, _tileSize);
		}
	}

	private void GenerateGraph (GameObject _startingPoint, float _size)
	{

	}

}
