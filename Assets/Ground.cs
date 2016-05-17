using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// transform.renderer.material.mainTextureScale = new Vector2(XScale , YScale 

		var meshRenderer = GetComponent<MeshRenderer>();

		meshRenderer.sharedMaterial.mainTextureScale = new Vector2 (20, 20);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
