using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hazards : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float fallDamage = 20f;
    private CombatCharacter target = null;

    [Header("Lifetime Settings")]
    [SerializeField] private bool decaying = false;
    [SerializeField] private float lifeTime = 5f;
    private float lifeTimer = 0f;

    private Animator anim = null;

    private void Awake() { anim = GetComponent<Animator>(); }

    /// <summary>
    /// Decrements the Life Timer, Destroys when timer runs out.
    /// </summary>
    private void Update()
    {
        if (decaying)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
                anim.SetTrigger("FallOut");
        }
    }

    public void BeginDecay() { lifeTimer = lifeTime; decaying = true; }
    public void DestroyHazard() { GameObject.Destroy(gameObject); }
}
