using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmHit : MonoBehaviour
{

    
    private float oldRotation;
    public float m_angle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (tag != "Player")
        {
            //
        }

        if ( tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetComponent<Animator>().SetBool("Hit", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("Hit", false);
            }
  
        }



        


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
