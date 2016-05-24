using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public HUD hud;

	private WorldObject _selectedObject;
	public WorldObject SelectedObject { get { return _selectedObject; } }

	void Start () {
		hud = FindObjectOfType<HUD> ();
	}

	void Update () {
	
	}
		
	public void SelectObject(WorldObject wob) {
		Debug.Assert (wob);
		ClearSelection ();

		hud.SetSelection(new WorldObject[] {wob});
		_selectedObject = wob;
		wob.SetSelected (true);
	}

	public void ClearSelection() {
		if (_selectedObject) {
			_selectedObject.SetSelected (false);
			_selectedObject = null;
		}

		hud.ClearSelection();
	}

}
