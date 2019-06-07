using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CombatCharacter : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float mana = 100f;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float damageReduction = 1f; //value between 0 and 1 reduces damage by the given percent


    [Header("Death Variables")]
    [SerializeField] private UnityEvent onDeath = null;

    /// <summary>
    /// Checks for death
    /// Invokes onDeath event when the combat dies
    /// </summary>
    private void Update()
    {
        if (health <= 0f)
        {
            onDeath.Invoke();
        }
    }

    // Getter Methods
    public float GetHealthPercent() { return (health / maxHealth); }
    public float GetManaPercent() { return (mana / maxMana); }

    public void SetDamageReduction(float dr) { damageReduction = Mathf.Clamp(dr, 0f, 1f); } 

    // Drain Methods
    public void TakeDamage(float dmg) { health = Mathf.Clamp(health - (dmg * damageReduction), 0f, maxHealth); }
    public void DrainMana(float drain) { mana = Mathf.Clamp(mana - drain, 0f, maxMana); }

    // Restore Methods
    public void RestoreHealth(float hp) { health = Mathf.Clamp(health + hp, 0f, maxHealth); }
    public void RestoreMana(float m) { mana = Mathf.Clamp(mana + m, 0f, maxMana); }

    /// <summary>
    /// Default Death event, can be overrode in the inspector
    /// </summary>
    public void DestroyCharacter()
    {
        if (gameObject.tag == "Enemy")
        {
            GameObject Player = GameObject.Find("Player");
            float award = 0;

            if (gameObject.name.Contains("CorruptedDwarf")) { award = 30; }
            if (gameObject.name.Contains("Wendigo")) { award = 60; }
            if (gameObject.name.Contains("CorruptedFairy")) { award = 100; }
            
            //This coding restores both player mana and health if the play if missing health and mana.
            if (Player.GetComponent<CombatCharacter>().mana < maxMana && Player.GetComponent<CombatCharacter>().health < maxHealth)
            {
                Player.GetComponent<CombatCharacter>().RestoreHealth(award/2);
                Player.GetComponent<CombatCharacter>().RestoreMana(award/2);
                Player.GetComponent<PlayerCombatController>().RestoreMana(award/2);
            }

            //This coding restores the players mana and not the player's health.
            if (Player.GetComponent<CombatCharacter>().mana < maxMana && Player.GetComponent<CombatCharacter>().health >= maxHealth)
            {
                Player.GetComponent<CombatCharacter>().RestoreMana(award);
                Player.GetComponent<PlayerCombatController>().RestoreMana(award);
            }

            //This coding restores the players health and not the player's mana.
            if (Player.GetComponent<CombatCharacter>().mana >= maxMana && Player.GetComponent<CombatCharacter>().health < maxHealth)
            {
                Player.GetComponent<CombatCharacter>().RestoreHealth(award);
            }
        }
        GameObject.Destroy(gameObject);
    }

    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}