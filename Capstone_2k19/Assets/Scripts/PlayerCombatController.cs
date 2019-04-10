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
    public float magicMana = 100;
    public float lifeTimer = 10;
    public float magicBuildUp = 0;
    private float Timer = 0;
    

    void Update()
    {
        if (Timer == 0)
        {
            if (Input.GetButton("Fire1") && magicBuildUp < magicMana)
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
                magicMana -= magicBuildUp;
                magicBuildUp = 0;
                Timer = setTime;
            }
            else if(Input.GetButtonUp("Fire1") && magicMana > 1)
            {
                Rigidbody fireBallInstance;
                fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
                fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
                fireBallInstance.transform.localScale = new Vector3(1, 1, 1);
                fireBallInstance.useGravity = false;
                magicMana -= 1;
                magicBuildUp = 0;
                Timer = setTime;
            }
        }
        else 
        {
            
            Timer -= 1;
        }

        if(magicMana < magicMax)
        {
            magicMana += Time.deltaTime;
        }
        else if(magicMana > magicMax)
        {
            magicMana = magicMax;
        }
    }
}
