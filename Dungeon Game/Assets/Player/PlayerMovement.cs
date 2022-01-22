using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    PlayerDetails playerDetails;

    Vector2 movement;

    public CameraFollow cameraFollow;

    void Start()
    {
        playerDetails = GetComponent<PlayerDetails>();
        //CameraTransform = GameObject.FindGameObjectWithTag("Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //gives 1 or -1
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(!CameraFollow.Transitioning)
        {
            rb.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "PassTop")
        {
            rb.velocity = Vector3.zero;
            Vector3 newPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + 3f, transform.position.z);
            transform.position = newPosition;

            cameraFollow.moveCameraTop();

        }
        if(collider2D.tag == "PassRight")
        {
            rb.velocity = Vector3.zero;
            Vector3 newPosition = new Vector3(Mathf.RoundToInt(transform.position.x) + 3f, Mathf.RoundToInt(transform.position.y), transform.position.z);
            transform.position = newPosition;

           cameraFollow.moveCameraRight();
        }
        if(collider2D.tag == "PassBottom")
        {
            rb.velocity = Vector3.zero;
            Vector3 newPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 3f, transform.position.z);
            transform.position = newPosition;

            cameraFollow.moveCameraBottom();
        }
        if(collider2D.tag == "PassLeft")
        {
            rb.velocity = Vector3.zero;
            Vector3 newPosition = new Vector3(Mathf.RoundToInt(transform.position.x) - 3f, Mathf.RoundToInt(transform.position.y), transform.position.z);
            transform.position = newPosition;

            cameraFollow.moveCameraLeft();
        }

    }

}
