using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager = null;

    [Header("Global Stat Variables")]
    public float timeMult = 1f;
    [SerializeField] private bool timeSlowed = false;

    [Header("Navigation Settings")]
    [SerializeField] private string mainMenu = "";

    private float timeLost = 0f;

    private void Awake()
    {
        manager = this;
        Application.targetFrameRate = 120;
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

    /// <summary>
    /// Restarts the Game
    /// </summary>
    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void ReturnToMainMenu() { SceneManager.LoadScene(mainMenu); }

}
