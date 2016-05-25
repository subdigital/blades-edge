using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class WorldObjectSelection : MonoBehaviour {

	public GameObject selectionIndicatorPrefab;

	Player player;
	bool dragging;
	Vector2 dragStart;
	Vector2 dragEnd;


	void Start () {
		player = GetComponent<Player> ();
	}

	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {
			if (EventSystem.current.IsPointerOverGameObject()) {
				return;
			}

			WorldObject wob = FindHitObject();
			if (wob) {
				SelectObject (wob);
			} else if (HitGround()) {
				ClearSelection ();
			}

			dragging = true;
			dragStart = Input.mousePosition;
		}

		if (dragging) {
			dragEnd = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			dragging = false;

			// select things under selection box
		}
	}

	void SelectObject(WorldObject wob) {
		if (player.SelectedObject && player.SelectedObject != wob) {
			ClearSelection ();
		}

		player.SelectObject (wob);
		AddSelectionRing (wob);
	}

	void OnGUI() {
		if (dragging) {
			var rect = Utils.ScreenDrawing.GetScreenRect (dragStart, dragEnd);
			Utils.ScreenDrawing.DrawRect (rect, new Color (0.7f, 1.0f, 0.7f, 0.3f));
			Utils.ScreenDrawing.DrawBox (rect, 3, new Color (0.8f, 1.0f, 0.8f, 0.5f));
		}
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
