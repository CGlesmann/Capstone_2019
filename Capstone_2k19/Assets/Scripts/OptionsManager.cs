using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Option References")]
    [SerializeField] private AudioMixerGroup musicMixer = null;
    [SerializeField] private AudioMixerGroup sfxMixer = null;

    [Header("GUI References")]
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider sfxSlider = null;

    [Header("Navigation Settings")]
    [SerializeField] private string tutorialScene = "";

    private void Awake()
    {
        // Music Volume
        musicMixer.audioMixer.GetFloat("Volume", out float mVal);
        musicSlider.value = mVal;

        // SFX Volume
        sfxMixer.audioMixer.GetFloat("Volume", out float sVal);
        sfxSlider.value = sVal;
    }

    public void NavigateToTutorial() { SceneManager.LoadScene(tutorialScene); }

    public void SetMusicVolume() { musicMixer.audioMixer.SetFloat("Volume", musicSlider.value); }
    public void SetSfxVolume() { sfxMixer.audioMixer.SetFloat("Volume", sfxSlider.value); }

    public void ExitOptions(string sceneName) { SceneManager.LoadScene(sceneName); }
}
