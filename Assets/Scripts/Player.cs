using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private WorldObject _selectedObject;
	public WorldObject SelectedObject { get { return _selectedObject; } }

	public void SelectObject(WorldObject wob) {
		if (_selectedObject) {
			_selectedObject.SetSelected (false);
		}
			
		_selectedObject = wob;
		if (_selectedObject) {
			wob.SetSelected (true);
		}
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
