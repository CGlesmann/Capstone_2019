using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NavMeshAgent), typeof(BattleAI))]
public class EnemyMovement : MonoBehaviour
{
    // Enum that tracks
    public enum EnemyAIState { Patrolling, Chasing, Attacking };

    [Header("Enemy AI State")]
    [SerializeField] protected EnemyAIState state = EnemyAIState.Patrolling;
    protected bool executeMovementAI = true;

    [Header("Enemy Patrol Variables")]
    [SerializeField] protected PatrolPoint[] wayPoints = null;
    [SerializeField] protected float patrolSpeed = 5f;
    [SerializeField] protected float acceptanceRange = 1f;

    protected MoveController controller = null;
    protected NavMeshAgent agent = null;

    protected PatrolPoint currentPPoint = null;
    protected bool pathStarted = false;
    [SerializeField] protected int pointIndex = 0;
    protected float pauseTimer = 0f;

    [Header("Enemy Chase Variables")]
    [SerializeField] protected LayerMask playerLayer = new LayerMask();
    [SerializeField] protected Vector3 sightOffset = Vector3.zero;
    [SerializeField] protected Vector3 sightRange = Vector3.zero;
    [SerializeField] protected float chaseSpeed = 7.5f;

    protected Transform player = null;
    protected Animator anim = null;

    [Header("Enemy Battle AI")]
    protected BattleAI battleAI = null;

    // AI toggles
    public void EnableMovementAI() { executeMovementAI = true; }
    public void DisableMovementAI() { executeMovementAI = false; }

    private void Start()
    {
        // Get player reference
        player = PlayerCombatController.controller.transform;
    }

    /// <summary>
    /// Grabs Private References
    /// </summary>
    protected virtual void Awake()
    {
        // Getting Reference to the MoveController
        controller = GetComponent<MoveController>();
        agent = GetComponent<NavMeshAgent>();

        // Movement Behavior from combat character
        battleAI = GetComponent<BattleAI>();

        anim = GetComponent<Animator>();

        // Setting the Initial Path
        if (state == EnemyAIState.Patrolling)
            ChangeCurrentPoint();
    }

    /// <summary>
    /// Checks for Point Change
    /// </summary>
    protected void FixedUpdate() {
        StateMachine();

        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(name + " is " + battleAI.DistanceToPlayer() + " away from player");
        }
    }

    /// <summary>
    /// Executes the finite state machine
    /// </summary>
    protected virtual void StateMachine()
    {
        Debug.Log("State Machine");
        if (executeMovementAI)
        {
            Debug.Log("Movement AI");
            // Patrol AI / Patrol -> Chasing Transition
            if (state == EnemyAIState.Patrolling) {
                // Patrolling AI
                ExecutePatrolAI();

                // Checking for Patrol -> Chasing Transition
                if (player != null && battleAI.PlayerInAttackRange())
                {
                    // Resetting the NavMesh
                    agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
                    agent.speed = chaseSpeed;

                    // Setting State
                    state = EnemyAIState.Chasing;
                    return;
                }
            }
            if (state == EnemyAIState.Chasing)
            {
                Debug.Log("chasing");
                agent.destination = player.transform.position;

                // Checking for Chase -> Attack Transition
                if (battleAI.PlayerInAttackRange())
                {
                    battleAI.EngageAttackAI();
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    agent.speed = 0f;
                    anim.SetBool("Walking", false);

                    state = EnemyAIState.Attacking;
                    return;
                }
            }
            else if (state == EnemyAIState.Attacking)
            {
                Debug.Log("Attacking");
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

    public void SetChaseState()
    {
        // Grab the player
        player = PlayerCombatController.controller.transform;

        // Set the state to chase
        state = EnemyAIState.Chasing;

        agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.speed = chaseSpeed;
    }

    #region Patrol AI Functions
    /// <summary>
    /// Executes the AI for enemy patrolling behavior
    /// </summary>
    protected void ExecutePatrolAI()
    {
        Debug.Log("Patrolling");

        // Setting Movement Speed 
        agent.destination = currentPPoint.wayPoint.transform.position;
        agent.speed = patrolSpeed;

        // Check for movement
        if (agent.velocity == Vector3.zero && !pathStarted)
        {
            // Decrement the pauseTimer
            if (pauseTimer > 0f)
                pauseTimer -= Time.deltaTime;
            else
            {
                // Timer is spent, change direction
                ChangeCurrentPoint();
            }
        }
        else
        {
            //Getting the Distance
            float dist = Vector3.Distance(transform.position, currentPPoint.wayPoint.transform.position);

            // Check if agent is within acceptanceRange of the point
            if (dist <= acceptanceRange)
            {
                // Stopping the Agent
                agent.isStopped = true;

                // Toggling the path
                pathStarted = false;
                anim.SetBool("Walking", false);

                // Setting the Pause Timer (if Needed)
                if (currentPPoint.pauseHere)
                    pauseTimer = currentPPoint.pauseLength;
            }
        }
    }

    /// <summary>
    /// Gets the Next point and sets the agent to it.
    /// </summary>
    protected void ChangeCurrentPoint()
    {
        // Getting the Next Point
        if (++pointIndex > wayPoints.Length - 1)
            pointIndex = 0;

        // Setting the Point and Starting the Agent
        currentPPoint = wayPoints[pointIndex];
        if (currentPPoint != null && agent != null && currentPPoint.wayPoint != null)
            agent.destination = currentPPoint.wayPoint.transform.position;

        // Setting the path
        pathStarted = true;
        agent.isStopped = false;

        // Set Walking Trigger
        anim.SetBool("Walking", true);
    }
    #endregion

    #region Chasing AI Functions
    // Check Functions
    protected bool PlayerSpotted()
    {
        Debug.Log("Looking for a player");
        // Getting the Player Reference
        Collider[] col = Physics.OverlapBox(transform.position + Vector3.Scale(sightOffset, GetComponent<NavMeshAgent>().velocity.normalized), sightRange / 2f, Quaternion.Euler(transform.forward), playerLayer);
        player = (col.Length > 0) ? col[0].transform : null;

        if (player != null)
            Debug.Log(player.ToString());

        // Returning Result
        return (player != null && battleAI.PlayerInAttackRange());
    }

    protected void SetChasePath()
    {
        // Resetting the NavMesh
        agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.speed = chaseSpeed;

        agent.isStopped = false;
        anim.SetBool("Walking", true);
    }
    #endregion
}

[System.Serializable]
public class PatrolPoint
{
    // Variable Declarations
    public GameObject wayPoint;
    public bool pauseHere;
    public float pauseLength;

    /// <summary>
    /// Sets PatrolPoint variables
    /// </summary>
    /// <param name="point"></param>
    /// <param name="pause"></param>
    /// <param name="length"></param>
    public PatrolPoint(GameObject point, bool pause, float length)
    {
        wayPoint = point;
        pauseHere = pause;
        pauseLength = length;
    }

}
