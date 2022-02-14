using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public Vector2 currentRoomCoords;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag.Equals("Ground"))
        {
            currentRoomCoords = new Vector2(collider2D.transform.position.x / 18, collider2D.transform.position.y / 10);
        }

        if(collider2D.tag.Equals("PassTop"))
        {
            // top player in the top room
        }
        if(collider2D.tag.Equals("PassRight"))
        {
            
        }
        if(collider2D.tag.Equals("PassBottom"))
        {
            
        }
        if(collider2D.tag.Equals("PassLeft"))
        {
            
        }
    }
}
