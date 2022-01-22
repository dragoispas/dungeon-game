using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    // public float smoothSpeed = 0.125f;
    public static bool isFollowingX = false;
    public static bool isFollowingY = false;

    public bool x;
    public bool y;

    float TransitionSpeed = 0.4f;
    public float HorizontalTransitionSpeed = 0.55f;
    public float VerticalTransitionSpeed = 0.4f;

    Vector3 newPosition;
    bool moved = true;
    public static bool Transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()   // MAKE IT SO THAT L ROOM CANNOT SPAWN WITH A ROOM IN THE MISSING CORNER
    {
        if(moved == false)
        {
            if(transform.position!= newPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, TransitionSpeed);
            }
            else
            {
                Transitioning = false;
                moved = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(CameraTrigger.xCol > 0)
        {
            Vector3 targetX = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = targetX;
            x = true;
        }
        else
        {
            x = false;
        }
        if(CameraTrigger.yCol > 0)
        {
            Vector3 targetY = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = targetY;
            y = true;
        }
        else
        {
            y = false;
        }
    }


    public void moveCameraTop()
    {
        Vector3 newCameraPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 10f), transform.position.z);
        newPosition = newCameraPosition;
        moved = false;
        Transitioning = true;
        TransitionSpeed = VerticalTransitionSpeed;

        //transform.position = newCameraPosition;
    }
    public void moveCameraRight()
    {
        Vector3 newCameraPosition = new Vector3(Mathf.RoundToInt(transform.position.x + 18f), Mathf.RoundToInt(transform.position.y), transform.position.z);
        newPosition = newCameraPosition;
        moved = false;
        Transitioning = true;
        TransitionSpeed = HorizontalTransitionSpeed;

        //transform.position = newCameraPosition;
    }
    public void moveCameraBottom()
    {
        Vector3 newCameraPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 10f), transform.position.z);
        newPosition = newCameraPosition;
        moved = false;
        Transitioning = true;
        TransitionSpeed = VerticalTransitionSpeed;

        //transform.position = newCameraPosition;
    }
    public void moveCameraLeft()
    {
        Vector3 newCameraPosition = new Vector3(Mathf.RoundToInt(transform.position.x - 18f), Mathf.RoundToInt(transform.position.y), transform.position.z);
        newPosition = newCameraPosition;
        moved = false;
        Transitioning = true;
        TransitionSpeed = HorizontalTransitionSpeed;

        //transform.position = newCameraPosition;
    }

    
}
