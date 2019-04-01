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
    [Header("Enemy Movement Variables")]
    [SerializeField] private PatrolPoint[] wayPoints = null;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceptanceRange = 1f;

    private MoveController controller = null;
    private NavMeshAgent agent = null;

    private PatrolPoint currentPPoint = null;
    private int pointIndex = 0;
    private float pauseTimer = 0f;

    /// <summary>
    /// Grabs Private References
    /// </summary>
    private void Awake()
    {
        // Getting Reference to the MoveController
        controller = GetComponent<MoveController>();
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Checks for Point Change
    /// </summary>
    private void Update()
    {
        // Check for movement
        if (agent.velocity == Vector3.zero)
        {
            // Decrement the pauseTimer
            if (pauseTimer > 0f)
                pauseTimer -= Time.deltaTime;
            else
            {
                // Timer is spent, change direction
                ChangeCurrentPoint();
            }
        } else {
            // Check if agent is within acceptanceRange of the point
            if (Vector3.Distance(transform.position, currentPPoint.wayPoint.transform.position) <= acceptanceRange)
            {
                // Stopping the Agent
                agent.isStopped = true;

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
        agent.SetDestination(currentPPoint.wayPoint.transform.position);
    }
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
