using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {

	bool inventoryIsActive;
	Canvas canvas;
	// Use this for initialization
	void Start () {
		canvas = GetComponent<Canvas> ();
		canvas.enabled = false;
		inventoryIsActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("i")) {
			if (inventoryIsActive) {
				canvas.enabled = false;
				inventoryIsActive = false;
			}
			else {
				canvas.enabled = true;
				inventoryIsActive = true;
			}
		}
	}
}
