using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerController")]
    private float SetTime = 600f;
    private float Timer = 0f;
    private bool MouseClick = false;
    public Transform prefab;
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MouseClick = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if(Timer >= 60)
            {
                Instantiate(prefab, transform.position + (transform.forward), transform.rotation);
            }
            else
            {
                Instantiate(prefab, transform.position + (transform.forward), transform.rotation);
            }
            
            Debug.Log(Timer);
            MouseClick = false;
            Timer = 0;
        }

        if(MouseClick == true)
        {
            Timer += 1;      
        }

        
    }
            
       
}
