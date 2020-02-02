using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

	// This is a reference to the Rigidbody component called "rb"

	public Vector2 movement;

	void Start()
	{
	}
	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	
	void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical;
		bool verticalMove = Input.GetKeyDown(KeyCode.Space) ;
		if (verticalMove)
		{
			vertical = 1;
		}
		else
		{
			vertical = 0;
		}
		movement.Set(horizontal, vertical);		
	}
	// void FixedUpdate ()
	// {
	// 	// Add a forward force		

	// 	if (Input.GetAxisRaw("Horizontal") == 1)	// If the player is pressing the "d" key
	// 	{
	// 		MoveForward();
	// 	}

	// 	if (Input.GetAxisRaw("Horizontal") == -1)  // If the player is pressing the "a" key
	// 	{
	// 		MoveBackWards();
	// 	}

	// 	if (Input.GetAxisRaw("Vertical") == 1)  // If the player is pressing the "a" key
	// 	{
	// 		Jump();
	// 	}
	// }
	// void MoveForward()
	// {
	// 	force.Set(forwardForce, upForceMoving);
	// 	rb.AddForce(force);
	// }

	// void MoveBackWards()
	// {
	// 	force.Set(-forwardForce, upForceMoving);
	// 	rb.AddForce(force);
	// }

	// void Jump()
	// {
	// 	if (rb.transform.position.y <1)
	// 	{
	// 		force.Set(0, upForceJumping);
	// 		rb.AddForce(force);
	// 	}
	// }
}

