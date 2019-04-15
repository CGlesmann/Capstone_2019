using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement), typeof(NavMeshAgent), typeof(CombatCharacter))]
public class BattleAI : MonoBehaviour
{
    [Header("Battle AI Variables")]
    [SerializeField] protected LayerMask enemyLayer = new LayerMask();

    [Header("Attack Variables")]
    [SerializeField] protected Attack[] attackPattern = null;
    [SerializeField] protected int nextAttackID = 0;
    protected float currentCooldown = 0f;

    // State Variables
    protected CombatCharacter target = null;
    public bool attackAIEngaged = false;
    [SerializeField] protected bool attackInProgress = false;

    /// <summary>
    /// Loop that executes the attack AI
    /// </summary>
    protected virtual void Update()
    {
        if (attackAIEngaged)
        {
            if (!attackInProgress)
            {
                // Check if enemy is ready for the next attack
                if (currentCooldown <= 0f)
                {
                    // Execute the next attack
                    if (++nextAttackID > attackPattern.Length - 1)
                        nextAttackID = 0;

                    attackPattern[nextAttackID].attackEvent.Invoke();
                    attackInProgress = true;

                    // Setting the Cooldown
                    currentCooldown = attackPattern[nextAttackID].attackCooldown;
                }
                else
                    currentCooldown -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Sets the AttackAI to be active
    /// </summary>
    public void EngageAttackAI() { attackAIEngaged = true; target = GetPlayer(); }
    /// <summary>
    /// Sets the AttackAI to be disabled
    /// </summary>
    public void DisEngageAttackAI() { attackAIEngaged = false; target = null; }

    /// <summary>
    /// Returns the Enemy's attack range
    /// Used for movement
    /// </summary>
    /// <returns></returns>
    protected CombatCharacter GetPlayer()
    {
        // Getting the hitbox
        Vector3 hitbox = attackAIEngaged ? attackPattern[nextAttackID].maxAttackRange : attackPattern[nextAttackID].minAttackRange;

        // Get amount of colliders
        Collider[] colliders = null;
        colliders = Physics.OverlapBox(transform.position, hitbox * 2f, Quaternion.Euler(Vector3.forward), enemyLayer);

        return ((colliders.Length > 0) ? (colliders[0].GetComponent<CombatCharacter>()) : null);
    }

    /// <summary>
    /// Checks whether the player is in range
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange()
    {
        // Get the player reference
        CombatCharacter player = GetPlayer();

        // If player is not null then the player has been found
        return (player != null);
    }
}

[System.Serializable]
public class Attack
{
    // Attack Variables
    public Vector3 minAttackRange = Vector3.zero;
    public Vector3 maxAttackRange = Vector3.zero;
    public float attackCooldown = 0f;
    public UnityEvent attackEvent = null;
}
