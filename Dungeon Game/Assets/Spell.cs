using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Spell Type")] //every spell type will have 1/2 aspects
    public bool Projectile = false; 
    //arc projectile(fire-rate): fast firing seeking arc balls (like wizard) || solar projectile(fire-rate): fireball || void projectile(charge): will spawn void balls and shoot them when release (like taken acolyte / syndra R) || stasis projectile(charge): burst of ice shards
    public bool Cone = false; 
    //arc cone(coofire-rateldown): swain q || solar cone(fire-rate): flamethrower || void cone(fire-rate): big knockback || stasis cone(fire-rate): slow/freezing cone
    public bool Ray = false; 
    //arc ray(fire-rate): thunder || solar ray: continous wide ray of bright solar energy ||void ray(fire-rate): charging beam (like brimstone) || stasis ray(fire-rate): tight freezing beam like aeger

    [Header("Spell Element")]
    public bool Arc = false; // will stun (will do more damage to slowed or frozen targets)
    public bool Solar = false; // will burn
    public bool Void = false; // will debuff
    public bool Stasis = false; // will freeze (enemies will take less damage when frozen, but will take big damage if the ice is broken)
    // frozen enemies will slowly unfreeze

    [Header("General")]
    [Header("Spell Atributes")]
    public float raw_damage;
    public float rate_of_fire;
    public float spell_size;

    [Header("Projectile")]
    public float projectile_speed; 

    [Header("Cone")]
    public float cone_width;

    [Header("Ray")]
    public float ray_unknonw;

    [Header("Arc")]
    public int Numver_of_chains;

    [Header("Solar")]
    public float burn_damage;

    [Header("Void")]
    public float debuff;

    [Header("Stasis")]
    public float freeze_modifier;

    [Header("Spells Prefabs")]
    public GameObject ArcProjectile;
    public GameObject ArcCone;
    public GameObject ArcRay;
    public GameObject SolarProjectile;
    public GameObject SolarCone;
    public GameObject SolarRay;
    public GameObject VoidProjectile;
    public GameObject VoidCone;
    public GameObject VoidRay;
    public GameObject StasisProjectile;
    public GameObject StasisCone;
    public GameObject StasisRay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell()
    {
        if(Projectile)
        {
            if(Arc)
            {
                CastArcProjectile();
            }
            if(Solar)
            {
                CastSolarProjectile();
            }
            if(Void)
            {
                CastVoidProjectile();
            }
            if(Stasis)
            {
                CastStasisProjectile();
            }
        }
        if(Cone)
        {
            if(Arc)
            {
                CastArcCone();
            }
            if(Solar)
            {   
                CastSolarCone();
            }
            if(Void)
            {
                CastVoidCone();
            }
            if(Stasis)
            {
                CastStasisCone();
            }
        }
        if(Ray)
        {
            if(Arc)
            {
                CastArcRay();
            }
            if(Solar)
            {
                CastSolarRay();
            }
            if(Void)
            {
                CastVoidRay();
            }
            if(Stasis)
            {
                CastStasisRay();
            }
        }
    }


    //PROJECTILES
    public void CastArcProjectile()
    {

    }
    public void CastSolarProjectile()
    {
        
    }
    public void CastVoidProjectile()
    {
        
    }
    public void CastStasisProjectile()
    {
        
    }

    //CONES
    public void CastArcCone()
    {

    }
    public void CastSolarCone()
    {
        
    }
    public void CastVoidCone()
    {
        
    }
    public void CastStasisCone()
    {
        
    }

    //RAYS
    public void CastArcRay()
    {

    }
    public void CastSolarRay()
    {
        
    }
    public void CastVoidRay()
    {
        
    }
    public void CastStasisRay()
    {
        
    }

}
