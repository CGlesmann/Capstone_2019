using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private string gameScene = "";
    [SerializeField] private string optionScene = "";
    
    /// <summary>
    /// Public Method that take the player to the initial game scene
    /// </summary>
    public void StartGame() { SceneManager.LoadScene(gameScene); }
    /// <summary>
    /// Public Method that takes the player to the options screen
    /// </summary>
    public void GoToOptions() { SceneManager.LoadScene(optionScene); }
    /// <summary>
    /// Public Method that terminates the game when called
    /// </summary>
    public void ExitGame() { Debug.Log("Quitting"); Application.Quit(); }
}
