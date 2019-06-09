using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CombatCharacter))]
public class PlayerGUIManager : MonoBehaviour
{
    [Header("GUI Reference")]
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image manaBar = null;

    private CombatCharacter combatCharacter = null;

    /// <summary>
    /// Grabs the Private Reference
    /// </summary>
    private void Awake() { combatCharacter = GetComponent <CombatCharacter>(); }

    /// <summary>
    /// Update the player GUI
    /// </summary>
    private void Update()
    {
        if (healthBar != null)
            healthBar.fillAmount = combatCharacter.GetHealthPercent();
        if (manaBar != null)
            manaBar.fillAmount = combatCharacter.GetManaPercent();
    }
}
