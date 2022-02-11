using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public List<int> SpellList;
    public GameObject Fireball;
    public float Fireball_Speed;
    public float Firball_Cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastActive()
    {

    }

    public void CastFireball()
    {
        GameObject newFireball = Instantiate(Fireball, transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        Rigidbody2D rb = newFireball.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * Fireball_Speed;
    }
}
