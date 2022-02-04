using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPassthrough : MonoBehaviour
{
    public string Direction; 
    // Start is called before the first frame update
    void Start()
    {
        float z = transform.parent.gameObject.transform.rotation.eulerAngles.z;
        if(z == 0f)
        {
            Direction = "TOP";
        }
        if(z == 90f)
        {
            Direction = "LEFT";
        }
        if(z == 180f)
        {
            Direction = "BOTTOM";
        }
        if(z == 270f)
        {
            Direction = "RIGHT";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
