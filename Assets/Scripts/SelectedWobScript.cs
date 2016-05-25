using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedWobScript : MonoBehaviour {
	public WorldObject Wob;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetWorldObject(WorldObject wob) {
		this.Wob = wob;

		var text = this.GetComponentInChildren<Text> ();
		text.text = wob.name;

		var slider = this.GetComponentInChildren<Slider> ();
		slider.value = 50;
	}
}
