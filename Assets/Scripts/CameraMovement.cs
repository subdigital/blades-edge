using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float cameraMoveSpeed = 50f;
	public float screenEdgeBuffer = 15;

	private GameObject _focusObject;
	public GameObject FocusObject {
		set {
			if (_focusObject != value) {
				_focusObject = value;
				FocusOn (_focusObject);
			}
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
		Debug.Log ("Focus object position: " + focusObject.transform.position);

		var cameraRig = transform;
		var camera = cameraRig.GetComponentInChildren<Camera> ();
		var cameraAngleX = camera.transform.eulerAngles.x;
		var cameraRigAngleY = cameraRig.eulerAngles.y;

		var height = camera.transform.position.y;
		var distanceBack = Mathf.Tan (Mathf.Deg2Rad * cameraAngleX) * height;

		var rotation = Quaternion.AngleAxis (cameraRigAngleY, Vector3.up);
		var direction = rotation * Vector3.back;

		Vector3 pos = cameraRig.transform.position;

		Vector3 newPos = focusObject.transform.position + direction * distanceBack;
		newPos.y = pos.y;

		Debug.Log ("angle:" + cameraAngleX);
		Debug.Log ("camera height: " + height);
		Debug.Log ("camera distance back: " + distanceBack);
		Debug.Log ("New position: " + newPos);

		cameraRig.transform.position = newPos;
	}
}
