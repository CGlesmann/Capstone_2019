using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement), typeof(NavMeshAgent), typeof(CombatCharacter))]
public class BattleAI : MonoBehaviour
{
    [Header("Battle AI Variables")]
    [SerializeField] private LayerMask enemyLayer = new LayerMask();
    [SerializeField] private float attackRange = 0f;

    // State Variables
    protected CombatCharacter target = null;
    protected bool attackAIEngaged = false;

    /// <summary>
    /// Sets the AttackAI to be active
    /// </summary>
    public void EngageAttackAI()
    {
        attackAIEngaged = true;
        target = GetPlayer();
    }

    /// <summary>
    /// Sets the AttackAI to be disabled
    /// </summary>
    public void DisEngageAttackAI()
    {
        attackAIEngaged = false;
        target = null;
    }

    /// <summary>
    /// Returns the Enemy's attack range
    /// Used for movement
    /// </summary>
    /// <returns></returns>
    private CombatCharacter GetPlayer()
    {
        // Get amount of colliders
        Collider[] colliders = null;
        colliders = Physics.OverlapBox(transform.position,
                                       new Vector3(attackRange, transform.localScale.y, attackRange),
                                       Quaternion.identity, enemyLayer);

        return ((colliders.Length > 0) ? (colliders[0].GetComponent<CombatCharacter>()) : null);
    }

    public float GetRange() { return attackRange; }
    public bool PlayerInAttackRange()
    {
        // Get the player reference
        CombatCharacter player = GetPlayer();

        // If player is not null then the player has been found
        return (player != null);
    }

    private void OnDrawGizmosSelected()
    {
        // Drawing the Attack Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(attackRange * 2f, 1f, attackRange * 2f));
    }
}
