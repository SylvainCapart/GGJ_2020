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
            if (Input.GetKey(KeyCode.F))
            {
                transform.Rotate(0, 0, m_angle);
            }
  
        }



        


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
