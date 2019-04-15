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
        Debug.Log("Try Jump");
        // Check if the wendigo is in JumpRange
        CombatCharacter player = GetPlayer();
        if (player != null) {
            Debug.Log("Jumping");

            // Stop the Agent
            GetComponent<NavMeshAgent>().isStopped = true;

            // Apply the Jump Force
            transform.LookAt(player.transform);

            Vector3 force = Vector3.Scale(transform.forward, jumpForce);
            GetComponent<Rigidbody>().AddForce(force);

            attackInProgress = false;
        }
    }
}
