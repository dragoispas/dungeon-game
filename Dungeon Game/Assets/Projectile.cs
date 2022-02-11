using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour // MAKE A SCRIPT TO ATTACH TO THE PROJECTILE ITSELF WITH ALL THE DETAILS
{
    public GameObject ProjectileObject;

    [Header("General")]
    public float kineticDmg = 1f;
    public float elementalDmg = 1f;
    public float RoF = 1f;
    public float manaCost = 20f;
    public float speed;
    public float range;
    public float size;

    private float current_kineticDmg = 1f;
    private float current_elementalDmg = 1f;
    private float current_RoF = 1f;
    private float current_manaCost = 20f;
    private float current_speed;
    private float current_range;
    private float current_size;

    [Header("Fire")]
    public bool Fire;
    public float burn;
    public float explosion;

    public float fire_kineticDmg_modifier = 2f;
    public float fire_elementalDmg_modifier = 2f;
    public float fire_RoF_modifier = 0.75f;
    public float fire_manaCost_modifier = 1.5f;
    public float fire_speed_modifier;
    public float fire_range_modifier;
    public float fire_size_modifier;


    [Header("Arc")]
    public bool Arc;
    public int chain;
    public float stun;
    public float arc_kineticDmg_modifier = 0.25f;
    public float arc_elementalDmg_modifier = 1.25f;
    public float arc_RoF_modifier = 1.5f;
    public float arc_manaCost_modifier = 0.5f;
    public float arc_speed_modifier;
    public float arc_range_modifier;
    public float arc_size_modifier;


    [Header("Freeze")]
    public bool Freeze;
    public float slow;
    public float shatter; //will work similar to explosion, but on frozen enemy death
    public float freeze_kineticDmg_modifier = 1.5f;
    public float freeze_elementalDmg_modifier = 0.5f;
    public float freeze_RoF_modifier = 1f;
    public float freeze_manaCost_modifier;
    public float freeze_speed_modifier;
    public float freeze_range_modifier;
    public float freeze_size_modifier;


    [Header("Void")]
    public bool Void;
    public float debuff;
    public float knockback;
    public float void_kineticDmg_modifier = 1f;
    public float void_elementalDmg_modifier = 1f;
    public float void_RoF_modifier = 1f;
    public float void_manaCost_modifier;
    public float void_speed_modifier;
    public float void_range_modifier;
    public float void_size_modifier;


    // Start is called before the first frame update
    void Start()
    {
        size = ProjectileObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(Fire)
        {
            current_kineticDmg = kineticDmg * fire_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * fire_elementalDmg_modifier;
            current_RoF = RoF * fire_RoF_modifier;
            current_manaCost = manaCost * fire_manaCost_modifier;
            current_speed = speed * fire_speed_modifier;
            current_range = range * fire_range_modifier;
            current_size = size * fire_size_modifier;
        }
        if(Arc)
        {
            current_kineticDmg = kineticDmg * arc_kineticDmg_modifier;
            current_elementalDmg *= arc_elementalDmg_modifier;
            current_RoF = RoF * arc_RoF_modifier;
            current_manaCost = manaCost * arc_manaCost_modifier;
            current_speed = speed * arc_speed_modifier;
            current_range = range * arc_range_modifier;
            current_size = size * arc_size_modifier;
        }
        if(Freeze)
        {
            current_kineticDmg = kineticDmg * freeze_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * freeze_elementalDmg_modifier;
            current_RoF = RoF * freeze_RoF_modifier;
            current_manaCost = manaCost * freeze_manaCost_modifier;
            current_speed = speed * freeze_speed_modifier;
            current_range = range * freeze_range_modifier;
            current_size = size * freeze_size_modifier;
        }
        if(Void)
        {
            current_kineticDmg = kineticDmg * void_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * void_elementalDmg_modifier;
            current_RoF = RoF * void_RoF_modifier;
            current_manaCost = manaCost * void_manaCost_modifier;
            current_speed = speed * void_speed_modifier;
            current_range = range * void_range_modifier;
            current_size = size * void_size_modifier;
        }
        if(!(Fire || Arc || Freeze || Void))
        {
            current_kineticDmg = kineticDmg;
            current_elementalDmg = elementalDmg;
            current_RoF = RoF;
            current_manaCost = manaCost;
            current_speed = speed;
            current_range = range;
            current_size = size;
        }
    }

    public void shoot()
    {
        GameObject newProjectile = Instantiate(ProjectileObject, transform.position, Quaternion.Euler(transform.eulerAngles));
        Rigidbody2D rb = ProjectileObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * current_speed;
    }
}
