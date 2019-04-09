using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleAI : MonoBehaviour
{
    [Header("Battle AI Variables")]
    [SerializeField] private float attackRange;

    [Header("AI Attack Pattern")]
    [SerializeField] private UnityEvent[] attackPattern = null;

    private int attackPatternIndex = 0; // Tracks the current index in the attackPattern array

    /// <summary>
    /// Returns the Enemy's attack range
    /// Used for movement
    /// </summary>
    /// <returns></returns>
    public float GetRange() { return attackRange; }

    private void OnDrawGizmosSelected()
    {
        // Drawing the Attack Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(attackRange, 1f, attackRange));
    }
}
