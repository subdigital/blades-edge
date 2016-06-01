using System;
using UnityEngine;

public class Unit : WorldObject {
	public float moveSpeed = 10;
	public float rotationSpeed = 180;
	public GameObject pathVisualPrefab;

	bool moving;
	Vector3 targetPosition;
	Quaternion targetRotation;

	GameObject pathVisual;
	GameObject targetVisual;

	void Update () {
		if (moving) {
			Move ();
			Rotate ();

			if (Selected) {
				UpdatePathVisual ();
			}
		}
	}

	public override void SetSelected(bool selected) {
		base.SetSelected (selected);

		if (selected) {
			if (moving) {
				AddPathVisual ();
			}
		} else {
			RemovePathVisual ();
		}
	}

	void Move () {
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, moveSpeed * Time.deltaTime);

		if (transform.position == targetPosition) {
			moving = false;
			RemovePathVisual ();
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


			AddPathVisual ();
		}
	}

	void AddPathVisual() {
		if (pathVisual) {
			RemovePathVisual ();
		}

		targetVisual = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		targetVisual.GetComponent<MeshRenderer> ().material.color = Color.blue;
		targetVisual.transform.position = targetPosition;
		targetVisual.transform.localScale = Vector3.one * 4;
		targetVisual.GetComponent<Collider> ().enabled = false;

		//pathVisual = (GameObject)Instantiate (pathVisualPrefab);
		pathVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pathVisual.GetComponent<MeshRenderer> ().sharedMaterial.color = Color.red;
		pathVisual.transform.SetParent (transform);
		Vector3 pos = transform.position;
		pos.y = 0.1f;
		pathVisual.transform.position = pos;
		pathVisual.transform.rotation = transform.rotation * Quaternion.Euler(0, 90f, 0);
	}

	void UpdatePathVisual() {
		pathVisual.transform.rotation = transform.rotation * Quaternion.Euler(0, 90f, 0);

		Vector3 direction = targetPosition - transform.position;
		float distance = direction.magnitude;
		pathVisual.transform.position = transform.position + (targetPosition-transform.position)/2f;
		pathVisual.transform.localScale = new Vector3 (distance / 6, 1, 1);
	}

	void RemovePathVisual() {
		if (pathVisual) {
			Destroy (pathVisual);
			pathVisual = null;
		}

		if (targetVisual) {
			Destroy (targetVisual);
			targetVisual = null;
		}
	}
}

