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
    [SerializeField] private float jumpLength = 15f;

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
                anim.SetBool("Dashing", false);

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
                if (player != null && (battleAI.PlayerInAttackRange() || Vector3.Distance(transform.position, player.position) <= jumpRange))
                {
                    SetChasePath();

                    // Setting State
                    state = EnemyAIState.Chasing;
                    anim.SetBool("Walking", true);
                    return;
                }
            }
            else if (state == EnemyAIState.Chasing)
            {
                agent.destination = player.transform.position;
                if (battleAI.PlayerInAttackRange() || Vector3.Distance(transform.position, player.position) > jumpRange || jumpTimer > 0f)
                {
                    // Checking for Chase -> Attack Transition
                    if (battleAI.PlayerInAttackRange())
                    {
                        state = EnemyAIState.Attacking;
                        battleAI.EngageAttackAI();
                        anim.SetBool("Walking", false);
                        //executeMovementAI = false;
                        agent.isStopped = true;
                        agent.speed = 0f;

                        return;
                    }
                } else if (jumpTimer <= 0f && Physics.Raycast(new Vector3(transform.position.x, player.position.y, transform.position.z), transform.forward, jumpLength, playerLayer)) {
                    BeginJump();
                }
            }
            else if (state == EnemyAIState.Attacking)
            {
                if (!battleAI.PlayerInAttackRange())
                {
                    // Disengaging from combat
                    battleAI.DisEngageAttackAI();
                    //executeMovementAI = true;
                    SetChasePath();

                    // Setting State
                    state = EnemyAIState.Chasing;
                    anim.SetBool("Walking", true);
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
        anim.SetBool("Dashing", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * jumpLength);
    }
}
