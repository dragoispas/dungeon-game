using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isActive = true;
    public GameObject DoorSwitch;
    public GameObject Passthrough;
    public GameObject BasicWall;
    public GameObject DoorSprite;
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
        DoorSwitch.SetActive(setting);
        Passthrough.SetActive(setting);
        DoorSprite.SetActive(setting);
        BasicWall.SetActive(!setting);
        isActive = setting;
    }

    public void openDoor()
    {
        if(!isActive)
        {
            return;
        }
        BoxCollider2D boxCollider2D = DoorSwitch.GetComponent<BoxCollider2D>();
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
        BoxCollider2D boxCollider2D = DoorSwitch.GetComponent<BoxCollider2D>();
        if(boxCollider2D==null)
        {
            return;
        }
        boxCollider2D.enabled = true;
    }
}
