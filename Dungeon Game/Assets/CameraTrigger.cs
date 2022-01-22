using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public static int xCol = 0;
    public static int yCol = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        Physics2D.IgnoreCollision(collision2D.collider, GetComponent<BoxCollider2D>());
    }
    void OnCollisionStay2D(Collision2D collision2D)
    {
        Physics2D.IgnoreCollision(collision2D.collider, GetComponent<BoxCollider2D>());
    }
    void OnCollisionExit2D(Collision2D collision2D)
    {
        Physics2D.IgnoreCollision(collision2D.collider, GetComponent<BoxCollider2D>());
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "FollowPlayerX")
        {
            xCol++;
        }
        if(collider2D.tag == "FollowPlayerY")
        {
            yCol++;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag == "FollowPlayerX")
        {
            xCol--;
        }
        if(collider2D.tag == "FollowPlayerY")
        {
            yCol--;
        }
    }
}
