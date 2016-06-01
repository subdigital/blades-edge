using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldObject : MonoBehaviour
{
	Player owner;
	Canvas canvas;
	Slider healthSlider;

	public RTS.Action[] actions;

	void Awake() {
		canvas = GetComponentInChildren<Canvas> ();
		if (canvas) {
			canvas.enabled = false;
			var text = canvas.GetComponentInChildren<Text> ();
			if (text) {
				text.text = name;
			}

			healthSlider = canvas.GetComponentInChildren<Slider> ();
			healthSlider.value = 100f;
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("collider: " + other.tag);
	}

	public bool Selected { get; private set; }

	public virtual void SetSelected(bool selected) {
		Selected = selected;
		if (canvas) {
			canvas.enabled = selected;
		}
	}

	public virtual void HandleRightClick(Vector3 worldPosition, WorldObject hitObject) {
	}

	public virtual void PerformAction(RTS.Action action) {
	}
}

