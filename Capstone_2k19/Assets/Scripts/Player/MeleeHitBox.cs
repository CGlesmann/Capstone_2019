using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{

    

    public void OnTriggerEnter(Collider col)
    {
        BoxCollider hitbox = gameObject.GetComponent<BoxCollider>();

        if (col.gameObject.tag == "Enemy")
        {                    
            col.gameObject.GetComponent<CombatCharacter>().TakeDamage(20);

            Debug.Log("Enemy Entered");
            hitbox.enabled = false;
        }
    }
}