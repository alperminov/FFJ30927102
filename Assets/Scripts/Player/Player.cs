using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		

		if (Input.GetButton ("Horizontal")) {

			Move ();

		} else if (Input.GetButtonUp ("Horizontal")) {
			Stop ();
		} 
		if (Input.GetButtonDown ("Jump") && onGround) {
				Jump ();
		} 
	}
}
