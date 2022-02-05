using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public Vector2 currentRoomCoords;
    public RoomControl roomControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roomControl!=null)
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                roomControl.openDoors();
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                roomControl.closeDoors();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.tag.Equals("Ground"))
        {
            roomControl = collider2D.transform.parent.gameObject.GetComponent<RoomControl>();
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
