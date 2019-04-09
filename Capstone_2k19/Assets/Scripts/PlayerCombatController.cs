using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Rigidbody fireBallPrefab;
    public Transform magicHand;
    public float fireBallSpeed = 500;
    public float setTime = 60;
    public float magicMax = 100;
    public float lifeTimer = 10;
    private float Timer = 0;
    private float magicBuildUp = 0;

    void Update()
    {
        if (Timer == 0)
        {
            if (Input.GetButton("Fire1") && magicBuildUp < magicMax)
            {
                magicBuildUp += .25f;
            }
            if (Input.GetButtonUp("Fire1") && magicBuildUp >= 10 )
            {
                Rigidbody fireBallInstance;
                fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
                fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
                fireBallInstance.transform.localScale = new Vector3(3, 3, 3);
                fireBallInstance.GetComponent<FireBall>().enabled = false;
                magicBuildUp = 0;
                Timer = setTime;
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                Rigidbody fireBallInstance;
                fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
                fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
                fireBallInstance.transform.localScale = new Vector3(1, 1, 1);
                fireBallInstance.useGravity = false;              
                magicBuildUp = 0;
                Timer = setTime;
            }
        }
        else 
        {
            
            Timer -= 1;
        }
    }
}
