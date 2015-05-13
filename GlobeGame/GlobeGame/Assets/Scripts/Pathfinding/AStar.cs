﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar
{


	public List<Vector3>  FindPath (List<Tile> _graph, Tile _start, Tile _end, GameObject _globe)
	{

		List<Tile> openSet = new List<Tile> ();
		HashSet<Tile> closedSet = new HashSet<Tile> ();
		List<Vector3> path = new List<Vector3> ();

		openSet.Add (_start);

		int stepCount = 0;
		while (openSet.Count > 0) {

			//Failsafe so unity doesn't break
			stepCount++;
			if(stepCount > 10000){
				Debug.Log("TOO MANY STEPS!");
				return null;
			}


			Tile current = openSet [0];
			for (int i = 0; i < openSet.Count; i++) {
				if (current.FCost() > openSet [i].FCost() || (current.FCost() == openSet [i].FCost() && openSet [i].HCost < current.HCost)) {
					current = openSet [i];
				}
			}//END for

			openSet.Remove (current);
			closedSet.Add (current);

			//End is found
			if (current == _end) {
				path = this.Backtracking(_start, _end);
				Debug.Log("RETURNLISTSIZE: " + path.Count);
				return path;
			}else{
				Debug.Log("Current isn't end.");
			}

			foreach (int neigh in current.Neighbours) {
				if (!_graph [neigh].Walkable || closedSet.Contains (_graph [neigh])) {
					Debug.Log("Current Neighbour is not walkable or already in ClosedSet");
					continue;
				}

				int newMovementCostToNeighbour = current.GCost + CircleDistance(current.WorldPos, _graph[neigh].WorldPos, _globe);
				if(newMovementCostToNeighbour < _graph[neigh].GCost || !openSet.Contains(_graph[neigh])){
					_graph[neigh].GCost = newMovementCostToNeighbour;
					_graph[neigh].HCost = CircleDistance(_graph[neigh].WorldPos, _end.WorldPos, _globe);
					_graph[neigh].Parent = current;

					if(!openSet.Contains(_graph[neigh])){
						Debug.Log("OpenSet doesn't contain neighbour");
						openSet.Add(_graph[neigh]);
					}
				}
			}//foreach

			Debug.Log ("OPEN SET CONTAINS: " + openSet.Count);

		}//END while

		Debug.Log ("RETURN NULL!");
		Debug.Log ("PATHSIZE: " + path.Count);
		path = this.Backtracking(_start, _end);
		return path;

	}


	private List<Vector3> Backtracking(Tile _start, Tile _end){


		Debug.Log ("BACKTRACKING!");
		List<Vector3> path = new List<Vector3> ();
		Tile current = _end;

		while (current != _start) {
			path.Insert(0, current.WorldPos);
			current = current.Parent;
		}

		return path;


	}

	private int ApproximateDistance (Vector3 _start, Vector3 _end)
	{

		int distance = Mathf.RoundToInt (Mathf.Abs (_end.x - _start.x) + Mathf.Abs (_end.y - _start.y) + Mathf.Abs (_end.z - _start.z));
		return distance;
	}

	private int CircleDistance (Vector3 _start, Vector3 _end, GameObject _globe)
	{
		Vector3 spokeToActual = _start - _globe.transform.position;
		Vector3 spokeToCorrect = _end - _globe.transform.position;
		float angleFromCenter = Vector3.Angle (spokeToActual, spokeToCorrect);
		
		float distance = 2 * Mathf.PI * (_globe.GetComponent<Renderer> ().bounds.size.x) * (angleFromCenter / 360);
		distance = distance * 1000.0f;
		Debug.Log ("Distance: " + Mathf.RoundToInt (distance));
		return Mathf.RoundToInt(distance);
	}



}
