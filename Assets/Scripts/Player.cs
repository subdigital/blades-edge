using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public HUD hud;

	private HashSet<WorldObject> _selectedObjects;
	public HashSet<WorldObject> SelectedObjects { get { return _selectedObjects; } }

	void Start () {
		hud = FindObjectOfType<HUD> ();
		_selectedObjects = new HashSet<WorldObject> ();
	}

	void Update () {
	
	}
		
	public void SelectObject(WorldObject wob, bool replace = false) {
		Debug.Assert (wob);
		if (replace) {
			ClearSelection ();
		}

		WorldObject[] wobs = new WorldObject [_selectedObjects.Count];
		SelectedObjects.CopyTo (wobs);
		hud.SetSelection(wobs);
		_selectedObjects.Add (wob);

		wob.SetSelected (true);
	}

	public void UnselectObject(WorldObject wob) {
		wob.SetSelected (false);
		SelectedObjects.Remove (wob);
	}

	public void ClearSelection() {
		foreach (var wob in SelectedObjects) {
			wob.SetSelected (false);
		}
		SelectedObjects.Clear ();

		hud.ClearSelection();
	}

}
