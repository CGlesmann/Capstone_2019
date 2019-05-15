using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WendigoAI : MeleeAI
{
    [Header("Jump Attack Variables")]
    [SerializeField] private Vector3 jumpForce = Vector3.one;
    [SerializeField] private float jumpAcceptanceRange = 1f;
    private bool jumping = false;

    private NavMeshAgent agent = null;

    /// <summary>
    /// Gets Private Reference
    /// </summary>
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        // Call the base update method from BattleAI
        base.Update();

        // Check for jump landing
        if (jumping && agent.velocity == Vector3.zero)
        {
            // Damage the player if in range
            if (Vector3.Distance(transform.position, target.transform.position) <= jumpAcceptanceRange)
                target.TakeDamage(20f);

            // Reset attack state vars
            jumping = false;
            attackInProgress = false;

            GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    /// <summary>
    /// Function that executes an attack AI
    /// </summary>
    public void ExecuteJumpAttack()
    {
        if (target != null)
        {
            // Stop the Agent
            GetComponent<NavMeshAgent>().isStopped = true;

            // Apply the Jump Force
            transform.LookAt(target.transform);

            Vector3 force = Vector3.Scale(transform.forward + new Vector3(0f, 1f, 0f), jumpForce);
            GetComponent<NavMeshAgent>().velocity = force;
            jumping = true;
        }
    }
}
