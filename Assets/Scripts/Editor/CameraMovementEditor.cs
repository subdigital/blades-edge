using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraMovement))]
public class CameraMovementEditor : Editor
{
	private GameObject _focusObject;

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		var cameraMovement = (CameraMovement)target;

		_focusObject = (GameObject)EditorGUILayout.ObjectField ("Focus on:", _focusObject, typeof(GameObject), true);
		if (_focusObject) {
			cameraMovement.FocusObject = _focusObject;
		}

		if (GUILayout.Button("Move")) {
			var baseGameObject = GameObject.Find ("Base");
			cameraMovement.FocusObject = baseGameObject;
		}

		if (GUILayout.Button ("Reset Position")) {
			cameraMovement.gameObject.transform.position = new Vector3 (0, 60, -60);
		}
	}
	
}