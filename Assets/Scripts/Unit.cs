using System;
using UnityEngine;

public class Unit : WorldObject {
	public float moveSpeed = 10;
	public float rotationSpeed = 180;

	bool moving;
	Vector3 targetPosition;
	Quaternion targetRotation;

	void Update () {
		if (moving) {
			Move ();
			Rotate ();
		}
	}

	void Move () {
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, moveSpeed * Time.deltaTime);

		if (transform.position == targetPosition) {
			moving = false;
		}
	}

	void Rotate () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	public override void HandleRightClick (Vector3 position, WorldObject hitObject) {
		Debug.Log ("RIGHT CLICK");
		var navMeshAgent = GetComponent<NavMeshAgent> ();
		if (navMeshAgent) {
			navMeshAgent.destination = targetPosition;
		} else {
			moving = true;
			targetPosition = position;
			targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
		}
	}
}

