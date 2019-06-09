using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController controller = null;
    public Rigidbody fireBallPrefab;
    public Transform magicHand;
    [SerializeField] private Animator leftArmAnim;
    [SerializeField] private Animator rightArmAnim;
    
    [SerializeField] private float magicBuildUp = 0;
    private bool animating = false;
    private float maxMana = 100;
    private float mana = 100;
    private float fireBallSpeed = 600;
    private float blockTimer = 0;
    private float attackTimer = 10;
    private float coolDownTimer = 0;
    private float setTime = 10;   
    private float smallFireBall = .5f;
    private float bigFireBall = 1.5f;
    private bool blocking = false;
    public bool attacking = false;
    public bool collecting = false;

    public void ShootFireBall()
    {
        //checks if magic counter is greater than 10 if so triggers big FireBall
        if (magicBuildUp >= 10) {
            RangedAttackBig();
        } else if (mana > 1) {
            RangedAttackSmall();
        }

        animating = false;
    }

    public void RangedAttackSmall()
    {
        //this is code for creating a FireBall with speed direction, size, and resets the timers
        //this small FireBall disables gravity and subtracts a small amount of mana from the mana you have
        Rigidbody fireBallInstance;
        fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
        fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
        fireBallInstance.transform.localScale = new Vector3(smallFireBall, smallFireBall, smallFireBall);
        gameObject.GetComponent<CombatCharacter>().DrainMana(5);
        mana -= 5;
        magicBuildUp = 0;
        coolDownTimer = setTime;
    }

    public void RangedAttackBig()
    {
        //this is code for creating a FireBall with speed direction, size, and resets the timers
        //this big FireBall enables gravity and subtracts the mana you built up from the mana you have
        Rigidbody fireBallInstance;
        fireBallInstance = Instantiate(fireBallPrefab, magicHand.position, magicHand.rotation) as Rigidbody;
        fireBallInstance.AddForce(magicHand.forward * fireBallSpeed);
        fireBallInstance.transform.localScale = new Vector3(bigFireBall, bigFireBall, bigFireBall);
        fireBallInstance.GetComponent<FireBall>().manaCharge(magicBuildUp);
        gameObject.GetComponent<CombatCharacter>().DrainMana(magicBuildUp);
        mana -= magicBuildUp;
        magicBuildUp = 0;
        coolDownTimer = setTime;
    }

    public void MeleeAttack()
    {
        GameObject hitbox = GameObject.Find("HitBox");
        BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();
        
        boxCollider.enabled = true;
        attacking = true;
        collecting = false;
        blocking = false;
        Debug.Log("Attack");            
    }

    public void Blocking()
    {
        float DR;
        blocking = true;
        collecting = false;
        attacking = false;
        DR = maxMana / (mana * 10);
        gameObject.GetComponent<CombatCharacter>().SetDamageReduction(DR);
    }

    private void MeleeReset()
    {
        GameObject hitbox = GameObject.Find("HitBox");
        BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();

        boxCollider.enabled = false;
        attacking = false;
        attackTimer = 10;
    }

    public void RestoreMana(float award)
    {
        mana += award;

        if (mana > maxMana)
            mana = maxMana;
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
        if (!PauseManager.manager.isPaused)
        {
            //FireBall Scripting
            if (coolDownTimer == 0)
            {
                //builds up magic counter
                if (Input.GetButton("Fire1") && magicBuildUp < mana) {
                    leftArmAnim.SetBool("Charging", true);
                    magicBuildUp += .25f;
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    leftArmAnim.SetBool("Charging", false);
                    leftArmAnim.SetTrigger("Fireball");
                    rightArmAnim.SetTrigger("Fireball");
                    animating = false;
                }
                /*
                //checks if magic counter is greater than 10 if so triggers big FireBall
                if (Input.GetButtonUp("Fire1") && magicBuildUp >= 10) { RangedAttackBig(); }

                //if magic counter is less than 10 makes small FireBall            
                else if (Input.GetButtonUp("Fire1") && mana > 1) { RangedAttackSmall(); }
                */
            }
            else //this is subtracking from cool down timer         
            { coolDownTimer -= 1; }

            //Blocking
            //has time counter to ceck if you are wanting to block
            //if you are wanting to block than triggers code to start blocking
            if (Input.GetButton("Fire2"))
            {
                blockTimer += 1;

                if (blockTimer >= 30)
                {
                    Blocking();
                    Debug.Log("Blocking");
                }
            }
            //Melee Attack Scripting
            //checks to see if you are blocking if not than triggers a melee attack
            if (Input.GetButtonUp("Fire2"))
            {
                if (blocking == false)
                {
                    //MeleeAttack();
                    rightArmAnim.SetTrigger("Staff");
                    blockTimer = 0;
                }
                //if you are finished blocking than triggers code to allow to to attack again
                else if (blocking == true)
                {
                    blocking = false;
                    blockTimer = 0;
                }
            }

            //Collecting Mana from dead enemies
            if (Input.GetKeyDown("f"))
            {
                GameObject hitbox = GameObject.Find("HitBox");
                BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();

                if (collecting == false)
                {
                    Debug.Log("Collecting");
                    collecting = true;
                    attacking = false;
                    blocking = false;
                    boxCollider.enabled = true;
                }
            }

            if (Input.GetKeyUp("f"))
            {
                GameObject hitbox = GameObject.Find("HitBox");
                BoxCollider boxCollider = hitbox.GetComponent<BoxCollider>();

                if (collecting == true)
                {
                    Debug.Log("Not Collecting");
                    collecting = false;
                    attacking = false;
                    blocking = false;
                    boxCollider.enabled = false;
                }
            }

            //Melee Attack reset timer.
            if (attacking == true && attackTimer > 0)
            {
                attackTimer -= 1;
            }
            else if (attacking == true && attackTimer <= 0)
            {
                MeleeReset();
            }
        }
    }
}
