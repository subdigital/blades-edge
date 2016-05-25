using UnityEngine;
using System.Collections;

public class WorldObjectSelection : MonoBehaviour {

	public GameObject selectionIndicatorPrefab;

	Player player;

	void Start () {
		player = GetComponent<Player> ();
	}

	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {

			if (player.hud.IsMouseInHUDBounds ()) {
				return;
			}

			WorldObject wob = FindHitObject();
			if (wob) {
				SelectObject (wob);
			} else if (HitGround()) {
				ClearSelection ();
			} else {
				Debug.Log ("Nada");
			}
		}
	}

	void SelectObject(WorldObject wob) {
		if (player.SelectedObject && player.SelectedObject != wob) {
			ClearSelection ();
		}

		player.SelectObject (wob);
		AddSelectionRing (wob);
	}

	void AddSelectionRing(WorldObject wob) {

		MeshRenderer[] renderers = wob.GetComponentsInChildren<MeshRenderer> ();
		if (renderers.Length == 0) {
			Debug.LogWarning (wob.name + " has no mesh renderers");
			return;
		}
		Bounds bounds = renderers [0].bounds;
		for (int i = 1; i < renderers.Length; i++) {
			MeshRenderer r = renderers [i];
			bounds.Encapsulate (r.bounds);
		}

		float maxSize = Mathf.Max (bounds.size.x, bounds.size.z);


		Vector3 objPosition = bounds.center;
		objPosition.y = 0.01f;

		var ring = (GameObject)Instantiate (selectionIndicatorPrefab, objPosition, Quaternion.identity);


		ring.transform.localScale = new Vector3 (maxSize * 1.75f, 1, maxSize * 1.75f);
		ring.transform.SetParent (wob.transform);
	}

	void ClearSelection() {
		WorldObject wob = player.SelectedObject;
		if (wob) {
			Destroy (wob.GetComponentInChildren<SelectionRing> ().gameObject);
			player.ClearSelection ();	
		}
	}

	WorldObject FindHitObject() {
		RaycastHit hitInfo;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {

			Transform rootOfHitObject = hitInfo.collider.gameObject.transform.root;
			WorldObject obj = rootOfHitObject.GetComponentInChildren<WorldObject> ();
			if (obj) {
				return obj;
			} else {
				return null;
			}

		} else {
			return null;
		}
	}

	bool HitGround() { 
		RaycastHit hitInfo;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {

			Transform rootOfHitObject = hitInfo.collider.gameObject.transform.root;
			return rootOfHitObject.gameObject.name == "Ground";
		}

		return false;
	}
}
