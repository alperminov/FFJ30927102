using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
	
	protected bool onGround { get; set; }
	protected Rigidbody2D rigidbody;
	public float moveForce = 1000f;
	public float maxSpeed = 5f;
	public bool facingRight = true;
	public bool jump = false;

	// Use this for initialization
	void Start () {
		onGround = false;
	}

	// Update is called once per frame
	void Update () {

	}

	protected void Jump() {
		onGround = false;
		//rigidbody = GetComponent<Rigidbody2D> ();
		rigidbody.AddForce (new Vector2(0f, 1000f));
	}

	protected void Move() {

		float h = Input.GetAxis("Horizontal");

		if (Mathf.Abs (rigidbody.velocity.x) > maxSpeed)
			rigidbody.velocity = new Vector2(Mathf.Sign (rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
		
		if (rigidbody.velocity.x < maxSpeed)
			rigidbody.AddForce (Vector3.right * h * moveForce * Time.deltaTime);
	}


	protected void Stop() {
		//rigidbody = GetComponent<Rigidbody2D> ();
		//rigidbody.AddForce
	}
		
	protected void OnCollisionEnter2D (Collision2D collider) {
		onGround = true;
		//Application.LoadLevel ("sc1");
	}
}

