using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {      
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Entered");
        }        
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Stay");            
        }
    }

    public void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Exited");
        }
    }
}
