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
    [SerializeField] private EnemyAIState state = EnemyAIState.Patrolling;
    private bool executeMovementAI = true;

    [Header("Enemy Patrol Variables")]
    [SerializeField] private PatrolPoint[] wayPoints = null;
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float acceptanceRange = 1f;

    private MoveController controller = null;
    private NavMeshAgent agent = null;

    private PatrolPoint currentPPoint = null;
    private bool pathStarted = false;
    private int pointIndex = 0;
    private float pauseTimer = 0f;

    [Header("Enemy Chase Variables")]
    [SerializeField] private LayerMask playerLayer = new LayerMask();
    [SerializeField] private Vector3 sightOffset = Vector3.zero;
    [SerializeField] private Vector3 sightRange = Vector3.zero;
    [SerializeField] private float chaseSpeed = 7.5f;

    private Transform player = null;
    private float updateCooldown = 1f;
    private float updateTimer = 0f;

    [Header("Enemy Battle AI")]
    private BattleAI battleAI = null;

    /// <summary>
    /// Grabs Private References
    /// </summary>
    private void Awake()
    {
        // Getting Reference to the MoveController
        controller = GetComponent<MoveController>();
        agent = GetComponent<NavMeshAgent>();

        // Movement Behavior from combat character
        battleAI = GetComponent<BattleAI>();

        // Setting the Initial Path
        if (state == EnemyAIState.Patrolling)
            ChangeCurrentPoint();
    }

    /// <summary>
    /// Checks for Point Change
    /// </summary>
    private void Update()
    {
        // Patrol AI / Patrol -> Chasing Transition
        if (state == EnemyAIState.Patrolling)
        {
            // Patrolling AI
            ExecutePatrolAI();

            // Checking for Patrol -> Chasing Transition
            if (PlayerSpotted())
            {
                // Resetting the NavMesh
                agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z); ;
                agent.speed = chaseSpeed;
                updateTimer = updateCooldown;

                // Setting State
                state = EnemyAIState.Chasing;
                return;
            }
        }

        // Chasing AI / Chase -> Attack Transition
        if (state == EnemyAIState.Chasing)
        {
            // Chasing AI
            ExecuteChaseAI();

            // Checking for Chase -> Attack Transition
            if (battleAI.PlayerInAttackRange())
            {
                state = EnemyAIState.Attacking;
                battleAI.EngageAttackAI();
                agent.isStopped = true;
                updateTimer = 0f;
            }
        }

        // Attacking -> Chasing Transition
        if (state == EnemyAIState.Attacking)
        {
            if (!battleAI.PlayerInAttackRange())
            {
                // Disengaging from combat
                battleAI.DisEngageAttackAI();
                agent.isStopped = false;

                // Resetting the NavMesh
                agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z); ;
                agent.speed = chaseSpeed;
                updateTimer = updateCooldown * 2f;

                // Setting State
                state = EnemyAIState.Chasing;
                return;
            }
        }
    }

    private Vector3 Mult(Vector3 left, Vector3 right)
    {
        return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
    }

    #region Patrol AI Functions
    /// <summary>
    /// Executes the AI for enemy patrolling behavior
    /// </summary>
    private void ExecutePatrolAI()
    {
        Debug.Log("Patrolling");

        // Setting Movement Speed
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

                // Setting the Pause Timer (if Needed)
                if (currentPPoint.pauseHere)
                    pauseTimer = currentPPoint.pauseLength;
            }
        }
    }

    /// <summary>
    /// Gets the Next point and sets the agent to it.
    /// </summary>
    private void ChangeCurrentPoint()
    {
        // Getting the Next Point
        if (++pointIndex > wayPoints.Length - 1)
            pointIndex = 0;

        // Setting the Point and Starting the Agent
        currentPPoint = wayPoints[pointIndex];
        agent.destination = currentPPoint.wayPoint.transform.position;

        // Setting the path
        pathStarted = true;
        agent.isStopped = false;
    }
    #endregion

    #region Chasing AI Functions
    // Check Functions
    private bool PlayerSpotted()
    {
        // Getting the Player Reference
        Collider[] col = Physics.OverlapBox(transform.position + Mult(sightOffset, GetComponent<NavMeshAgent>().velocity.normalized), sightRange / 2f, Quaternion.Euler(transform.forward), playerLayer);
        player = (col.Length > 0) ? col[0].transform : null;

        if (player != null)
            Debug.Log(player.ToString());

        // Returning Result
        return (player != null);
    }

    // AI Loop
    private void ExecuteChaseAI()
    {
        // Checking for path update
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0f)
        {
            agent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
            updateTimer = updateCooldown;
        }
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
