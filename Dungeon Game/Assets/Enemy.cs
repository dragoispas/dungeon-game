using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Hp = 100;
    public string Element;


    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageTaken)
    {
        Hp-=damageTaken;

        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("revertColor", 0.1f);

    }

    void revertColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

}
