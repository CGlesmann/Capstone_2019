using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossMovement : EnemyMovement
{
    [Header("Movement Settings")]
    public bool fightingEngaged = false;

    private void Start()
    {
        // Get player reference
        player = PlayerCombatController.controller.transform;

        // Resetting the NavMesh
        agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.speed = chaseSpeed;
    }

    protected override void Awake()
    {
        base.Awake();

        // Setting State
        state = EnemyAIState.Chasing;
        anim.SetBool("Walking", true);
        return;
    }

    protected override void StateMachine()
    {
        if (state == EnemyAIState.Chasing)
        {
            agent.destination = player.transform.position;

            // Checking for Chase -> Attack Transition
            if (battleAI.PlayerInAttackRange())
            {
                battleAI.EngageAttackAI();
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.speed = 0f;

                state = EnemyAIState.Attacking;
                anim.SetBool("Walking", false);
                return;
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
                anim.SetBool("Walking", true);
                return;
            }
        }
    }
}
