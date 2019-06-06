using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MeleeAI
{
    [Header("Environment Attack Vars")]
    [SerializeField] private int numOfShots = 3;
    [SerializeField] private float ySpawnPoint = 0f;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private GameObject environmentAttackPrefab = null;

    public void EnvironmentAttack()
    {
        StartCoroutine(SpawnEnvironmentHazards());
        anim.SetBool("Spawning", true);
    }

    private IEnumerator SpawnEnvironmentHazards()
    {
        Debug.Log("Beginning Spawning");
        for(int i = 0; i < numOfShots; i++)
        {
            // Spawn the new hazard
            Vector3 spawnPoint = new Vector3(target.transform.position.x, target.transform.position.y - (target.transform.localScale.y * 2f), target.transform.position.z);
            GameObject newHazard = Instantiate(environmentAttackPrefab);
            newHazard.transform.position = spawnPoint;

            // Spawn Delay
            yield return new WaitForSeconds(spawnDelay);
        }
        attackInProgress = false;
    }

    public void TongueAttack()
    {
        Debug.Log("Tongue Attack");
        TongueStrike();
        anim.SetTrigger("ClawSwipe");
    }

    private void TongueStrike()
    {
        if (PlayerInAttackRange())
            target.TakeDamage(25f);
        attackInProgress = false;
    }
}
