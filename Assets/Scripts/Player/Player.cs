using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {


    private const string bedTag = "Bed";
    private const string tableTag = "Table";
    private const string doorTag = "Door";
    private const string lockerTag = "Locker";

	public int inventoryCellCount = 16; 
	public string containerTag;
	public List<Item> itemList;

    // Use this for initialization
    void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		moveSpeed = 5f;
     	maxSpeed = 5f;
		jumpForceY = moveSpeed * rigidbody.mass * 100f;
		jumpForceX = moveSpeed * rigidbody.mass * 80f;
		rigidbody.centerOfMass = new Vector2(0f, -2f);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(bedTag);
         if (other.gameObject.tag == bedTag)
         {
             Debug.Log(bedTag);
         }
         if (other.gameObject.tag == tableTag)
         {
             Debug.Log(tableTag);
         }
         if (other.gameObject.tag == doorTag)
         {
             Debug.Log(doorTag);
         }
         if (other.gameObject.tag == lockerTag)
         {
             Debug.Log(lockerTag);
         }

    }
}
