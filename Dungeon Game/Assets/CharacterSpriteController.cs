using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    public GameObject Character;
    public Sprite Front;
    public Sprite FrontLeft;
    public Sprite FrontRight;
    public Sprite Back;
    public Sprite BackLeft;
    public Sprite BackRight;

    //Sprite CharacterSprite;
    // public float speed;
    // Start is called before the first frame update
    void Start()
    {
        //CharacterSprite = Character.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        // float x = Input.GetAxis("Mouse X");
        // float y = Input.GetAxis("Mouse Y");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        float z = transform.rotation.eulerAngles.z;
        Debug.Log(z);
        if(z>0)
        {
            if(z<20)
            {
                Character.GetComponent<SpriteRenderer>().sprite = Back;
            }
            if(z<90 && z>=20)
            {
                Character.GetComponent<SpriteRenderer>().sprite = BackLeft;
            }
            if(z<160 && z>=90)
            {
                Character.GetComponent<SpriteRenderer>().sprite = FrontLeft;
            }
            if(z<200 && z>=160)
            {
                Character.GetComponent<SpriteRenderer>().sprite = Front;
            }
            if(z<270 && z>=200)
            {
                Character.GetComponent<SpriteRenderer>().sprite = FrontRight;
            }
            if(z<340 && z>=270)
            {
                Character.GetComponent<SpriteRenderer>().sprite = BackRight;
            }
            if(z<360 && z>=340)
            {
                Character.GetComponent<SpriteRenderer>().sprite = Back;
            }
        }

    }
}
