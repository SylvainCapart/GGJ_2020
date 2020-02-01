using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

	// This is a reference to the Rigidbody component called "rb"
	public Rigidbody2D rb;
	
	public Rigidbody rb1;
	

	public float forwardForce = 10f;	// Variable that determines the forward force
	public float upForce = 10f;  // Variable that determines the sideways force
	private Vector2 force;

	void Start()
	{
		force.Set(forwardForce, upForce);
	}
	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate ()
	{
		// Add a forward force		

		if (Input.GetKey("d"))	// If the player is pressing the "d" key
		{
			force.Set(forwardForce, upForce);
			rb.AddForce(force);
		}

		if (Input.GetKey("a"))  // If the player is pressing the "a" key
		{
			force.Set(-forwardForce, upForce);
			rb.AddForce(force);
			// Add a force to the left
			//rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0);
		}
	}
}
