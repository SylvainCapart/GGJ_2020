using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulse : MonoBehaviour
{
    private float m_bumpActivationDelay = 0.5f;
    private float m_lastBumpActivation = 0.0f;
    public float m_repulsiveForce = 10f;
    private Animator m_Anim;

    // Use this for initialization
    void Start()
    {
        m_Anim = this.GetComponentInParent<Animator>();
        if (m_Anim == null)
            Debug.LogError(this.name + " : Animator not found");

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D incRb;
        Vector2 repulsiveVector;
        Debug.Log("STAY SPRING 1");
        if (collision.transform.tag == "Ground")
        {
            m_Anim.SetBool("BounceActivation", true);
            Debug.Log("STAY SPRING 2");
            if ((Time.time - m_lastBumpActivation) > m_bumpActivationDelay)
            {

                repulsiveVector = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position; // new Vector2(collision.transform.position.x - this.transform.position.x, collision.transform.position.y - this.transform.position.y);

                if (collision.transform.tag == "Ground" && transform.tag == "Player")
                {
                    Debug.Log("STAY SPRING 3");
                    transform.GetComponent<Rigidbody2D>().AddForce(repulsiveVector.normalized * m_repulsiveForce, ForceMode2D.Impulse);

                }

                m_lastBumpActivation = Time.time;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("ENTER SPRING");
        m_Anim.SetBool("BounceActivation", false);
    }
}
