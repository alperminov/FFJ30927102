using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
	
	protected bool onGround { get; set; }
	protected Rigidbody2D rigidbody;
	public float moveSpeed;
	public float maxSpeed;
	public float jumpForceY;
	public float jumpForceX;

	// Use this for initialization
	void Start () {
		onGround = false;
	}

	// Update is called once per frame
	void Update () {

	}

	protected void Jump(float moveDirection) {
		//rigidbody = GetComponent<Rigidbody2D> ();
		onGround = false;
		rigidbody.AddForce (new Vector2(moveDirection * jumpForceX * Time.deltaTime, jumpForceY * Time.deltaTime), ForceMode2D.Impulse);
	}

	protected void Move(float moveDirection) {
		transform.position += Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
	}
		
	protected void OnCollisionEnter2D (Collision2D collider) {
		onGround = true;
		//Application.LoadLevel ("sc1");
	}
}

