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

    // Drain Methods
    public void TakeDamage(float dmg) { health = Mathf.Clamp(health - dmg, 0f, maxHealth); }
    public void DrainMana(float drain) { mana = Mathf.Clamp(mana - drain, 0f, maxMana); }

    // Restore Methods
    public void RestoreHealth(float hp) { health = Mathf.Clamp(health + hp, 0f, maxHealth); }
    public void RestoreMana(float m) { mana = Mathf.Clamp(mana + m, 0f, maxMana); }

    /// <summary>
    /// Default Death event, can be overrode in the inspector
    /// </summary>
    public void DestroyCharacter() { GameObject.Destroy(gameObject);}

    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}