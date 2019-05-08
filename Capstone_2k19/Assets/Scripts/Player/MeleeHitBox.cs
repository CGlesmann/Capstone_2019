using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            BoxCollider hitbox = gameObject.GetComponent<BoxCollider>();
            col.gameObject.GetComponent<CombatCharacter>().TakeDamage(20);

            Debug.Log("Enemy Entered");
            hitbox.enabled = false;


        }
    }
}