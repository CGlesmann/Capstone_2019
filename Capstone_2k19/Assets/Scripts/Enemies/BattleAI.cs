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
    [SerializeField] protected float currentCooldown = 0f;

    // State Variables
    [SerializeField] public CombatCharacter target = null;
    public bool attackAIEngaged = false;
    [SerializeField] protected bool attackInProgress = false;
    protected Animator anim = null;

    protected virtual void Start()
    {
        target = PlayerCombatController.controller.GetComponent<CombatCharacter>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Loop that executes the attack AI
    /// </summary>
    protected virtual void Update()
    {
        if (attackAIEngaged)
        {
            if (!attackInProgress && PlayerInAttackRange())
            {
                // Check if enemy is ready for the next attack
                if (currentCooldown <= 0f)
                {
                    // Execute the next attack
                    if (nextAttackID > attackPattern.Length - 1)
                        nextAttackID = 0;

                    attackInProgress = true;
                    attackPattern[nextAttackID].attackEvent.Invoke();

                    // Setting the Cooldown
                    currentCooldown = attackPattern[nextAttackID].attackCooldown;
                }
                else
                {
                    currentCooldown -= Time.deltaTime;
                    if (currentCooldown <= 0f)
                        if (++nextAttackID >= attackPattern.Length)
                            nextAttackID = 0;
                }
            }
        }
    }

    /// <summary>
    /// Sets the AttackAI to be active
    /// </summary>
    public void EngageAttackAI() { attackAIEngaged = true; }
    /// <summary>
    /// Sets the AttackAI to be disabled
    /// </summary>
    public void DisEngageAttackAI() { attackAIEngaged = false; }

    public float DistanceToPlayer() { return Vector3.Distance(transform.position, target.transform.position); }

    /// <summary>
    /// Returns the Enemy's attack range
    /// Used for movement
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAttackRange()
    {
        if (nextAttackID <= attackPattern.Length - 1 && nextAttackID >= 0)
        {
            float dist = (attackAIEngaged ? attackPattern[nextAttackID].minAttackRange : attackPattern[nextAttackID].maxAttackRange);
            return (Vector3.Distance(transform.position, target.transform.position) <= dist);
        }
        return false;
    }
}

[System.Serializable]
public class Attack
{
    // Attack Variables
    //public Vector3 minAttackRange = Vector3.zero;
    //public Vector3 maxAttackRange = Vector3.zero;
    public float minAttackRange = 0f;
    public float maxAttackRange = 0f;
    public float attackCooldown = 0f;
    public UnityEvent attackEvent = null;
}
