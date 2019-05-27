using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Singleton reference
    public static PauseManager manager = null;

    [Header("Pause Variables")]
    public bool isPaused = false;
    public GameObject pauseMenu = null;

    /// <summary>
    /// Sets the singleton pattern
    /// </summary>
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
            GameObject.Destroy(gameObject);

        // Setting the Cursor Lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Check for Enabling the Pause Menu
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                UnPauseGame();
        }
    }

    /// <summary>
    /// Pause Toggles
    /// </summary>
    public void PauseGame() { pauseMenu.SetActive(true); isPaused = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    public void UnPauseGame() { pauseMenu.SetActive(false); isPaused = false; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }

    public void ExitGame() { Debug.Log("Quitting"); Application.Quit(); }
}
