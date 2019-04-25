using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{

    void OnTriggerStay(Collider col)
    {
        GameObject player = GameObject.Find("Player");
        PlayerCombatController script = player.GetComponent<PlayerCombatController>();

        if (col.gameObject.tag == "Enemy" && script.Attacking == true)
        {
            Destroy(col.gameObject);            
        }
    }
}
