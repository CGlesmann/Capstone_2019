using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossMovement : EnemyMovement
{
    [Header("Movement Settings")]
    public bool fightingEngaged = false;

    protected override void Awake()
    {
        base.Awake();

        // Getting the Player
        player = battleAI.target.transform;

        // Resetting the NavMesh
        agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.speed = chaseSpeed;

        // Setting State
        state = EnemyAIState.Chasing;
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
                return;
            }
        }
    }
}
