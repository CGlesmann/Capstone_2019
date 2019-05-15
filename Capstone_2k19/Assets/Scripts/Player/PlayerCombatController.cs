using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController controller = null;
    public GameObject EnemyPrefab;
    public Rigidbody fireBallPrefab;
    public Transform magicHand;
    public Transform EnemySpawn;
    
    [SerializeField] private float magicBuildUp = 0;    
    private float maxMana = 100;
    private float mana = 100;
    private float fireBallSpeed = 600;
    private float blockTimer = 0;
    private float coolDownTimer = 0;
    private float setTime = 10;
    private bool Blocking = false;
    [SerializeField] public bool Attacking = false;

    public void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
    }

    public void rangedAttackSmall()
    {
        //this is code for creating a FireBall with speed direction, size, and resets the timers
        //this small FireBall disables gravity and subtracts a small amount of mana from the mana you have
        Rigidbody fireBallInstance;
        fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
        fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
        fireBallInstance.transform.localScale = new Vector3(.5f, .5f, .5f);
        gameObject.GetComponent<CombatCharacter>().DrainMana(5);
        mana -= 5;
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
        fireBallInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        fireBallInstance.GetComponent<FireBall>().enabled = false;
        fireBallInstance.GetComponent<FireBall>().manaCharge(magicBuildUp);
        gameObject.GetComponent<CombatCharacter>().DrainMana(magicBuildUp);
        mana -= magicBuildUp;
        magicBuildUp = 0;
        coolDownTimer = setTime;
    }

    public void meleeAttack()
    {
        GameObject hitbox = GameObject.Find("HitBox");
        BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();
        
        boxCollider.enabled = true;
        Attacking = true;
        Debug.Log("Attack");            
    }

    public void blocking()
    {
        float DR;
        Blocking = true;
        DR = mana / maxMana;
        gameObject.GetComponent<CombatCharacter>().SetDamageReduction(DR);
    }

    public void manaGain()
    {
        if (mana < maxMana)
        {
            float addMana = 0f;
            addMana += .25f;
            mana += addMana;
            gameObject.GetComponent<CombatCharacter>().RestoreMana(addMana);
        }
        else if (mana > maxMana)
        {
            mana = maxMana;
        }
    }
    private void Awake()
    {
        if (controller == null)
        {
            controller = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
            GameObject.Destroy(gameObject);
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

            if (blockTimer >= 30)
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
                Attacking = false;
            }
            //if you are finished blocking than triggers code to allow to to attack again
            else if(Blocking == true)
            {
                Blocking = false;
                blockTimer = 0;
                Attacking = false;
            }
        }

        //SpawnEnemy
        if(Input.GetKeyDown("f")){ SpawnEnemy(); }
        
        //Restoring Mana 
        manaGain();
    }
}
