using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Navigation Variables")]
    [SerializeField] private string optionScene = "";

    public void NavigateToOptions() { SceneManager.LoadScene(optionScene); }
    public void ExitGame() { Application.Quit(); }
}
