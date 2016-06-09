using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PositionSphere : MonoBehaviour {


	public Transform target;
	public float angleFromCube;
	public float distanceFromCube;

	public bool autoAngle = true;
	public float orbitSpeed = 10;

	void Start () {
	
	}

	void Update () {

		if (autoAngle) {
			angleFromCube += Time.deltaTime * orbitSpeed;
			if (angleFromCube > 360) {
				angleFromCube -= 360;
			}
		}


		Quaternion rotation = Quaternion.Euler (0, angleFromCube, 0);

		Vector3 direction = rotation * Vector3.back;
		transform.position = target.position + (direction * distanceFromCube);


	}
}
