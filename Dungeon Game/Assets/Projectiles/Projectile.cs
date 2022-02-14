using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;

    public float kineticDmg;
    public float elementalDmg;
    // public float RoF;
    // public float manaCost;
    public float speed;
    public float range;
    public float size;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        transform.localScale = new Vector3(transform.localScale.x * size, transform.localScale.y * size, transform.localScale.z * size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag=="Enemy")
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            enemy.TakeDamage(kineticDmg);
            enemy.TakeDamage(elementalDmg);

            FireElement fireElement = gameObject.GetComponent<FireElement>();
            if(fireElement!=null)
            {
                enemy.Burn(fireElement.burn, fireElement.numberOfTics);
            }

            Destroy(gameObject);
        }
    }
}
