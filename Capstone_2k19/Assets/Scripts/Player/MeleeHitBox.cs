using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        GameObject player = GameObject.Find("Player");
        PlayerCombatController script = player.GetComponent<PlayerCombatController>();

        if (col.gameObject.name == "Enemy (Test)" && script.Attaking == true)
        {
            Destroy(col.gameObject);
        }
    }
}
