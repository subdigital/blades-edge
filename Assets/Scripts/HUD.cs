using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

	public Component minimap;
	public Component selectionPanel;
	public RectTransform actionsPanel;

	public Text selectionNameText;

	public GameObject actionButtonPrefab;

	WorldObject[] selection;
	List<GameObject> actionButtons;

	void Start () {
		ClearSelection ();
	}

	void Update () {
	}

	public void SetSelection(WorldObject[] wobs) {
		Debug.Assert (wobs != null);
		selection = wobs;

		if (wobs.Length == 0) {
			selectionNameText.text = string.Empty;
		} else if (wobs.Length == 1) {
			ConfigureHUDForWorldObject (wobs [0]);

		} else {
			selectionNameText.text = "Multiple Selection";
			// if all units are the same type, we can show the single selection UI
		}
	}

	public void ClearSelection() {
		selectionNameText.text = string.Empty;
		if (actionButtons != null) {
			foreach (var ab  in actionButtons) {
				Destroy(ab);
			}
			actionButtons.Clear ();
		}
	}

	public bool IsMouseInHUDBounds() {
		Vector3[] corners = new Vector3[4];
		actionsPanel.GetLocalCorners (corners);
		float yMax = 0;

		foreach (Vector3 corner in corners) {

			float offsetY = corner.y + actionsPanel.anchoredPosition.y;
			// Vector3 offsetCorner = actionsPanel.anchorMax
			yMax = Mathf.Max (yMax, offsetY);
		}

		Vector3 mpos = Input.mousePosition;
		if (mpos.y <= yMax) {
			return true;
		}

		return false;
	}

	void ConfigureHUDForWorldObject(WorldObject wob) {
		selectionNameText.text = wob.name;
		AddActions (wob);
	}

	void AddActions(WorldObject wob) {
		List<GameObject> buttons = new List<GameObject> (wob.actions.Length);
//		float x = 0;
//		float y = actionsPanel.;
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
		// for selected units pass action
		// or
		// player

		// for now, only handle single selection
		WorldObject wob = selection[0];
		wob.PerformAction (action);
	}
}
