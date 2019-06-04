using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{    
    GameObject Player = GameObject.Find("Player");

    public void OnTriggerEnter(Collider col)
    {
        BoxCollider hitbox = gameObject.GetComponent<BoxCollider>();

        if (col.gameObject.tag == "Enemy")
        {                    
            col.gameObject.GetComponent<CombatCharacter>().TakeDamage(20);
            col.gameObject.GetComponent<CombatCharacter>().meleeAttacked = true;

            Debug.Log("Enemy Entered");
            hitbox.enabled = false;
        }
    }
}