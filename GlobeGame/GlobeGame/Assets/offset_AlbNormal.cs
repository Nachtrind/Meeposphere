using UnityEngine;
using System.Collections;

public class offset_AlbNormal : MonoBehaviour {
	private Material Lava_Move ;
	
	// Use this for initialization
	void Start () {
		Lava_Move = GetComponent<Renderer> ().sharedMaterial;
		
	}
	public float speed = 0.02f;
	public float speed2 = 0.06f;
	// Update is called once per frame
	void Update () {
		float offset2 = Time.time * speed2 ;
		float offset = Time.time * speed ;
		Lava_Move.SetTextureOffset("_MainTex", new Vector2(offset2,offset2));
		Lava_Move.SetTextureOffset("_DetailAlbedoMap", new Vector2(offset,offset));
		
		
	}
}
