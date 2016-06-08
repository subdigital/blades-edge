using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float cameraMoveSpeed = 50f;
	public float screenEdgeBuffer = 15;

	private GameObject _focusObject;
	public GameObject FocusObject {
		set {
			_focusObject = value;
			FocusOn (_focusObject);
		}
		get {
			return _focusObject;
		}
	}

	Camera viewCamera;

	void Start () {
		viewCamera = GetComponentInChildren<Camera> ();
	}

	void Update () {
		HandleMouse ();
		HandleKeyboard ();
	}

	void HandleMouse() {
		float mx = Input.mousePosition.x;
		float my = Input.mousePosition.y;
		if (mx >= -screenEdgeBuffer && mx <= screenEdgeBuffer) {
			MoveCamera (Vector3.left);
		} else if (mx >= Screen.width - screenEdgeBuffer && mx <= Screen.width + screenEdgeBuffer) {
			MoveCamera (Vector3.right);
		}

		if (my >= -screenEdgeBuffer && my <= screenEdgeBuffer) {
			MoveCamera (Vector3.back);
		} else if (my >= Screen.height - screenEdgeBuffer && my <= Screen.height + screenEdgeBuffer) {
			MoveCamera (Vector3.forward);
		}
	}

	void HandleKeyboard() {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			MoveCamera (Vector3.left);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			MoveCamera (Vector3.right);
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			MoveCamera (Vector3.forward);
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			MoveCamera (Vector3.back);
		}
	}

	void MoveCamera(Vector3 direction) {
		Vector3 cameraDirection = viewCamera.transform.TransformDirection (direction);
		Vector3 newPosition = viewCamera.transform.position + cameraDirection * cameraMoveSpeed * Time.deltaTime;
		newPosition.y = viewCamera.transform.position.y;
		viewCamera.transform.position = newPosition;
	}

	void FocusOn(GameObject focusObject) {
		var cameraRig = transform;
		var camera = cameraRig.GetComponentInChildren<Camera> ();
		var cameraAngle = camera.transform.eulerAngles.x;

		var height = camera.transform.position.y;
		var distanceBack = Mathf.Tan (Mathf.Deg2Rad * cameraAngle) * height;

		Vector3 pos = cameraRig.transform.position;
		pos.x = focusObject.transform.position.x;
		pos.z = focusObject.transform.position.z - distanceBack;
		Debug.Log ("angle:" + cameraAngle);
		Debug.Log ("angle2:" + camera.transform.eulerAngles.x);
		Debug.Log ("camera height: " + height);
		Debug.Log ("camera distance back: " + distanceBack);
		cameraRig.transform.position = pos;
	}
}
