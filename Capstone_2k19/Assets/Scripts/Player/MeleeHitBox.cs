using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    [SerializeField] private float resetTimer = 0;
    GameObject Player = GameObject.Find("Player");
    private float collectTimer = 0;

    public void OnTriggerEnter(Collider col)
    {
        BoxCollider hitbox = gameObject.GetComponent<BoxCollider>();

        if (col.gameObject.tag == "Enemy")
        {                    
            col.gameObject.GetComponent<CombatCharacter>().TakeDamage(20);

            Debug.Log("Enemy Entered");
            hitbox.enabled = false;
        }

        if (col.gameObject.name.Contains("DeadEnemy"))
        {
            float collectTimer = resetTimer;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name.Contains("DeadEnemy"))
        {
            if(Input.GetKey("F"))
            {
                if (collectTimer > 0)
                {
                    collectTimer -= 1;
                }
                else if (collectTimer <= 0)
                {
                    Player.gameObject.GetComponent<CombatCharacter>().RestoreMana(30);
                    Destroy(col.gameObject);
                }
            }
        }
    }
}