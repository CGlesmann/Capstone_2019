using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeAI : BattleAI
{
    [Header("Attacker State Variables")]
    [SerializeField] private float clawDamage = 10f;

    [Header("Melee Attack Variables")]
    [SerializeField] private Attack[] attackPattern = null;
    private int nextAttackID = 0;
    private float currentCooldown = 0f;

    /// <summary>
    /// Loop that executes the attack AI
    /// </summary>
    private void Update()
    {
        if (attackAIEngaged)
        {
            // Check if enemy is ready for the next attack
            if (currentCooldown <= 0f)
            {
                // Execute the next attack
                if (++nextAttackID > attackPattern.Length - 1)
                    nextAttackID = 0;
                attackPattern[nextAttackID].attackEvent.Invoke();

                // Setting the Cooldown
                currentCooldown = attackPattern[nextAttackID].attackCooldown;
            }
            else
                currentCooldown -= Time.deltaTime;
        }
    }

    #region Attack Functions
    public void ClawSwipe()
    {
        // Dealing Damage
        target.TakeDamage(clawDamage);
    }
    #endregion   
}

[System.Serializable]
public class Attack
{
    // Attack Variables
    public float attackCooldown = 0f;
    public UnityEvent attackEvent = null;
}
