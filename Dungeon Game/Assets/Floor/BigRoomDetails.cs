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
            else
            {
                TopLeftDoor.setActive(true);
            }

            if(!hasDoorTopRight)
            {
                TopRightDoor.setActive(false);
            }
            else
            {
                TopRightDoor.setActive(true);
            }

            if(!hasDoorRightTop)
            {
                RightTopDoor.setActive(false);
            }
            else
            {
                RightTopDoor.setActive(true);
            }

            if(!hasDoorRightBottom)
            {
                RightBottomDoor.setActive(false);
            }
            else
            {
                RightBottomDoor.setActive(true);
            }

            if(!hasDoorBottomRight)
            {
                BottomRightDoor.setActive(false);
            }
            else
            {
                BottomRightDoor.setActive(true);
            }

            if(!hasDoorBottomLeft)
            {
                BottomLeftDoor.setActive(false);
            }
            else
            {
                BottomLeftDoor.setActive(true);
            }

            if(!hasDoorLeftBottom)
            {
                LeftBottomDoor.setActive(false);
            }
            else
            {
                LeftBottomDoor.setActive(true);
            }

            if(!hasDoorLeftTop)
            {
                LeftTopDoor.setActive(false);
            }
            else
            {
                LeftTopDoor.setActive(true);
            }

            doorsSet = true;
        }
    }
}
