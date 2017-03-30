using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Use this for initialization
	void Start () {
		this.rigidbody = GetComponent<Rigidbody2D> ();
		this.moveSpeed = 5f;
     	this.maxSpeed = 5f;
		this.jumpForceY = moveSpeed * rigidbody.mass * 100f;
		this.jumpForceX = moveSpeed * rigidbody.mass * 80f;
		this.rigidbody.centerOfMass = new Vector2(0f, -2f);
	}

	// Update is called once per frame
	void Update () {
		
		float moveDirection = Input.GetAxis("Horizontal");
		if (onGround) {
			if (Input.GetButtonDown ("Jump")) {
				Jump (moveDirection);
			} else if (Input.GetButton ("Horizontal")) {
				Move (moveDirection);
			} 
		}
	}
}
