using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	//[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	//[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	//[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	//const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public PlayerMovement playerMovement;

	public Vector2 targetVelocity;

	public float maxVelocity;
	private Vector2 currentVelocity;
    public PartsManager m_PartsManager;

	private bool called;


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;
    public float m_rotateForce = 2;


	private void Awake()
	{


		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		called = false;
	}

    private void FixedUpdate()
    {
        bool oneTaken = HasOneSpotTakenAtLeast();
        if (oneTaken)
        {
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            m_Rigidbody2D.constraints = 0;
        }

        if (!m_PartsManager.m_Attaching)
        {
            Vector2 velocityDenormalized = new Vector2(targetVelocity.x * playerMovement.movement.x * Time.fixedDeltaTime, targetVelocity.x * playerMovement.movement.y * Time.fixedDeltaTime);
            m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, velocityDenormalized, ref currentVelocity, m_MovementSmoothing, maxVelocity * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift) )
            {
                //m_Rigidbody2D.rotation += m_rotateForce;
                transform.Rotate(0, 0, m_rotateForce, Space.Self);

                foreach (var spot in m_PartsManager.spots)
                {
                    if (spot.isTaken)
                    {
                        Rigidbody2D rb = spot.tr.transform.GetComponentInChildren<Rigidbody2D>();
                        if (rb != null)
                        {
                            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.rotation += m_rotateForce;
                        }
                    }

                }
            }

            //if (Input.GetKeyUp(KeyCode.LeftShift))
            //{
            //    foreach (var spot in m_PartsManager.spots)
            //    {
            //        if (spot.isTaken)
            //        {
            //            //Rigidbody2D rb = spot.tr.transform.GetComponentInChildren<Rigidbody2D>();
            //            //if (rb != null)
            //            //{
            //            //    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //            //}
            //        }

            //    }

            //}
        }
    }

    public bool HasOneSpotTakenAtLeast()
    {
        bool taken = false;
        foreach (var spot in m_PartsManager.spots)
        {
            if (spot.isTaken)
            {
                taken = true;
            }
        }
        return taken;
    }


	public void Move()
	{
		
		// // If crouching, check to see if the character can stand up
		// if (!crouch)
		// {
		// 	// If the character has a ceiling preventing them from standing up, keep them crouching
		// 	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		// 	{
		// 		crouch = true;
		// 	}
		// }

		// //only control the player if grounded or airControl is turned on
		// if (m_Grounded || m_AirControl)
		// {

		// 	// If crouching
		// 	if (crouch)
		// 	{
		// 		if (!m_wasCrouching)
		// 		{
		// 			m_wasCrouching = true;
		// 			OnCrouchEvent.Invoke(true);
		// 		}

		// 		// Reduce the speed by the crouchSpeed multiplier
		// 		move *= m_CrouchSpeed;

		// 		// Disable one of the colliders when crouching
		// 		if (m_CrouchDisableCollider != null)
		// 			m_CrouchDisableCollider.enabled = false;
		// 	} else
		// 	{
		// 		// Enable the collider when not crouching
		// 		if (m_CrouchDisableCollider != null)
		// 			m_CrouchDisableCollider.enabled = true;

		// 		if (m_wasCrouching)
		// 		{
		// 			m_wasCrouching = false;
		// 			OnCrouchEvent.Invoke(false);
		// 		}
		// 	}

		// 	// Move the character by finding the target velocity
		// 	Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
		// 	// And then smoothing it out and applying it to the character
		// 	m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		// 	// If the input is moving the player right and the player is facing left...
		// 	if (move > 0 && !m_FacingRight)
		// 	{
		// 		// ... flip the player.
		// 		Flip();
		// 	}
		// 	// Otherwise if the input is moving the player left and the player is facing right...
		// 	else if (move < 0 && m_FacingRight)
		// 	{
		// 		// ... flip the player.
		// 		Flip();
		// 	}
		// }
		// // If the player should jump...
		// if (m_Grounded && jump)
		// {
		// 	// Add a vertical force to the player.
		// 	m_Grounded = false;
		// 	m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		// }
	}


	private void Flip()
	{
		// // Switch the way the player is labelled as facing.
		// m_FacingRight = !m_FacingRight;

		// // Multiply the player's x local scale by -1.
		// Vector3 theScale = transform.localScale;
		// theScale.x *= -1;
		// transform.localScale = theScale;
	}
	

}
