using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Rigidbody fireBallPrefab;
    public Transform magicHand;

    [SerializeField] private float magicBuildUp = 0;
    private float maxMana = 100;
    private float mana = 100;
    private float fireBallSpeed = 500;
    private float blockTimer = 0;
    private float coolDownTimer = 0;
    private float attackTimer = 0;
    private float setTime = 10;
    private bool Blocking = false;
    public bool Attacking = false;
    

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
        mana -= 1;
        magicBuildUp = 0;
        coolDownTimer = setTime;
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
        mana -= magicBuildUp;
        magicBuildUp = 0;
        coolDownTimer = setTime;
    }

    public void meleeAttack()
    {
        Attacking = true;
        attackTimer += 1;
    }

    public void blocking()
    {
        Blocking = true;
        gameObject.GetComponent<CombatCharacter>().SetDamageReduction(1);
    }

    public void manaGain()
    {
        if (mana < maxMana)
        {
            float addMana = 0f;
            addMana += 2 * Time.deltaTime;
            mana += addMana;
            gameObject.GetComponent<CombatCharacter>().RestoreMana(addMana);
        }
        else if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    void Update()
    {
        //FireBall Scripting
        if (coolDownTimer == 0)
        {
            //builds up magic counter
            if (Input.GetButton("Fire1") && magicBuildUp < mana){ magicBuildUp += .25f; }
            
            //checks if magic counter is greater than 10 if so triggers big FireBall
            if (Input.GetButtonUp("Fire1") && magicBuildUp >= 10){ rangedAttackBig(); }            
            
            //if magic counter is less than 10 makes small FireBall            
            else if(Input.GetButtonUp("Fire1") && mana > 1){ rangedAttackSmall(); }
        }        
        else //this is subtracking from cool down timer         
        { coolDownTimer -= 1; }

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
            }
            //if you are finished blocking than triggers code to allow to to attack again
            else if(Blocking == true)
            {
                Blocking = false;
                blockTimer = 0;
            }
        }

        if(attackTimer == 0)
        {
            Attacking = false;
        }
        else
        {
            attackTimer -= 1;
        }

        //Restoring Mana 
        manaGain();
    }
}
