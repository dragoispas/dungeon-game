using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDetails : MonoBehaviour
{
    public Vector2 GridPosition;
    public bool hasTopDoor = false;
    public bool hasRightDoor = false;
    public bool hasBottomDoor = false;
    public bool hasLeftDoor = false;
    [Space]
    public float distanceFromStart;
    [Space]
    public bool isBossRoom = false;
    public bool isItemRoom = false;
    [Space]
    public bool isExtraRoom = false;
    [Space]
    public int path = 0;
    [Header("After Floor Generation is Done")]
    [Space]
    public Door TopDoor;
    public Door RightDoor;
    public Door BottomDoor;
    public Door LeftDoor;

    public bool doorsSet = false;

    // Start is called before the first frame update
    void Start()
    {
        GridPosition = FloorRoomsDetails.GetGridPosition(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(FloorRoomsDetails.FinishedGeneration && doorsSet == false)
        {
            if(!hasTopDoor)
            {
                TopDoor.setActive(false);
            }
            if(!hasRightDoor)
            {
                RightDoor.setActive(false);
            }
            if(!hasBottomDoor)
            {
                BottomDoor.setActive(false);
            }
            if(!hasLeftDoor)
            {
                LeftDoor.setActive(false);
            }
            doorsSet = true;
        }

    }

    public bool isIsolated()
    {
        return getNumberOfNeighbours() == 1;
    }

    public int getNumberOfNeighbours()
    {
        int x = 0;
        if(hasTopDoor)
        {
            x++;
        }
        if(hasRightDoor)
        {
            x++;
        }
        if(hasBottomDoor)
        {
            x++;
        }
        if(hasLeftDoor)
        {
            x++;
        }

        return x;
    }

    public bool hasOnlyTopAndBottomDoors()
    {
        bool value = false;
        if(hasTopDoor && hasBottomDoor && hasRightDoor == false && hasLeftDoor == false)
        {
            value = true;
        }
        return value;
    }

    public bool hasOnlyRightAndLeftDoors()
    {
        bool value = false;
        if(hasRightDoor && hasLeftDoor && hasTopDoor == false && hasBottomDoor == false)
        {
            value = true;
        }
        return value;
    }
}
