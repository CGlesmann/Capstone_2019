using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string bossTrigger = "";
    [SerializeField] private string spawnerTrigger = "";
    [SerializeField] private string lightTrigger = "";

    private void OnTriggerEnter(Collider other)
    {
        // Get the GameObject reference
        GameObject obj = other.gameObject;

        // Check for a light
        if (obj.CompareTag(lightTrigger)) { obj.GetComponent<LightTrigger>().ActivateLight(); }
    }

    private void OnTriggerExit(Collider other)
    {
        // Get the GameObject reference
        GameObject obj = other.gameObject;

        // Check for a light
        if (obj.CompareTag(lightTrigger)) { obj.GetComponent<LightTrigger>().DeactivateLight(); }
        if (obj.CompareTag(spawnerTrigger)) { obj.GetComponent<EnemySpawner>().StartSpawnEnemies(); }
        if (obj.CompareTag(bossTrigger)) { BossMovement.boss.EngageFight(); }
    }
}
