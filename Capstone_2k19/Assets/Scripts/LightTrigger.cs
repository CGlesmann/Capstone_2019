using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LightTrigger : MonoBehaviour
{
    [Header("Torch Effect References")]
    [SerializeField] private GameObject lightSource = null;
    [SerializeField] private GameObject fireEffect = null;

    /// <summary>
    /// Sets the torch to be off by default
    /// </summary>
    private void Awake()
    {
        lightSource.SetActive(false);
        fireEffect.SetActive(false);
    }

    /// <summary>
    /// Activate the light and particle effect
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Player"))
        {
            lightSource.SetActive(true);
            fireEffect.SetActive(true);
        }
    }

    /// <summary>
    /// De-activates the light and particle effect
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Player"))
        {
            lightSource.SetActive(false);
            fireEffect.SetActive(false);
        }
    }
}
