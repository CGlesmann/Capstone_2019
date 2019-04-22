using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class MoveController : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody used for simple movement

    private void Awake()
    {
        // Getting the RigidBody Reference
        rb = GetComponent<Rigidbody>();
    }

    public void PerformMove(Vector3 direction)
    {
        // Apply a force of val "Direction" to RigidBody "rb"
        rb.AddForce(direction);
    }
}
