using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectSelection : MonoBehaviour {

	public GameObject selectionIndicatorPrefab;
	public LayerMask groundLayer;
	public LayerMask worldObjectsLayer;

	Player player;
	bool dragging;
	Vector2 dragStart;
	Vector2 dragEnd;
	HashSet<WorldObject> selectedObjectsThisFrame;


	void Start () {
		player = GetComponent<Player> ();
		selectedObjectsThisFrame = new HashSet<WorldObject> ();
	}

	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {

			if (player.hud.IsMouseInHUDBounds ()) {
				return;
			}

			WorldObject wob = FindHitObject();
			if (wob) {
				if (!wob.Selected) {
					SelectObject (wob);
				}
			} else if (HitGround()) {
				ClearSelection ();
				dragging = true;
				dragStart = Input.mousePosition;
			}
		}

		if (dragging) {
			dragEnd = Input.mousePosition;

			//DragSelect (dragStart, dragEnd);
			DragSelectBoxCast(dragStart, dragEnd);
		}

		if (Input.GetMouseButtonUp (0)) {
			dragging = false;
			//Destroy (_selectionCube);

			// select things under selection box
		}
	}

	// ()()()()()()()()
	// ----------------
	// [][][][][][][][]

	static GameObject _dragStartMarker;
	static GameObject _dragEndMarker;
	static GameObject _dragCenterMarker;
	GameObject DebugSphere(Vector3 center, Color color) {
		var sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		sphere.transform.position = center;
		sphere.transform.rotation = Quaternion.LookRotation (Camera.main.transform.forward);
		sphere.GetComponent<MeshRenderer> ().material.color = color;
		sphere.transform.localScale = Vector3.one * 4f;

		return sphere;
	}

	static GameObject _boxCastVisual;
	void DragSelectBoxCast(Vector2 screenPos1, Vector2 screenPos2) {
		Vector3 wp1 = Camera.main.ScreenToWorldPoint (screenPos1);
//		Vector3 wp2 = Camera.main.ScreenToWorldPoint (screenPos2);
//		Vector3 wp3 = Camera.main.ScreenToWorldPoint (new Vector2(screenPos1.x, screenPos2.y));
//		Vector3 wp4 = Camera.main.ScreenToWorldPoint (new Vector2(screenPos2.x, screenPos1.y));
		Vector3 wpcenter = Camera.main.ScreenToWorldPoint (
			new Vector2(screenPos1.x + (screenPos2.x - screenPos1.x) / 2f,
				screenPos1.y + (screenPos2.y - screenPos1.y) / 2f)
		);
				
		Vector3 extents = new Vector3(wpcenter.x - wp1.x, wp1.y - wpcenter.y, 50);

		Vector3 direction = Camera.main.transform.forward;
		Quaternion rotation = Quaternion.LookRotation (direction);
		RaycastHit[] hits = Physics.BoxCastAll (wpcenter, extents / 2f, direction, rotation, Mathf.Infinity);

		Debug.Log ("hits: " + hits.Length);

		// keep track of selected objects this so we can remove the ones that are no longer selected
		selectedObjectsThisFrame.Clear();

		//var noLongerSelected = new HashSet<WorldObject> (player.SelectedObjects);
		foreach (var hit in hits) {
			var wob = hit.transform.root.GetComponentInChildren<WorldObject> ();
			// noLongerSelected.Remove (wob);
			selectedObjectsThisFrame.Add(wob);

			if (wob) {
				AddToSelection (wob);
			}
		}

		var noLongerSelected = player.SelectedObjects.Where (x => !selectedObjectsThisFrame.Contains (x));

		//anything leftover is no longer selected
		foreach (var wob in noLongerSelected) {
			UnselectObject (wob);
		}
	}
		
	static GameObject _selectionMesh;
	void DragSelect(Vector2 screenPos1, Vector2 screenPos2) {
		// ray cast this to the ground so we can create a 3d collision cube in world space
		Ray ray1 = Camera.main.ScreenPointToRay(screenPos1);
		Ray ray2 = Camera.main.ScreenPointToRay (screenPos2);
		Ray ray3 = Camera.main.ScreenPointToRay (new Vector2 (screenPos1.x, screenPos2.y));
		Ray ray4 = Camera.main.ScreenPointToRay (new Vector2 (screenPos2.x, screenPos1.y));

		RaycastHit hit1, hit2, hit3, hit4;
		if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, groundLayer) && 
			Physics.Raycast (ray2, out hit2, Mathf.Infinity, groundLayer) &&
			Physics.Raycast (ray3, out hit3, Mathf.Infinity, groundLayer) && 
			Physics.Raycast (ray4, out hit4, Mathf.Infinity, groundLayer)
		) {
			Vector3 worldPos1 = hit1.point;
			Vector3 worldPos2 = hit2.point;
			Vector3 worldPos3 = hit3.point;
			Vector3 worldPos4 = hit4.point;

			// if we dragged backwards, need to flip the points so the mesh gets created facing the right way
			if (worldPos1.x > worldPos2.x || worldPos1.z > worldPos2.z) {
				Vector3 tmp = worldPos1;
				worldPos1 = worldPos2;
				worldPos2 = tmp;

				tmp = worldPos3;
				worldPos3 = worldPos4;
				worldPos4 = tmp;
			}

			if (_selectionMesh == null) {
				_selectionMesh = CreateSelectionMeshObject ();
			}
			UpdateSelectionMesh (_selectionMesh, worldPos1, worldPos2, worldPos3, worldPos4);

		} else {
			Debug.LogWarning ("No raycast hit?");
		}
	}
		
	GameObject CreateSelectionMeshObject() {
		var selectionObject = new GameObject ();
		selectionObject.name = "SelectionTrap";

		var collider = selectionObject.AddComponent<BoxCollider> ();
		collider.tag = "SelectionBox";
		collider.isTrigger = true;

		var meshFilter = selectionObject.AddComponent<MeshFilter> ();
		meshFilter.sharedMesh = new Mesh ();

		var meshRenderer = selectionObject.AddComponent<MeshRenderer> ();

		meshRenderer.material.color = Color.blue;

		return selectionObject;
	}

	void UpdateSelectionMesh(GameObject selectionObject, Vector3 worldPos1, Vector3 worldPos2, Vector3 worldPos3, Vector3 worldPos4) {
		var mf = selectionObject.GetComponent<MeshFilter> ();
		var mesh = mf.sharedMesh;

		// 1......4
		// ........
		// ........
		// 3......2

		var vertices = new Vector3[] {
			worldPos1,
			worldPos4,
			worldPos3,
			worldPos2
		};
		var triangles = new int[] { 0, 1, 2, 2, 1, 3 };
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		// FIXME: positioned in the wrong spot
		var collider = selectionObject.GetComponent<BoxCollider> ();
		collider.center = new Vector3 (worldPos2.x - worldPos1.x, 0, worldPos2.z - worldPos1.z);
		collider.size = new Vector3 (worldPos2.x - worldPos1.x, 10, worldPos2.z - worldPos1.z);

		Vector3 pos = Vector3.zero;
		pos.y = 0.2f;
		selectionObject.transform.position = pos;
	}

	void SelectObject(WorldObject wob) {

		if (player.SelectedObjects.Contains (wob)) {
			return;
		}

		player.SelectObject (wob, true);

		ClearSelectionRings ();
		AddSelectionRing (wob);
	}

	void UnselectObject(WorldObject wob) {
		player.UnselectObject(wob);
		RemoveSelectionRing (wob);
	}

	void AddToSelection(WorldObject wob) {
		if (player.SelectedObjects.Contains (wob)) {
			return;
		}

		player.SelectObject (wob, false);
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
		if (wob.GetComponentInChildren<SelectionRing> ()) {
			return;
		}

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
		ClearSelectionRings ();
		player.ClearSelection ();
	}

	void RemoveSelectionRing(WorldObject wob) {
		var selectionRing = wob.GetComponentInChildren<SelectionRing> ();
		if (selectionRing) {
			Destroy (selectionRing.gameObject);
		}
	}

	void ClearSelectionRings() {
		foreach (WorldObject wob in player.SelectedObjects) {
			RemoveSelectionRing (wob);
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
