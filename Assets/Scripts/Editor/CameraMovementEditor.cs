using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraMovement))]
public class CameraMovementEditor : Editor
{
	private GameObject _focusObject;

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		_focusObject = (GameObject)EditorGUILayout.ObjectField ("Focus on:", _focusObject, typeof(GameObject), true);
		if (_focusObject) {
			var cameraMovement = (CameraMovement)target;
			cameraMovement.FocusObject = _focusObject;
		}

		if (GUILayout.Button("Move")) {
			Debug.Log ("CLICKED");
			var baseGameObject = GameObject.Find ("Base");

			// target x/z
		

			var cameraMovement = (CameraMovement)target;
			cameraMovement.FocusObject = baseGameObject;
//			var cameraRig = cameraMovement.transform;
//			var camera = cameraRig.GetComponentInChildren<Camera> ();
//			var cameraAngle = camera.transform.eulerAngles.x;
//
//			var height = camera.transform.position.y;
//			var distanceBack = Mathf.Tan (Mathf.Deg2Rad * cameraAngle) * height;
//
//			Vector3 pos = cameraRig.transform.position;
//			pos.x = baseGameObject.transform.position.x;
//			pos.z = baseGameObject.transform.position.z - distanceBack;
//			Debug.Log ("angle:" + cameraAngle);
//			Debug.Log ("angle2:" + camera.transform.eulerAngles.x);
//			Debug.Log ("camera height: " + height);
//			Debug.Log ("camera distance back: " + distanceBack);
//			cameraRig.transform.position = pos;
		}
	}
	
}