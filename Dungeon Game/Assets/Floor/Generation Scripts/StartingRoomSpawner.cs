using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoomSpawner : MonoBehaviour
{
    FloorRoomsDetails floorRoomsDetails;

    public GameObject NewRoom;
    [Space]
    public RoomDetails roomDetails;



    bool SpawnedRooms = false;
    // Start is called before the first frame update
    void Start()
    {
        floorRoomsDetails = FindObjectOfType<FloorRoomsDetails>();
        floorRoomsDetails.AddRoomToList(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnedRooms)
        {
            return;
        }

        int x = Random.Range(1,5);
        int xCopy = x;

        SpawnRoom(x);
        while(x==xCopy)
        {
            x = Random.Range(1,5);
        }
        SpawnRoom(x);

        int xCopy2 = x;

        if(50 < Random.Range(0,100))
        {
            while(x==xCopy || x==xCopy2)
            {
                x = Random.Range(1,5);
            }
            SpawnRoom(x);
        }
        SpawnedRooms = true;
    }

    void spawnTopRoom()
    {
        Vector3 newRoomPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        roomDetails.hasTopDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasBottomDoor = true;
            newRoomDetails.distanceFromStart = 1;
            newRoomDetails.path = 1;
        }
    }
    void spawnRightRoom()
    {
        Vector3 newRoomPosition = new Vector3(transform.position.x + 18f, transform.position.y, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        roomDetails.hasRightDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasLeftDoor = true;
            newRoomDetails.distanceFromStart = 1;
            newRoomDetails.path = 2;
        }
    }
    void spawnBottomRoom()
    {
        // Create the Spawn position and instantiate the room there
        Vector3 newRoomPosition = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);

        // Add the new room to the list of all rooms and uptade the number of rooms that need to be spawned
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        // Update Current Room Details
        roomDetails.hasBottomDoor = true;

        // Give the new room a door where it must have it
        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasTopDoor = true;
            newRoomDetails.distanceFromStart = 1;
            newRoomDetails.path = 3;
        }
    }
    void spawnLeftRoom()
    {
        Vector3 newRoomPosition = new Vector3(transform.position.x - 18f, transform.position.y, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        roomDetails.hasLeftDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasRightDoor = true;
            newRoomDetails.distanceFromStart = 1;
            newRoomDetails.path = 4;
        }

    }

    void SpawnRoom(int x)
    {
        if(x==1)
        {
            spawnTopRoom();
        }
        else if(x==2)
        {
            spawnRightRoom();
        }
        else if(x==3)
        {
            spawnBottomRoom();
        }
        else if(x==4)
        {
            spawnLeftRoom();
        }
    }
}
