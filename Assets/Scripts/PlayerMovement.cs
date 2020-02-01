using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

	// This is a reference to the Rigidbody component called "rb"
	public Rigidbody2D rb;
	
	public float forwardForce = 10f;	// Variable that determines the forward force
	public float upForceMoving = 10f;  // Variable that determines the sideways force
	public float upForceJumping = 50f;
	private Vector2 force;

	void Start()
	{
	}
	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate ()
	{
		// Add a forward force		

		if (Input.GetAxisRaw("Horizontal") == 1)	// If the player is pressing the "d" key
		{
			MoveForward();
		}

		if (Input.GetAxisRaw("Horizontal") == -1)  // If the player is pressing the "a" key
		{
			MoveBackWards();
		}

		if (Input.GetAxisRaw("Vertical") == 1)  // If the player is pressing the "a" key
		{
			Jump();
		}
	}
	void MoveForward()
	{
		force.Set(forwardForce, upForceMoving);
		rb.AddForce(force);
	}

	void MoveBackWards()
	{
		force.Set(-forwardForce, upForceMoving);
		rb.AddForce(force);
	}

	void Jump()
	{
		if (rb.transform.position.y <1)
		{
			force.Set(0, upForceJumping);
			rb.AddForce(force);
		}
	}
}

