using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public bool InRange = false;
    public void OnTriggerEnter(Collider col)
    {      
        if (col.gameObject.tag == "Enemy")
        {
            InRange = true;
            Debug.Log("Enemy Entered");
        }        
    }

    public void OnTriggerStay(Collider col)
    {
        
        GameObject Player = GameObject.Find("Player");
        PlayerCombatController script = Player.GetComponent<PlayerCombatController>();

        if (col.gameObject.tag == "Enemy" && script.Attacking == true)
        {
            Destroy(col.gameObject);
            Debug.Log("Enemy Stay");            
        }
    }

    public void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "Enemy")
        {
            InRange = false;
            Debug.Log("Enemy Exited");
        }
    }
}
