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
}
