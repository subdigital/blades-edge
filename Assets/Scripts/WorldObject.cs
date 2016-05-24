using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldObject : MonoBehaviour
{
	Player owner;
	Canvas canvas;
	Slider healthSlider;

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

	void Update() {
		
	}

	public void SetSelected(bool selected) {
		if (canvas) {
			canvas.enabled = selected;
		}
	}
}


