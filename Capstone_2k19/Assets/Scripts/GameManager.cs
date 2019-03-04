using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager = null;

    [Header("Global Stat Variables")]
    public float timeMult = 1f;
    [SerializeField] private bool timeSlowed = false;

    private float timeLost = 0f;

    private void Awake()
    {
        manager = this;
    }

    // Called by boss GameManager.manager.SlowTime
    public void SlowTime(float timeLoss)
    {
        if (!timeSlowed) {
            timeLost = timeLoss;
            StartCoroutine("SlowDownTime", timeLoss);
            timeSlowed = true;
        }
    }

    public void ResetTime()
    {
        if (timeSlowed) {
            StartCoroutine("SpeedUpTime", timeLost);
            timeSlowed = true;
        }
    }

    private IEnumerator SlowDownTime(float timeLoss)
    {
        int reps = 100;

        for(int i = 0; i < reps; i++) {
            timeMult -= (timeLoss / reps);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private IEnumerator SpeedUpTime(float timeReset)
    {
        int reps = 100;

        for (int i = 0; i < reps; i++) {
            timeMult += (timeReset / reps);
            yield return new WaitForSeconds(0.001f);
        }
    }

}
