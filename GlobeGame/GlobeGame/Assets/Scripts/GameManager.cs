using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public List<
	
	public static GameManager Instance {
		get { return instance ?? (instance = new GameObject ("GameManager").AddComponent<GameManager> ()); }
	}

}
