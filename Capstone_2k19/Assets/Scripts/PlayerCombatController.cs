using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Rigidbody fireBallPrefab;
    public Transform magicHand;
    public GameObject hitBox;
    public float fireBallSpeed = 500;
    public float setTime = 60;
    private float magicMax = 100;
    private float magicMana = 100;
    private float lifeTimer = 10;
    private float magicBuildUp = 0;
    private float Timer = 0;
    private bool Blocking = false;
    private float blockTimer = 0;


    public void rangedAttackSmall()
    {
        //this is code for creating a FireBall with speed direction, size, and resets the timers
        //this small FireBall disables gravity and subtracts a small amount of mana from the mana you have
        Rigidbody fireBallInstance;
        fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
        fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
        fireBallInstance.transform.localScale = new Vector3(1, 1, 1);
        fireBallInstance.useGravity = false;
        gameObject.GetComponent<CombatCharacter>().DrainMana(1);
        magicBuildUp = 0;
        Timer = setTime;
    }

    public void rangedAttackBig()
    {
        //this is code for creating a FireBall with speed direction, size, and resets the timers
        //this big FireBall enables gravity and subtracts the mana you built up from the mana you have
        Rigidbody fireBallInstance;
        fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
        fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
        fireBallInstance.transform.localScale = new Vector3(3, 3, 3);
        fireBallInstance.GetComponent<FireBall>().enabled = false;
        gameObject.GetComponent<CombatCharacter>().DrainMana(magicBuildUp); 
        magicBuildUp = 0;
        Timer = setTime;
    }

    public void meleeAttack()
    {
        hitBox.GetComponent<BoxCollider>().enabled = true;
           
    }

    public void blocking()
    {
        Blocking = true;
    }

    void Update()
    {
        //FireBall Scripting
        if (Timer == 0)
        {
            //builds up magic counter
            if (Input.GetButton("Fire1") && magicBuildUp < magicMana)
            {
                magicBuildUp += .25f;
            }
            //checks if magic counter is greater than 10 if so triggers big FireBall
            if (Input.GetButtonUp("Fire1") && magicBuildUp >= 10 )
            {
                rangedAttackBig();
            }
            //if magic counter is less than 10 makes small FireBall            
            else if(Input.GetButtonUp("Fire1") && magicMana > 1)
            {
                
                rangedAttackSmall();
            }
        }        
        else 
        { 
            //this is a cool dow timer before you can shoot another FireBall         
            Timer -= 1;
        }

        //Blocking
        //has time counter to ceck if you are wanting to block
        //if you are wanting to block than triggers code to start blocking
        if(Input.GetButton("Fire2"))
        {
            blockTimer += 1;

            if (blockTimer >= 10)
            {
                blocking();
                Debug.Log("Blocking");
            }
        }
        //Melee Attack Scripting
        //checks to see if you are blocking if not than triggers a melee attack
        if (Input.GetButtonUp("Fire2"))
        {
            if (Blocking == false)
            {
                meleeAttack();
                blockTimer = 0;
                Debug.Log("Attacked");
            }
            //if you are finished blocking than triggers code to allow to to attack again
            else if(Blocking == true)
            {
                Blocking = false;
                blockTimer = 0;
            }
        }

        //Mana Stuff 
        if (magicMana < magicMax)
        {
            magicMana += Time.deltaTime;
        }
        else if(magicMana > magicMax)
        {
            magicMana = magicMax;
        }
    }
}
