using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoomDetails : MonoBehaviour
{
    public bool hasDoorTopLeft;
    public bool hasDoorTopRight;
    public bool hasDoorRightTop;
    public bool hasDoorRightBottom;
    public bool hasDoorBottomRight;
    public bool hasDoorBottomLeft;
    public bool hasDoorLeftBottom;
    public bool hasDoorLeftTop;

    [Space]
    public Door TopLeftDoor;
    public Door TopRightDoor;
    public Door RightTopDoor;
    public Door RightBottomDoor;
    public Door BottomRightDoor;
    public Door BottomLeftDoor;
    public Door LeftBottomDoor;
    public Door LeftTopDoor;

    public bool doorsSet = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FloorRoomsDetails.FinishedGeneration && doorsSet == false)
        {
            if(!hasDoorTopLeft)
            {
                TopLeftDoor.setActive(false);
            }
            if(!hasDoorTopRight)
            {
                TopRightDoor.setActive(false);
            }
            if(!hasDoorRightTop)
            {
                RightTopDoor.setActive(false);
            }
            if(!hasDoorRightBottom)
            {
                RightBottomDoor.setActive(false);
            }
            if(!hasDoorBottomRight)
            {
                BottomRightDoor.setActive(false);
            }
            if(!hasDoorBottomLeft)
            {
                BottomLeftDoor.setActive(false);
            }
            if(!hasDoorLeftBottom)
            {
                LeftBottomDoor.setActive(false);
            }
            if(!hasDoorLeftTop)
            {
                LeftTopDoor.setActive(false);
            }
            doorsSet = true;
        }
    }
}
