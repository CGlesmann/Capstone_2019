using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public float setcollectingTime = 30;
    private float collectingTime = 0;

    public void OnTriggerEnter(Collider col)
    {
        BoxCollider hitbox = gameObject.GetComponent<BoxCollider>();

        if (col.gameObject.tag == "Enemy" && gameObject.GetComponent<PlayerCombatController>().attacking == true)
        {                    
            col.gameObject.GetComponent<CombatCharacter>().TakeDamage(20);
            col.gameObject.GetComponent<CombatCharacter>().meleeAttacked = true;

            Debug.Log("Enemy Entered");
            hitbox.enabled = false;
        }

        if (col.gameObject.name.Contains("DeadEnemy"))
        {
            Debug.Log("Collection time set");
            collectingTime = setcollectingTime;  
        }
    }

    public void OnTriggerStay(Collider col)
    {
        GameObject Player = GameObject.Find("Player");

        if (col.gameObject.name.Contains("DeadEnemy"))
        {
            if (collectingTime > 0)
            {
                Debug.Log("Collecting Mana");
                collectingTime -= 1;
            }
            else if(collectingTime <= 0)
            {
                Player.gameObject.GetComponent<CombatCharacter>().RestoreMana(30);
                Destroy(col.gameObject);
            }
        }
    }
}