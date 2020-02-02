using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmHit : MonoBehaviour
{
    public Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if ( Input.GetKeyDown(KeyCode.F) && tag == "Player")
        {
            m_Anim.Play("ArmHit");
        }
        


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
