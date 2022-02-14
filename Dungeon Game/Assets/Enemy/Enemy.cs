using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Hp = 100;
    public string Element;

    public float burnDamage;
    public bool burning = false;
    float nextTic = 0f;
    int numberOfTics = 0;


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
        Hp-=damageTaken;

        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("revertColor", 0.1f);

    }

    public void Burn(float burn, int tics)
    {
        burning = true;
        burnDamage = burn;
        numberOfTics = tics;
    }

    void revertColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

}
