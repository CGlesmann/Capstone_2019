using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WendigoAI : MeleeAI
{
    [Header("Jump Attack Variables")]
    [SerializeField] private Vector3 jumpForce = Vector3.one;

    /// <summary>
    /// Function that executes an attack AI
    /// </summary>
    public void ExecuteJumpAttack()
    {
        if (target != null) {
            // Stop the Agent
            GetComponent<NavMeshAgent>().isStopped = true;

            // Apply the Jump Force
            transform.LookAt(target.transform);

            Vector3 force = Vector3.Scale(transform.forward + new Vector3(0f, 1f, 0f), jumpForce);
            GetComponent<NavMeshAgent>().velocity = force;
        }
    }
}
