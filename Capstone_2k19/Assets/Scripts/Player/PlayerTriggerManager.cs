using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string bossTrigger = "";
    [SerializeField] private string spawnerTrigger = "";
    [SerializeField] private string lightTrigger = "";
    [SerializeField] private string hazardTrigger = "";

    private CombatCharacter character = null;

    /// <summary>
    /// Get Private References
    /// </summary>
    private void Awake() { character = GetComponent<CombatCharacter>(); }

    private void OnTriggerEnter(Collider other)
    {
        // Get the GameObject reference
        GameObject obj = other.gameObject;

        // Check for a light
        if (obj != null)
        {
            if (obj.CompareTag(lightTrigger)) { obj.GetComponent<LightTrigger>().ActivateLight(); }
            if (obj.CompareTag(spawnerTrigger)) { obj.GetComponent<EnemySpawner>().StartSpawnEnemies(); }
            if (obj.CompareTag(bossTrigger)) { BossMovement.boss.EngageFight(); }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Get the GameObject Reference
        GameObject obj = other.gameObject;

        if (obj != null)
            if (obj.CompareTag(hazardTrigger)) { character.TakeDamage(2.5f * Time.deltaTime); }
    }

    private void OnTriggerExit(Collider other)
    {
        // Get the GameObject reference
        GameObject obj = other.gameObject;

        // Check for a light
        if (obj != null)
            if (obj.CompareTag(lightTrigger)) { obj.GetComponent<LightTrigger>().DeactivateLight(); }
    }
}
