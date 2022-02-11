using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public Transform LeftHand;
    public Transform RightHand;
    private Spells leftHandSpells;
    private Spells rightHandSpells;
    private bool shootLeftHand = false;
    private bool shootRightHande = false;

    // Start is called before the first frame update
    void Start()
    {
        leftHandSpells = LeftHand.GetComponent<Spells>();
        rightHandSpells = RightHand.GetComponent<Spells>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootLeftHand = true;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            shootRightHande = true;
        }
    }

    void FixedUpdate()
    {
        if(shootLeftHand)
        {
            ShootLeftHand();
        }
    }

    void ShootLeftHand()
    {
        // GameObject newFireball = Instantiate(Fireball, hand.transform.position, Quaternion.Euler(hand.transform.rotation.eulerAngles));
        // Rigidbody2D rb = newFireball.GetComponent<Rigidbody2D>();
        // rb.velocity = transform.up * FireballSpeed;
        // shootFireball = false;
    }
}
