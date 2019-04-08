using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Rigidbody fireBallPrefab;
    public Transform magicHand;
    public float fireBallSpeed = 500;
    public float fireRate = 60;
    private float timer = 0;

    void Update()
    {
        if (timer == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Rigidbody fireBallInstance;
                fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
                fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
                timer = fireRate;
            }
        }
        else
        {
            timer -= 1;
        }
    }
}
