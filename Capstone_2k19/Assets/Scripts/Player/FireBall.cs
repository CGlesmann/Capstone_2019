using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float basicDMG = 30;
    private float manaBuildUp = 0;
    public float lifeTime = 10;
    
    public void manaCharge(float mana)
    {
        manaBuildUp = mana;
    }
    void Update()
    {
        if(lifeTime == 0)
            Destroy(gameObject);
        else
            lifeTime -= 1;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            
            if(gameObject.transform.localScale == new Vector3(1.5f,1.5f,1.5f))
            {
                float dmgMod = (manaBuildUp / 10) * basicDMG;
                col.gameObject.GetComponent<CombatCharacter>().TakeDamage(dmgMod);
                Destroy(gameObject);
            }

            if (gameObject.transform.localScale == new Vector3(.5f, .5f, .5f))
            {                
                col.gameObject.GetComponent<CombatCharacter>().TakeDamage(basicDMG);
                Destroy(gameObject);
            }           
        }
        else if (col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
