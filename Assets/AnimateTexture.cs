using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AnimateTexture : MonoBehaviour {

	public Vector2 textureScale = new Vector2(1, 1);
	public Vector2 speed = new Vector2(0.001f, 0);

	public float framesPerSecond = 30f;

	void Start () {
		StartCoroutine (Animate ());
	}

	IEnumerator Animate() {
		while (true) {
			var renderer = GetComponent<MeshRenderer> ();
			var mat = renderer.sharedMaterial;
			Vector2 scale = new Vector2 (
				                transform.localScale.x * textureScale.x,
				                transform.localScale.z * textureScale.y
			                );
			mat.SetTextureScale ("_MainTex", scale);

			Vector2 offset = speed + mat.GetTextureOffset ("_MainTex");

			mat.SetTextureOffset ("_MainTex", offset);

			yield return new WaitForSeconds (1f/framesPerSecond);
		}
	}

	void Update () {
		
	}
}
