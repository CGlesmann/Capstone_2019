using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    // Enum that tracks
    public enum EnemyAIState { Patrolling, Chasing, Attacking };

    [Header("Enemy AI State")]
    [SerializeField] private EnemyAIState state = EnemyAIState.Patrolling;

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
    [SerializeField] private string playerTag = "";
    [SerializeField] private float chaseSpeed = 7.5f;

    private bool playerSpotted = false;
    private bool enemyCalled = false;

    /// <summary>
    /// Grabs Private References
    /// </summary>
    private void Awake()
    {
        // Getting Reference to the MoveController
        controller = GetComponent<MoveController>();
        agent = GetComponent<NavMeshAgent>();

        // Setting the Initial Path
        ChangeCurrentPoint();
    }

    /// <summary>
    /// Checks for Point Change
    /// </summary>
    private void Update()
    {
        // Patrol AI / Patrol -> Chasing Transition
        if (state == EnemyAIState.Patrolling) {
            // AI
            ExecutePatrolAI();
            
            // Checking for Patrol -> Chasing Transition
            if (playerSpotted || enemyCalled)
            {
                state = EnemyAIState.Chasing;
                return;
            }
        }
    }

    #region Patrol AI Function
    /// <summary>
    /// Executes the AI for enemy patrolling behavior
    /// </summary>
    private void ExecutePatrolAI()
    {
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
