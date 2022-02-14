using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    public RoomDetails roomDetails;
    public BigRoomDetails bigRoomDetails;
    public Door[] Doors;

    bool doorsAddedToList = false;

    // Start is called before the first frame update
    void Start()
    {
        doorsAddedToList = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(!doorsAddedToList)
        {
            if(roomDetails!=null && FloorRoomsDetails.FinishedGeneration)
            {
                if(roomDetails.doorsSet)
                {
                    Doors = transform.GetComponentsInChildren<Door>();
                }
                doorsAddedToList = true;
            }
            
            if(bigRoomDetails!=null && FloorRoomsDetails.FinishedGeneration)
            {
                if(bigRoomDetails.doorsSet)
                {
                    Doors = transform.GetComponentsInChildren<Door>();
                }
                doorsAddedToList = true;
            }

            if(roomDetails == null && bigRoomDetails == null && FloorRoomsDetails.FinishedGeneration)
            {
                Doors = transform.GetComponentsInChildren<Door>();

                doorsAddedToList = true;
            }
        }
        

        if(!doorsAddedToList)
        {
            return;
        }
    }

    public void closeDoors()
    {
        for(int i=0 ; i<Doors.Length ; i++)
        {
            if(Doors[i].isActive)
            {
                Doors[i].closeDoor();
            }
        }
    }
    public void openDoors()
    {
        for(int i=0 ; i<Doors.Length ; i++)
        {
            if(Doors[i].isActive)
            {
                Doors[i].openDoor();
            }
            
        }
    }

}
