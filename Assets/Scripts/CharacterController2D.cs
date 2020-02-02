using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	//[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	//[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	public float k_GroundedRadius = 2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public PlayerMovement playerMovement;

	public Vector2 targetVelocity;

	public float maxVelocity;
	public GameObject respawnPoint; 
	private Vector2 currentVelocity;
    public PartsManager m_PartsManager;

    public float m_DeathYPos;

	private bool called;
	

	public DurabilityManager durabilityManager;
	
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
		m_Grounded = true;
	}

    private void FixedUpdate()
    {
		bool wasGrounded = m_Grounded;
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
        }
		
		if (playerMovement.movement.y > 0)
		{
			Jump();		
		}
		if (playerMovement.movement.x != 0)
		{
			Move();		
		}
		CheckFall();
		CheckDurability();
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

	public void CheckFall()
	{
        if(m_Rigidbody2D.position.y < -1)
		{
			Respawn();
		}
	}

	public void CheckDurability()
	{
		if (durabilityManager.getDurability() <= 0)
		{
			Respawn();
		}
	}

	public void Respawn()
	{
		Vector2 newPosition = new Vector2(respawnPoint.transform.position.x, respawnPoint.transform.position.y);
		transform.position = newPosition;
		m_Rigidbody2D.velocity = Vector2.zero;
		
		durabilityManager.resetDurability();
	}
	public void Jump()
	{
		if (m_Grounded)
		{
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			m_Grounded = false;
		}	
	}
	public void ResetJump()
	{
		m_Grounded = true;
	}
	public void Move()
	{
		if (!m_PartsManager.m_Attaching)
		{
			Vector2 velocityDenormalized = new Vector2(targetVelocity.x * playerMovement.movement.x * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);
        	m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, velocityDenormalized, ref currentVelocity, m_MovementSmoothing, maxVelocity * Time.deltaTime);
			durabilityManager.decrementDurability();
		}

		// // If crouching, check to see if the character can stand up
		// if (!crouch)
		// {
		// 	// If the character has a ceiling preventing them from standing up, keep them crouching
		// 	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		// 	{
		// 		crouch = true;
		// 	}
		// }

		
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

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Ground")
		{
			Debug.Log("ResetJump called from OncollisionEnter2D");
			ResetJump();
		}
	}

 }
