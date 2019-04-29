using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoMovement : EnemyMovement
{
    [Header("Jump State Variable")]
    [SerializeField] private float jumpCooldown = 3f;
    private float jumpTimer = 0f;

    [Header("Jump Collision Variables")]
    [SerializeField] private float jumpRange = 1f;

    [Header("Jump Attack Variables")]
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float jumpAcceptanceRange = 1f;
    [SerializeField] private bool jumping = false;

    /// <summary>
    /// Decrements the jump timer,
    /// Checks for a jump collision
    /// </summary>
    protected virtual void Update()
    {
        if (jumping) {
            // Agent is no longer moving
            if (agent.velocity == Vector3.zero)
            {
                // Checking for a hit
                if (Vector3.Distance(transform.position, player.position) <= jumpAcceptanceRange)
                    player.GetComponent<CombatCharacter>().TakeDamage(20f);

                // Resetting the enemy state
                jumping = false;
                jumpTimer = jumpCooldown;
                agent.isStopped = false;

                // Set the Chase Path
                SetChasePath();
            }
        }
        else if (jumpTimer > 0f)
            jumpTimer -= Time.deltaTime;
    }

    protected override void StateMachine()
    {
        if (executeMovementAI && !jumping)
        {
            // Patrol AI / Patrol -> Chasing Transition
            if (state == EnemyAIState.Patrolling)
            {
                // Patrolling AI
                ExecutePatrolAI();

                // Checking for Patrol -> Chasing Transition
                if (PlayerSpotted() || player != null)
                {
                    SetChasePath();

                    // Setting State
                    state = EnemyAIState.Chasing;
                    return;
                }
            }
            else if (state == EnemyAIState.Chasing)
            {
                if (Vector3.Distance(transform.position, player.position) > jumpRange || jumpTimer > 0f)
                {
                    // Checking for Chase -> Attack Transition
                    if (battleAI.PlayerInAttackRange())
                    {
                        state = EnemyAIState.Attacking;
                        battleAI.EngageAttackAI();
                        agent.isStopped = true;
                        agent.speed = 0f;

                        return;
                    }
                } else if (jumpTimer <= 0f) {
                    BeginJump();
                }
            }
            else if (state == EnemyAIState.Attacking)
            {
                if (!battleAI.PlayerInAttackRange())
                {
                    // Disengaging from combat
                    battleAI.DisEngageAttackAI();
                    SetChasePath();

                    // Setting State
                    state = EnemyAIState.Chasing;
                    return;
                }
            }
        }
    }

    private void BeginJump()
    {
        Debug.Log("Jumping");

        // Stop the agent
        agent.isStopped = true;

        // Look at the player and set velocity
        agent.velocity *= jumpForce;

        // Set the state to jumping
        jumping = true;
    }
}
