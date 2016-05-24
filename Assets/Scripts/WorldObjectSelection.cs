using UnityEngine;
using System.Collections;

public class WorldObjectSelection : MonoBehaviour {

	Player player;

	void Start () {
		player = GetComponent<Player> ();
	}

	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {
			WorldObject wob = FindHitObject();
			if (wob) {
				Debug.Log ("Hit object: " + wob);
				if (player.SelectedObject != wob) {
					player.SelectObject (wob);
				}
			} else if (HitGround()) {
				player.SelectObject(null);
			} else {
				Debug.Log ("Nada");
			}

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
