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
			
		_selectedObjects.Add (wob);

		wob.SetSelected (true);
		UpdateHUDSelection ();
	}

	public void UnselectObject(WorldObject wob) {
		wob.SetSelected (false);
		SelectedObjects.Remove (wob);

		UpdateHUDSelection ();
	}

	public void ClearSelection() {
		foreach (var wob in SelectedObjects) {
			wob.SetSelected (false);
		}
		SelectedObjects.Clear ();

		if (hud) {
			hud.ClearSelection();
		}
	}

	void UpdateHUDSelection() {
		if (!hud) {
			return;
		}
		WorldObject[] wobs = new WorldObject [_selectedObjects.Count];
		SelectedObjects.CopyTo (wobs);
		hud.SetSelection(wobs);
	}
}
