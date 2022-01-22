using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    FloorRoomsDetails floorRoomsDetails;
    public RoomDetails roomDetails;

    public GameObject NewRoom;
    [Space]
    public GameObject TopDoor;
    public GameObject RightDoor;
    public GameObject BottomDoor;
    public GameObject LeftDoor;
    [Space]
    public int chanceToSpawnTop = 50;
    public int chanceToSpawnRight = 50;
    public int chanceToSpawnBottom = 50;
    public int chanceToSpawnLeft = 50;

    [HideInInspector]
    public bool SpawnedRooms = false;
    // Start is called before the first frame update
    void Start()
    {
        floorRoomsDetails = FindObjectOfType<FloorRoomsDetails>();
        if(GetComponent<RoomDetails>() != null)
        {
            roomDetails = GetComponent<RoomDetails>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnedRooms)
        {
            return;
        }

        if(Chance(chanceToSpawnTop))
        {
            if(CanSpawnTop() && floorRoomsDetails.RoomsToGenerate > 0)
            {
                spawnTopRoom();
            }
        }

        if(Chance(chanceToSpawnRight))
        {
            if(CanSpawnRight() && floorRoomsDetails.RoomsToGenerate > 0)
            {
                spawnRightRoom();
            }
        }
        
        if(Chance(chanceToSpawnBottom))
        {
            if(CanSpawnBottom() && floorRoomsDetails.RoomsToGenerate > 0)
            {
                spawnBottomRoom();
            }
        }

        if(Chance(chanceToSpawnLeft))
        {
            if(CanSpawnLeft() && floorRoomsDetails.RoomsToGenerate > 0)
            {
                spawnLeftRoom();
            }
        }

        

        SpawnedRooms = true;
    }

    void spawnTopRoom()
    {
        if(Mathf.Abs(transform.position.y + 10f) / 10 > floorRoomsDetails.MaxHeightOrLength)
        {
            return;
        }
        Vector3 newRoomPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        newRoom.GetComponent<RoomSpawner>().chanceToSpawnTop = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnRight = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnBottom = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnLeft = 50;

        roomDetails.hasTopDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasTopDoor = false;
            newRoomDetails.hasRightDoor = false;
            newRoomDetails.hasBottomDoor = true;
            newRoomDetails.hasLeftDoor = false;
            newRoomDetails.distanceFromStart = roomDetails.distanceFromStart + 1;
            newRoomDetails.path = roomDetails.path;
        }
    }
    void spawnRightRoom()
    {
        if(Mathf.Abs(transform.position.x + 18f) / 18 > floorRoomsDetails.MaxHeightOrLength)
        {
            return;
        }
        Vector3 newRoomPosition = new Vector3(transform.position.x + 18f, transform.position.y, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        newRoom.GetComponent<RoomSpawner>().chanceToSpawnTop = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnRight = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnBottom = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnLeft = 50;

        roomDetails.hasRightDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasTopDoor = false;
            newRoomDetails.hasRightDoor = false;
            newRoomDetails.hasBottomDoor = false;
            newRoomDetails.hasLeftDoor = true;
            newRoomDetails.distanceFromStart = roomDetails.distanceFromStart + 1;
            newRoomDetails.path = roomDetails.path;
        }
    }
    void spawnBottomRoom()
    {
        // Check if it is out of bounds
        if(Mathf.Abs(transform.position.y - 10f) / 10 > floorRoomsDetails.MaxHeightOrLength)
        {
            return;
        }

        // Create the Spawn position and instantiate the room there
        Vector3 newRoomPosition = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);

        // Add the new room to the list of all rooms
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        // Make it less likely to spawn in the same direction
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnTop = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnRight = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnBottom = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnLeft = 50;

        // Update Current Room Details
        roomDetails.hasBottomDoor = true;

        // Give the new room a door where it must have it
        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasTopDoor = true;
            newRoomDetails.hasRightDoor = false;
            newRoomDetails.hasBottomDoor = false;
            newRoomDetails.hasLeftDoor = false;
            newRoomDetails.distanceFromStart = roomDetails.distanceFromStart + 1;
            newRoomDetails.path = roomDetails.path;
        }
    }
    void spawnLeftRoom()
    {
        if(Mathf.Abs(transform.position.x - 18f) / 18 > floorRoomsDetails.MaxHeightOrLength)
        {
            return;
        }
        Vector3 newRoomPosition = new Vector3(transform.position.x - 18f, transform.position.y, transform.position.z);
        GameObject newRoom = Instantiate(NewRoom, newRoomPosition, Quaternion.identity);
        floorRoomsDetails.AddRoomToList(newRoom);
        floorRoomsDetails.RoomsToGenerate--;

        newRoom.GetComponent<RoomSpawner>().chanceToSpawnTop = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnRight = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnBottom = 50;
        newRoom.GetComponent<RoomSpawner>().chanceToSpawnLeft = 50;

        roomDetails.hasLeftDoor = true;

        if(newRoom.GetComponent<RoomDetails>()!=null)
        {
            RoomDetails newRoomDetails = newRoom.GetComponent<RoomDetails>();
            newRoomDetails.hasTopDoor = false;
            newRoomDetails.hasRightDoor = true;
            newRoomDetails.hasBottomDoor = false;
            newRoomDetails.hasLeftDoor = false;
            newRoomDetails.distanceFromStart = roomDetails.distanceFromStart + 1;
            newRoomDetails.path = roomDetails.path;
        }

    }

    bool CanSpawnTop()
    {
        Vector2 Top = new Vector2(roomDetails.GridPosition.x, roomDetails.GridPosition.y + 1);
        Vector2 TopRight = new Vector2(roomDetails.GridPosition.x + 1, roomDetails.GridPosition.y + 1);
        Vector2 TopLeft = new Vector2(roomDetails.GridPosition.x - 1, roomDetails.GridPosition.y + 1);
        Vector2 TopTop = new Vector2(roomDetails.GridPosition.x, roomDetails.GridPosition.y + 2);

        foreach(Vector2 Coord in floorRoomsDetails.Grid)
        {
            if(Coord == Top)
            {
                return false;
            }
            if(Coord == TopRight)
            {
                return false;
            }
            if(Coord == TopLeft)
            {
                return false;
            }
            if(Coord == TopTop)
            {
                return false;
            }
        }

        return true;
    }

    bool CanSpawnRight()
    {
        Vector2 Right = new Vector2(roomDetails.GridPosition.x + 1, roomDetails.GridPosition.y);
        Vector2 RightTop = new Vector2(roomDetails.GridPosition.x + 1, roomDetails.GridPosition.y + 1);
        Vector2 RightBottom = new Vector2(roomDetails.GridPosition.x + 1, roomDetails.GridPosition.y - 1);
        Vector2 RightRight = new Vector2(roomDetails.GridPosition.x + 2, roomDetails.GridPosition.y);

        foreach(Vector2 Coord in floorRoomsDetails.Grid)
        {
            if(Coord == Right)
            {
                return false;
            }
            if(Coord == RightTop)
            {
                return false;
            }
            if(Coord == RightBottom)
            {
                return false;
            }
            if(Coord == RightRight)
            {
                return false;
            }
        }

        return true;
    }

    bool CanSpawnBottom()
    {
        Vector2 Bottom = new Vector2(roomDetails.GridPosition.x, roomDetails.GridPosition.y - 1);
        Vector2 BottomRight = new Vector2(roomDetails.GridPosition.x + 1, roomDetails.GridPosition.y - 1);
        Vector2 BottomLeft = new Vector2(roomDetails.GridPosition.x - 1, roomDetails.GridPosition.y - 1);
        Vector2 BottomBottom = new Vector2(roomDetails.GridPosition.x, roomDetails.GridPosition.y - 2);

        foreach(Vector2 Coord in floorRoomsDetails.Grid)
        {
            if(Coord == Bottom)
            {
                return false;
            }
            if(Coord == BottomRight)
            {
                return false;
            }
            if(Coord == BottomLeft)
            {
                return false;
            }
            if(Coord == BottomBottom)
            {
                return false;
            }
        }

        return true;
    }

    bool CanSpawnLeft()
    {
        Vector2 Left = new Vector2(roomDetails.GridPosition.x - 1, roomDetails.GridPosition.y);
        Vector2 LeftTop = new Vector2(roomDetails.GridPosition.x - 1, roomDetails.GridPosition.y + 1);
        Vector2 LeftBottom = new Vector2(roomDetails.GridPosition.x - 1, roomDetails.GridPosition.y - 1);
        Vector2 LeftLeft = new Vector2(roomDetails.GridPosition.x - 2, roomDetails.GridPosition.y);

        foreach(Vector2 Coord in floorRoomsDetails.Grid)
        {
            if(Coord == Left)
            {
                return false;
            }
            if(Coord == LeftTop)
            {
                return false;
            }
            if(Coord == LeftBottom)
            {
                return false;
            }
            if(Coord == LeftLeft)
            {
                return false;
            }
        }

        return true;
    }

    public bool Chance(int x)
    {
        return x > Random.Range(0,100);
    }
}
