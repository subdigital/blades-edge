using System;
using UnityEngine;
using System.Collections;

public class Building : WorldObject {

	public Transform spawnPoint;
	public Transform rallyPoint;

	public override void PerformAction(RTS.Action action) {

		if (action.actionType == RTS.ActionType.BuildUnit) {
			Instantiate (action.unitPrefab, spawnPoint.position, spawnPoint.rotation);
		}

	}

	public override void HandleRightClick(Vector3 worldPosition, WorldObject hitObject) {
		rallyPoint.position = worldPosition;
	}

	void Update() {
		if (Selected) {
			Debug.DrawLine (transform.position, rallyPoint.position, Color.red);
		}
	}
}
