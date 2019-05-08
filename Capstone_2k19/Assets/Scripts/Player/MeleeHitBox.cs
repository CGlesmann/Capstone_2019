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

            Debug.Log("Enemy Entered");
            Destroy(col.gameObject);
            hitbox.enabled = false;


        }
    }
}