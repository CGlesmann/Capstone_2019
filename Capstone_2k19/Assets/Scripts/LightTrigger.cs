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
    private void Awake() { DeactivateLight(); }

    public void ActivateLight()
    {
        lightSource.SetActive(true);
        fireEffect.SetActive(true);
    }

    public void DeactivateLight()
    {
        lightSource.SetActive(false);
        fireEffect.SetActive(false);
    }
}
