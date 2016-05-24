using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

	public Component minimap;
	public RectTransform selectionPanel;
	public RectTransform actionsPanel;

	public GameObject actionButtonPrefab;
	public GameObject selectedWobPrefab;

	WorldObject[] selection;
	List<GameObject> actionButtons;
	List<GameObject> selectedWobObjects;

	void Start () {
		selectedWobObjects = new List<GameObject> ();
		ClearSelection ();
	}

	void Update () {
	}

	public void SetSelection(WorldObject[] wobs) {
		Debug.Assert (wobs != null);
		selection = wobs;

		if (wobs.Length == 0) {
//			selectionNameText.text = string.Empty;
		} else if (wobs.Length == 1) {
			ConfigureHUDForWorldObject (wobs [0]);
			AddSelectedWob (wobs [0]);
		} else {
			// if all units are the same type, we can show the single selection UI
		}
	}

	void AddSelectedWob(WorldObject wob) {
		var selectedWobObj = (GameObject)Instantiate (selectedWobPrefab);
		var script = selectedWobObj.GetComponent<SelectedWobScript> ();
		script.SetWorldObject (wob);

		selectedWobObj.transform.SetParent (selectionPanel, false);
		selectedWobObj.transform.localScale = new Vector3 (1, 1, 1);
		selectedWobObjects.Add (selectedWobObj);
	}

	public void ClearSelection() {
		if (actionButtons != null) {
			foreach (var ab  in actionButtons) {
				Destroy(ab);
			}
			actionButtons.Clear ();
		}
//		foreach (var gameObject in selectedWobObjects) {
//			Destroy (gameObject);
//		}
//		selectedWobObjects.Clear ();
	}

	public bool IsMouseInHUDBounds() {
		Vector3[] corners = new Vector3[4];
		actionsPanel.GetLocalCorners (corners);
		float yMax = 0;
		foreach (Vector3 corner in corners) {
			float offsetY = corner.y + actionsPanel.anchoredPosition.y;
			yMax = Mathf.Max (yMax, offsetY);
		}

		Vector3 mpos = Input.mousePosition;
		if (mpos.y <= yMax) {
			return true;
		}

		return false;
	}

	void ConfigureHUDForWorldObject(WorldObject wob) {
		AddActions (wob);
	}

	void AddActions(WorldObject wob) {
		List<GameObject> buttons = new List<GameObject> (wob.actions.Length);
		foreach (var action in wob.actions) {
			var buttonObj = (GameObject)Instantiate (actionButtonPrefab);
			buttons.Add (buttonObj);
			var button = buttonObj.GetComponent<Button> ();
			button.onClick.AddListener (delegate() {
				HandleAction(action);	
			});

			buttonObj.GetComponentInChildren<Text> ().text = action.name;

			buttonObj.transform.SetParent (actionsPanel, false);
			buttonObj.transform.localScale = new Vector3 (1, 1, 1);
		}

		actionButtons = buttons;
	}

	public void HandleAction(RTS.Action action) {
		WorldObject wob = selection[0];
		wob.PerformAction (action);
	}
}
