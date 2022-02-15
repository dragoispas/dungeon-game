using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Hp = 100;
    public string Element;

    // ~~~~ Fire Damage
    public float burnDamage;
    public bool burning = false;
    float nextTic = 0f;
    int numberOfTics = 0;
    // ~~~~

    // ~~~~ Void Damage
    public float debuff = 1F;
    public float maxDebuff = 1.5F;
    // ~~~~

    
    public bool isChaining = false;

    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        














        // ~~~~~~~~~~ BURN DAMAGE
        if(burning && Time.time >= nextTic)
        {
            if(numberOfTics == 0)
            {
                burning = false;
            }
            nextTic = Time.time + 1/5f;
            TakeDamage(burnDamage);
            numberOfTics--;
            if(numberOfTics == 0)
            {
                burning = false;
            }
        }
        // ~~~~~~~~~~~
    }

    public void TakeDamage(float damageTaken)
    {
        Hp-=damageTaken * debuff;

        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("revertColor", 0.1f);

    }

    public void Burn(float burn, int tics)
    {
        burning = true;
        burnDamage = burn;
        numberOfTics = tics;
    }

    public void Chain(int chains, float dmg, float chainRadius)
    {
        // TakeDamage(dmg);
        // isChaining = true;
        // GameObject[] enemies= GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    public void Debuff(float debuff, float maxDebuff, float debuffDuration)
    {
        CancelInvoke("RemoveDebuff");
        this.maxDebuff = maxDebuff;
        if(this.debuff + debuff <= this.maxDebuff)
        {
            this.debuff += debuff;
        }

        Invoke("RemoveDebuff", debuffDuration);

        // make damage numbers bigger
    }

    void RemoveDebuff()
    {
        debuff = 1;
    }

    // TODOOOOOOOOOOOOOOOOOOOO
    public void Slow()
    {

    }

    void revertColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

}
