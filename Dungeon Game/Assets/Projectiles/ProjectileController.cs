using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour // MAKE A SCRIPT TO ATTACH TO THE PROJECTILE ITSELF WITH ALL THE DETAILS
{
    public GameObject FireProjectile;
    public GameObject ArcProjectile;
    public GameObject FreezeProjectile;
    public GameObject VoidProjectile;

    private float nextTimeToFire = 0f;
    private float arcAlternator = -0.2f;

    [Header("General")]
    public float kineticDmg = 1f;
    public float elementalDmg = 1f;
    public float RoF = 15f;
    public float manaCost = 20f;
    public float speed = 10f;
    public float range;
    public float size_modifier;

    private float current_kineticDmg;
    private float current_elementalDmg;
    private float current_RoF;
    private float current_manaCost;
    private float current_speed;
    private float current_range;

    [Header("Fire")]
    public bool Fire;
    public float burn;
    public int numberOfTics;
    public float explosion;
    private float fire_kineticDmg_modifier = 2f;
    private float fire_elementalDmg_modifier = 2f;
    private float fire_RoF_modifier = 0.75f;
    private float fire_manaCost_modifier = 1.5f;
    private float fire_speed_modifier = 0.75f;
    private float fire_range_modifier = 1f;


    [Header("Arc")]
    public bool Arc;
    public int chain;
    public float chainDamage;
    public float chainRadius;
    public float stun;
    private float arc_kineticDmg_modifier = 0.25f;
    private float arc_elementalDmg_modifier = 1.25f;
    private float arc_RoF_modifier = 1.25f;
    private float arc_manaCost_modifier = 0.5f;
    private float arc_speed_modifier = 0.75f;
    private float arc_range_modifier = 1f;


    [Header("Freeze")]
    public bool Freeze;
    public float slow;
    public float shatter; //will work similar to explosion, but on frozen enemy death
    private float freeze_kineticDmg_modifier = 1.5f;
    private float freeze_elementalDmg_modifier = 0.5f;
    private float freeze_RoF_modifier = 1f;
    private float freeze_manaCost_modifier = 1f;
    private float freeze_speed_modifier = 1.5f;
    private float freeze_range_modifier = 1.5f;


    [Header("Void")]
    public bool Void;
    public float debuff;
    public float maxDebuff;
    public float debuffDuration;
    public float knockback;
    private float void_kineticDmg_modifier = 1f;
    private float void_elementalDmg_modifier = 1f;
    private float void_RoF_modifier = 1f;
    private float void_manaCost_modifier = 1f;
    private float void_speed_modifier = 1f;
    private float void_range_modifier = 1.25f;


    // Start is called before the first frame update
    void Start()
    {

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
        }
        if(Arc)
        {
            current_kineticDmg = kineticDmg * arc_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * arc_elementalDmg_modifier;
            current_RoF = RoF * arc_RoF_modifier;
            current_manaCost = manaCost * arc_manaCost_modifier;
            current_speed = speed * arc_speed_modifier;
            current_range = range * arc_range_modifier;
        }
        if(Freeze)
        {
            current_kineticDmg = kineticDmg * freeze_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * freeze_elementalDmg_modifier;
            current_RoF = RoF * freeze_RoF_modifier;
            current_manaCost = manaCost * freeze_manaCost_modifier;
            current_speed = speed * freeze_speed_modifier;
            current_range = range * freeze_range_modifier;
        }
        if(Void)
        {
            current_kineticDmg = kineticDmg * void_kineticDmg_modifier;
            current_elementalDmg = elementalDmg * void_elementalDmg_modifier;
            current_RoF = RoF * void_RoF_modifier;
            current_manaCost = manaCost * void_manaCost_modifier;
            current_speed = speed * void_speed_modifier;
            current_range = range * void_range_modifier;
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fire = true;
            Arc = false;
            Freeze = false;
            Void = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fire = false;
            Arc = true;
            Freeze = false;
            Void = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Fire = false;
            Arc = false;
            Freeze = true;
            Void = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Fire = false;
            Arc = false;
            Freeze = false;
            Void = true;
        }

    }

    public void Shoot()
    {
        if(Fire)
        {
            ShootFire();
        }
        if(Arc)
        {
            ShootArc();
        }
        if(Void)
        {
            ShootVoid();
        }
        if(Freeze)
        {
            ShootFreeze();
        }
    }

    public void ShootFire()
    {
        GameObject newProjectile = Instantiate(FireProjectile, transform.position, Quaternion.Euler(transform.eulerAngles));
        newProjectile.GetComponent<Rigidbody2D>().mass *= fire_kineticDmg_modifier;

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = size_modifier;

        FireElement fireElement = newProjectile.GetComponent<FireElement>();
        fireElement.burn = burn * elementalDmg / 5F;
        fireElement.numberOfTics = numberOfTics;
        fireElement.explosion = explosion;
    }

    public void ShootArc()
    {

        //Vector3 newPosition = new Vector3(transform.parent.localPosition.x + arcAlternator * arc_size_modifier, transform.parent.localPosition.y, transform.parent.localPosition.z);
        Vector3 newPosition = transform.forward + transform.right * arcAlternator * size_modifier + transform.position;
        arcAlternator *= -1;
        GameObject newProjectile = Instantiate(ArcProjectile, newPosition, Quaternion.Euler(transform.eulerAngles));
        newProjectile.GetComponent<Rigidbody2D>().mass *= arc_kineticDmg_modifier;


        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = size_modifier;

        ArcElement arcElement = newProjectile.GetComponent<ArcElement>();
        arcElement.chain = chain;
        arcElement.chainDmg = chainDamage * elementalDmg / 5f;
        arcElement.chainRadius = chainRadius;
        arcElement.stun = stun;
    }

    public void ShootVoid()
    {
        GameObject newProjectile = Instantiate(VoidProjectile, transform.position, Quaternion.Euler(transform.eulerAngles));
        newProjectile.GetComponent<Rigidbody2D>().mass *= void_kineticDmg_modifier;

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = size_modifier;

        VoidElement voidElement = newProjectile.GetComponent<VoidElement>();
        voidElement.debuff = debuff;
        voidElement.maxDebuff = maxDebuff;
        voidElement.knockback = knockback;
        voidElement.debuffDuration = debuffDuration;
    }

    public void ShootFreeze()
    {
        GameObject newProjectile = Instantiate(FreezeProjectile, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0,0,Random.Range(-5f, 5f))));
        newProjectile.GetComponent<Rigidbody2D>().mass *= freeze_kineticDmg_modifier;

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.kineticDmg = current_kineticDmg;
        projectile.elementalDmg = current_elementalDmg;
        // projectile.RoF = current_RoF;
        // projectile.manaCost = current_manaCost;
        projectile.speed = current_speed;
        projectile.range = current_range;
        projectile.size = size_modifier;

        FreezeElement freezeElement = newProjectile.GetComponent<FreezeElement>();
        freezeElement.slow = slow;
        freezeElement.shatter = shatter;
    }
}
