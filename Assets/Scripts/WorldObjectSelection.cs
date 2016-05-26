using UnityEngine;
using System.Collections;

public class WorldObjectSelection : MonoBehaviour {

	public GameObject selectionIndicatorPrefab;
	public LayerMask groundLayer;

	Player player;
	bool dragging;
	Vector2 dragStart;
	Vector2 dragEnd;


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
				if (!wob.Selected) {
					SelectObject (wob);
				}
			} else if (HitGround()) {
				ClearSelection ();
			}

			dragging = true;
			dragStart = Input.mousePosition;
		}

		if (dragging) {
			dragEnd = Input.mousePosition;

			DragSelect (dragStart, dragEnd);
		}

		if (Input.GetMouseButtonUp (0)) {
			dragging = false;
			//Destroy (_selectionCube);

			// select things under selection box
		}
	}
		
	static GameObject _trapezoid;
	void DragSelect(Vector2 screenPos1, Vector2 screenPos2) {
		// ray cast this to the ground so we can create a 3d collision cube in world space
		Ray ray1 = Camera.main.ScreenPointToRay(screenPos1);
		Ray ray2 = Camera.main.ScreenPointToRay (screenPos2);
		Ray ray3 = Camera.main.ScreenPointToRay (new Vector2 (screenPos1.x, screenPos2.y));
		Ray ray4 = Camera.main.ScreenPointToRay (new Vector2 (screenPos2.x, screenPos1.y));

		RaycastHit hit1, hit2, hit3, hit4;
		if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, groundLayer) && 
			Physics.Raycast (ray2, out hit2, Mathf.Infinity, groundLayer) &&
			Physics.Raycast(ray3, out hit3, Mathf.Infinity, groundLayer) && 
			Physics.Raycast(ray4, out hit4, Mathf.Infinity, groundLayer)
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

			if (_trapezoid == null) {
				_trapezoid = CreateTrapezoid ();
			}
			UpdateSelectionMesh (_trapezoid, worldPos1, worldPos2, worldPos3, worldPos4);

		} else {
			Debug.LogWarning ("No raycast hit?");
		}
	}
		
	GameObject CreateTrapezoid() {
		var trap = new GameObject ();
		trap.name = "SelectionTrap";

		var collider = trap.AddComponent<BoxCollider> ();
		collider.tag = "SelectionBox";
		collider.isTrigger = true;

		var meshFilter = trap.AddComponent<MeshFilter> ();
		meshFilter.sharedMesh = new Mesh ();

		var meshRenderer = trap.AddComponent<MeshRenderer> ();

		meshRenderer.material.color = Color.blue;

		return trap;
	}

	void UpdateSelectionMesh(GameObject trap, Vector3 worldPos1, Vector3 worldPos2, Vector3 worldPos3, Vector3 worldPos4) {
		
		var mf = trap.GetComponent<MeshFilter> ();
		var mesh = mf.sharedMesh;

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
		var collider = trap.GetComponent<BoxCollider> ();
		collider.center = new Vector3 (worldPos2.x - worldPos1.x, 0, worldPos2.z - worldPos1.z);
		collider.size = new Vector3 (worldPos2.x - worldPos1.x, 10, worldPos2.z - worldPos1.z);

		Vector3 pos = Vector3.zero;
		pos.y = 0.2f;
		trap.transform.position = pos;
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
