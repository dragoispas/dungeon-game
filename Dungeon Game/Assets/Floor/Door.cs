using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isActive = true;
    public GameObject DoorBody;
    public GameObject PassThrough;
    public GameObject BasicWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setActive(bool setting)
    {
        DoorBody.SetActive(setting);
        PassThrough.SetActive(setting);
        BasicWall.SetActive(!setting);
        isActive = setting;
    }

    public void openDoor()
    {
        if(!isActive)
        {
            return;
        }
        BoxCollider2D boxCollider2D = DoorBody.GetComponent<BoxCollider2D>();
        if(boxCollider2D==null)
        {
            return;
        }
        boxCollider2D.enabled = false;
    }

    public void closeDoor()
    {
        if(!isActive)
        {
            return;
        }
        BoxCollider2D boxCollider2D = DoorBody.GetComponent<BoxCollider2D>();
        if(boxCollider2D==null)
        {
            return;
        }
        boxCollider2D.enabled = true;
    }
}
