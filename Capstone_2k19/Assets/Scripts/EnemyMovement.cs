using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Boss Movement Variables")]
    [SerializeField] private float moveSpeed = 5f;

    private MoveController controller = null;
    private Vector3 motion = Vector3.zero;

    private void Awake()
    {
        // Getting Reference to the MoveController
        controller = GetComponent<MoveController>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Performing movement if Boss is in motion
        if (motion != Vector3.zero)
            controller.PerformMove(motion * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Gets a new Wander Direction for the boss when in "Patrol" Mode
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNewDirection()
    {
        return Vector3.zero;
    }

}
