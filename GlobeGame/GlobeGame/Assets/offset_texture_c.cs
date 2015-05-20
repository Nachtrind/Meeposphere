using UnityEngine;
using System.Collections;

public class offset_texture_c : MonoBehaviour {
	private Material Lava_Move ;
	
	// Use this for initialization
	void Start () {
		Lava_Move = GetComponent<Renderer> ().sharedMaterial;
		
	}
	public float speed = 0.02f;

	// Update is called once per frame
	void Update () {

		float offset = Time.time * speed ;

		Lava_Move.SetTextureOffset("_DetailAlbedoMap", new Vector2(offset,0));
		
		
	}
}
 