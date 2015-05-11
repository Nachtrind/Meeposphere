using UnityEngine;
using System.Collections;

[System.Serializable]
public class StupidVector{

	public float x;
	public float y;
	public float z;

	public StupidVector(Vector3 _vec){
		this.x = _vec.x;
		this.y = _vec.y;
		this.z = _vec.z;
	}
}
