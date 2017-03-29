using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Use this for initialization
	public Vector2 com;
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		rigidbody.centerOfMass = com;
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
