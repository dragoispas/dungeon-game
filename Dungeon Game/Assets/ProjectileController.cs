using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour // MAKE A SCRIPT TO ATTACH TO THE PROJECTILE ITSELF WITH ALL THE DETAILS
{
    public GameObject ProjectileObject;
    private float nextTimeToFire = 0f;
    private float arcAlternator = -0.5f;

    [Header("General")]
    public Sprite sprite;
    public float kineticDmg = 1f;
    public float elementalDmg = 1f;
    public float RoF = 15f;
    public float manaCost = 20f;
    public float speed = 10f;
    public float range;
    public float size;

    private Sprite current_sprite;
    private string current_element;
    private float current_kineticDmg;
    private float current_elementalDmg;
    private float current_RoF;
    private float current_manaCost;
    private float current_speed;
    private float current_range;
    private float current_size;

    [Header("Fire")]
    public bool Fire;
    public Sprite fire_projectile_model;
    public float burn;
    public float explosion;
    private float fire_kineticDmg_modifier = 2f;
    private float fire_elementalDmg_modifier = 2f;
    private float fire_RoF_modifier = 0.75f;
    private float fire_manaCost_modifier = 1.5f;
    private float fire_speed_modifier = 0.75f;
    private float fire_range_modifier = 1f;
    private float fire_size_modifier = 1.25f;


    [Header("Arc")]
    public bool Arc;
    public Sprite arc_projectile_model;
    public int chain;
    public float stun;
    private float arc_kineticDmg_modifier = 0.25f;
    private float arc_elementalDmg_modifier = 1.25f;
    private float arc_RoF_modifier = 1.5f;
    private float arc_manaCost_modifier = 0.5f;
    private float arc_speed_modifier = 0.75f;
    private float arc_range_modifier = 1f;
    private float arc_size_modifier = 0.5f;


    [Header("Freeze")]
    public bool Freeze;
    public Sprite freeze_projectile_model;
    public float slow;
    public float shatter; //will work similar to explosion, but on frozen enemy death
    private float freeze_kineticDmg_modifier = 1.5f;
    private float freeze_elementalDmg_modifier = 0.5f;
    private float freeze_RoF_modifier = 1f;
    private float freeze_manaCost_modifier = 1f;
    private float freeze_speed_modifier = 1.5f;
    private float freeze_range_modifier = 1.5f;
    private float freeze_size_modifier = 0.75f;


    [Header("Void")]
    public bool Void;
    public Sprite void_projectile_model;
    public float debuff;
    public float knockback;
    private float void_kineticDmg_modifier = 1f;
    private float void_elementalDmg_modifier = 1f;
    private float void_RoF_modifier = 1f;
    private float void_manaCost_modifier = 1f;
    private float void_speed_modifier = 1f;
    private float void_range_modifier = 1.25f;
    private float void_size_modifier = 1f;


    // Start is called before the first frame update
    void Start()
    {
        //size = ProjectileObject.transform.localScale.x;
        sprite = ProjectileObject.GetComponent<SpriteRenderer>().sprite;
        current_sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(Fire)
        {
            current_sprite = fire_projectile_model;
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
            current_sprite = arc_projectile_model;
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
            current_sprite = freeze_projectile_model;
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
            current_sprite = void_projectile_model;
            current_kineticDmg = kineticDmg * void_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * void_elementalDmg_modifier;
            current_RoF = RoF * void_RoF_modifier;
            current_manaCost = manaCost * void_manaCost_modifier;
            current_speed = speed * void_speed_modifier;
            current_range = range * void_range_modifier;
            current_size = size * void_size_modifier;
        }
        // if(!(Fire || Arc || Freeze || Void))
        // {
        //     current_kineticDmg = kineticDmg;
        //     current_elementalDmg = elementalDmg;
        //     current_RoF = RoF;
        //     current_manaCost = manaCost;
        //     current_speed = speed;
        //     current_range = range;
        //     current_size = size;
        // }

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1/current_RoF;
            Shoot();
        }

    }

    public void Shoot()
    {
        if(Fire)
        {
            ShootGeneric();
        }
        if(Arc)
        {
            ShootArc();
        }
        if(Void)
        {
            ShootGeneric();
        }
        if(Freeze)
        {
            ShootFreeze();
        }
    }

    public void ShootGeneric()
    {
        GameObject newProjectile = Instantiate(ProjectileObject, transform.position, Quaternion.Euler(transform.eulerAngles));

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Element = current_element;
        projectile.sprite = current_sprite;
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = current_size;
    }

    public void ShootArc()
    {

        //Vector3 newPosition = new Vector3(transform.parent.localPosition.x + arcAlternator * arc_size_modifier, transform.parent.localPosition.y, transform.parent.localPosition.z);
        Vector3 newPosition = transform.forward + transform.right * arcAlternator * arc_size_modifier + transform.position;
        arcAlternator *= -1;
        GameObject newProjectile = Instantiate(ProjectileObject, newPosition, Quaternion.Euler(transform.eulerAngles));


        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Element = current_element;
        projectile.sprite = current_sprite;
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = current_size;
    }

    public void ShootFreeze()
    {
        GameObject newProjectile = Instantiate(ProjectileObject, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0,0,Random.Range(-5f, 5f))));

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Element = current_element;
        projectile.sprite = current_sprite;
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = current_size;
    }
}
