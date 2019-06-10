using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossMovement : EnemyMovement
{
    public static BossMovement boss = null;

    [Header("Movement Settings")]
    public bool fightingEngaged = false;

    private void Start()
    {
        // Get player reference
        player = PlayerCombatController.controller.transform;

        // Resetting the NavMesh
        if (fightingEngaged)
        {
            agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
            agent.speed = chaseSpeed;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        // Singleton
        if (boss == null)
            boss = this;
        else
            GameObject.Destroy(gameObject);

        // Setting State
        if (fightingEngaged)
        {
            state = EnemyAIState.Chasing;
            anim.SetBool("Walking", true);
        }
        else
            anim.SetBool("Walking", false);

        return;
    }

    protected override void StateMachine()
    {
        if (fightingEngaged)
        {
            if (state == EnemyAIState.Chasing)
            {
                agent.destination = player.transform.position;
                agent.speed = chaseSpeed;

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

    public void EngageFight() {
        fightingEngaged = true;
        GetComponent<CombatCharacter>().SetInvurnable(true);
    }
}
