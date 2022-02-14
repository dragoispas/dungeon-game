using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRoomsDetails : MonoBehaviour
{
    public static bool FinishedGeneration = false;

    [Header("Floor Generation")]
    [Tooltip("Total Numver of rooms to generate")]

    public int RoomsToGenerate;
    int oldRoomsToGenerate;
    int x;

    [Tooltip("e.g. if set to 4, rooms will only be allowed to spawn inside the grid from (4,4) to (-4,-4) - WILL BE AUTOMATICALLY SET THROUGH SCRIPT")]
    public int MaxHeightOrLength;

    [Tooltip("number of extra rooms that will connect different paths")]
    public int extraRooms;

    public int LRoomsToSpawn;




    [Space]
    [Header("Floor Details")]
    [Tooltip("Grid Positions of all Rooms (Basically Coordinates)")]
    public List<Vector2> Grid;

    [HideInInspector]
    public List<GameObject> Rooms; // contains all Rooms
    [Space]
    public List<Vector2> RestrictedPositions; // positions where nothing is allowed

    [Space]
    public List<Vector2> FreePositions; // will contain all free spots next to the rooms



    bool BossRoomChosen = false;
    bool ItemRoomChosen = false;
    int bossPath = 0;


    public GameObject ExtraRoom;
    public GameObject TallRoom;
    public GameObject WideRoom;
    [Space]
    public GameObject LRoomTopLeft;
    public GameObject LRoomTopRight;
    public GameObject LRoomBottomLeft;
    public GameObject LRoomBottomRight;




    bool canSpawnPrimaryExtraRooms = true;

    bool cannotSpawnTallOrWideRoooms = false;

    bool cannotSpawnLRoomTopLeft = false;
    bool cannotSpawnLRoomTopRight = false;
    bool cannotSpawnLRoomBottomLeft = false;
    bool cannotSpawnLRoomBottomRight = false;

    float LChance1 = 100;
    float LChance2 = 100;
    float LChance3 = 100;
    float LChance4 = 100;

    bool cannotSpawnLRooms = false;

    // Start is called before the first frame update
    void Start()
    {
        RoomsToGenerate--; // so that rooms to be spawned will match the number in the editor
        x = 0;
        oldRoomsToGenerate = RoomsToGenerate;
    }

    // Update is called once per frame
    void Update() // this update is composed of 3 blocks that will happen in order  *1* *2* *3*
    {
        // *1* If there are still rooms that need to be spawned, HelpGenerate will re-activate the isolated rooms and make them start random spawning again
        if(RoomsToGenerate > 0)
        {
            if(oldRoomsToGenerate == RoomsToGenerate)
            {
                x++;
            }

            oldRoomsToGenerate = RoomsToGenerate;
            if(x>50)
            {
                HelpGenerate();

                x=0;
            }
        }
        else // *2* everything in this else will happen after the base rooms are spawned 1*
        {
            // Spawn all the rooms that connect different paths
            if(extraRooms > 0)
            {
                if(canSpawnPrimaryExtraRooms == true)
                {
                    spawnExtraRoomPrio();
                }
                else
                {
                    spawnExtraRoom();
                }
                extraRooms--;
            }

            // *3* At this point all of the rooms will have been generated 
            if(extraRooms <= 0)
            {
                if(!BossRoomChosen)
                {
                    ChooseBossRoom();
                    BossRoomChosen = true;
                }

                if(!ItemRoomChosen)
                {
                    ChooseItemRoom();
                    ItemRoomChosen = true;
                }

                addTallAndWideRooms();

                if(cannotSpawnLRooms == false && LRoomsToSpawn > 0)
                {
                    SpawnLRoom();
                }
                else
                {
                    FinishedGeneration = true;
                }

                // *4* finished floor generation
            }

            // create more *special* rooms by finding isolated rooms that are not restricted
        }
    }

    public static Vector2 GetGridPosition(GameObject gameObject)
    {
        Vector2 gridPosition = new Vector2(gameObject.transform.position.x / 18, gameObject.transform.position.y / 10);
        return gridPosition;
    }

    public void AddRoomToList(GameObject Room)
    {
        Rooms.Add(Room);
        Grid.Add(GetGridPosition(Room));
    }

    public bool isOccupied(Vector2 GridPosition)
    {
        foreach(Vector2 coord in Grid)
        {
            if(GridPosition == coord)
            {
                return true;
            }
        }
        return false;
    }

    void HelpGenerate()
    {
        List<GameObject> isoRooms = getIsolatedRooms();
        foreach(GameObject room in isoRooms)
        {
            room.GetComponent<RoomSpawner>().SpawnedRooms = false;
        }
    }

    List<GameObject> getIsolatedRooms()
    {
        List<GameObject> isoRooms = new List<GameObject>();

        foreach(GameObject room in Rooms)
        {
            if(room.GetComponent<RoomDetails>() != null)
            {
                RoomDetails roomDetails = room.GetComponent<RoomDetails>();
                if(roomDetails.isIsolated())
                {
                    isoRooms.Add(room);
                }
            }
        }
        return isoRooms;
    }

    List<Vector2> getIsolatedPositions()
    {
        List<Vector2> isoPositions = new List<Vector2>();
        List<GameObject> isoRooms = getIsolatedRooms();

        foreach(GameObject room in isoRooms)
        {
            isoPositions.Add(getRoomCoords(room));
        }

        return isoPositions;
    }

    void ChooseBossRoom() // this will choose a boss room on the futhest isolated position
    {
        float maxPos = 0f;
        int roomIndex = 0;

        for(int i = 0; i < Rooms.Count ; i++)
        {
            if(Rooms[i].tag == "BasicRoom")
            {
                int xPos = (int)Rooms[i].transform.position.x / 18;
                int yPos = (int)Rooms[i].transform.position.y / 10;
                int currentPos = 0;
                if(xPos > yPos)
                {
                    currentPos = xPos;
                }
                else
                {
                    currentPos = yPos;
                }
                if(Rooms[i].GetComponent<RoomDetails>()!=null)
                {
                    RoomDetails roomDetails = Rooms[i].GetComponent<RoomDetails>();

                    if(roomDetails.isIsolated())
                    {
                        if(currentPos > maxPos)
                        {
                            maxPos = currentPos;
                            roomIndex = i;
                        }
                    }
                }
            }
        }

        Rooms[roomIndex].tag = "BossRoom";
        if(Rooms[roomIndex].GetComponent<RoomDetails>()!=null)
        {
            RoomDetails roomDetails = Rooms[roomIndex].GetComponent<RoomDetails>();
            roomDetails.isBossRoom = true;
            bossPath = roomDetails.path;
            Instantiate(new GameObject("BOSS"), Rooms[roomIndex].transform.position, Quaternion.identity);
        }

        float x = Rooms[roomIndex].transform.position.x / 18;
        float y = Rooms[roomIndex].transform.position.y / 10;
        RestrictPositionsInAreaOf(x,y);
    }

    void ChooseItemRoom() // will not spawn on the same path as the boss // add a requirement that it has to be isolated // if item room doesnt spawn like this, spawn it last next to start room
    {
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails==null)
            {
                continue;
            }

            if(!roomDetails.isIsolated())
            {
                continue;
            }

            if(roomDetails.path == bossPath)
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            roomDetails.isItemRoom = true;
            room.tag = "ItemRoom";
            RestrictedPositions.Add(roomPosition);
            Instantiate(new GameObject("ITEM"), room.transform.position, Quaternion.identity);

            return;
        }
    }

    Vector2 getRoomCoords(GameObject room)
    {
        float x = room.transform.position.x / 18;
        float y = room.transform.position.y / 10;
        Vector2 roomCoords = new Vector2(x,y);
        return roomCoords;
    }

    int getRoomIndexAtCoords(Vector2 checkPos)
    {
        float x = checkPos.x * 18f;
        float y = checkPos.y * 10f;

        int roomIndex = -1;
        for(int i = 0 ; i<Rooms.Count ; i++)
        {
            float xPos = Rooms[i].transform.position.x;
            float yPos = Rooms[i].transform.position.y;

            if(x == xPos && y == yPos)
            {
                roomIndex = i;
                break;
            }
        }

        return roomIndex;
    }

    void UpdateFreePositons()
    {
        FreePositions.Clear();
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails==null)
            {
                continue;
            }
            
            Vector2 roomPosition = getRoomCoords(room);

            Vector2 topPosition = new Vector2(roomPosition.x, roomPosition.y + 1);
            Vector2 rightPosition = new Vector2(roomPosition.x + 1, roomPosition.y);
            Vector2 bottomPosition = new Vector2(roomPosition.x, roomPosition.y - 1);
            Vector2 leftPosition = new Vector2(roomPosition.x - 1, roomPosition.y);
                

            if(!roomDetails.hasTopDoor)
            {
                if(!FreePositions.Contains(topPosition))
                {
                    FreePositions.Add(topPosition);
                }
            }
            else if(roomDetails.hasTopDoor)
            {
                if(FreePositions.Contains(topPosition))
                {
                    FreePositions.Remove(topPosition);
                }
            }

            if(!roomDetails.hasRightDoor)
            {
                if(!FreePositions.Contains(rightPosition))
                {
                    FreePositions.Add(rightPosition);
                }
            }
            else if(roomDetails.hasRightDoor)
            {
                if(FreePositions.Contains(rightPosition))
                {
                    FreePositions.Remove(rightPosition);
                }
            }

            if(!roomDetails.hasBottomDoor)
            {
                if(!FreePositions.Contains(bottomPosition))
                {
                    FreePositions.Add(bottomPosition);
                }
            }
            else if(roomDetails.hasBottomDoor)
            {
                if(FreePositions.Contains(bottomPosition))
                {
                    FreePositions.Remove(bottomPosition);
                }
            }

            if(!roomDetails.hasLeftDoor)
            {
                if(!FreePositions.Contains(leftPosition))
                {
                    FreePositions.Add(leftPosition);
                }
            }
            else if(roomDetails.hasLeftDoor)
            {
                if(FreePositions.Contains(leftPosition))
                {
                    FreePositions.Remove(leftPosition);
                }
            }
        }
        

        // remove the restrictedPositions from freePositions

        foreach(Vector2 position in RestrictedPositions)
        {
            if(FreePositions.Contains(position))
            {
                FreePositions.Remove(position);
            }
        }

        shuffleFreePositionsIndexes();
    }

    void RestrictPositionsInAreaOf(float x, float y)
    {
        Vector2 position1 = new Vector2(x + 1, y);
        Vector2 position2 = new Vector2(x + 1, y + 1);
        Vector2 position3 = new Vector2(x, y + 1);
        Vector2 position4 = new Vector2(x - 1, y);
        Vector2 position5 = new Vector2(x - 1, y - 1);
        Vector2 position6 = new Vector2(x, y - 1);
        Vector2 position7 = new Vector2(x + 1, y - 1);
        Vector2 position8 = new Vector2(x - 1, y + 1);
        
        RestrictedPositions.Add(position1);
        RestrictedPositions.Add(position2);
        RestrictedPositions.Add(position3);
        RestrictedPositions.Add(position4);
        RestrictedPositions.Add(position5);
        RestrictedPositions.Add(position6);
        RestrictedPositions.Add(position7);
        RestrictedPositions.Add(position8);
    }
    
    void spawnExtraRoomPrio() // looks for free positions that are next to 2 rooms that are not connected to each other - spawns an extra room there to connect paths
    {
        UpdateFreePositons();
        Debug.Log("~~~~~~~~~CALLED PRIMARY SPAWN EXTRA ROOMS~~~~~~~~~");
        //check rooms in diagonal !!!!!!!!! foreach free space

        // check top spot and right spot, if there are rooms there, then check top-right spot. If there is a room there. then go next
        foreach(Vector2 freePosition in FreePositions)
        {

            if(getRoomIndexAtCoords(freePosition) != -1)
            {
                //FreePositions.Remove(freePosition);
                continue;
            }

            Vector2 topPosition = new Vector2(freePosition.x, freePosition.y + 1);
            Vector2 rightPosition = new Vector2(freePosition.x + 1, freePosition.y);
            Vector2 bottomPosition = new Vector2(freePosition.x, freePosition.y - 1);
            Vector2 leftPosition = new Vector2(freePosition.x - 1, freePosition.y);
            Vector2 topRightPosition = new Vector2(freePosition.x + 1, freePosition.y + 1);
            Vector2 rightBottomPosition = new Vector2(freePosition.x + 1, freePosition.y - 1);
            Vector2 bottomLeftPosition = new Vector2(freePosition.x - 1, freePosition.y - 1);
            Vector2 leftTopPosition = new Vector2(freePosition.x - 1, freePosition.y + 1);

            int topIndex = getRoomIndexAtCoords(topPosition);
            int rightIndex = getRoomIndexAtCoords(rightPosition);
            int bottomIndex = getRoomIndexAtCoords(bottomPosition);
            int leftIndex = getRoomIndexAtCoords(leftPosition);
            int topRightIndex = getRoomIndexAtCoords(topRightPosition);
            int rightBottomIndex = getRoomIndexAtCoords(rightBottomPosition);
            int bottomLeftIndex = getRoomIndexAtCoords(bottomLeftPosition);
            int leftTopIndex = getRoomIndexAtCoords(leftTopPosition);

            RoomDetails topRoomDetails = null;
            RoomDetails rightRoomDetails = null;
            RoomDetails bottomRoomDetails = null;
            RoomDetails leftRoomDetails = null;
            RoomDetails topRightRoomDetails = null;
            RoomDetails rightBottomRoomDetails = null;
            RoomDetails bottomLeftRoomDetails = null;
            RoomDetails leftTopRoomDetails = null;

            if(topIndex!=-1)
            {
                topRoomDetails = Rooms[topIndex].GetComponent<RoomDetails>();
            }
            if(rightIndex!=-1)
            {
                rightRoomDetails = Rooms[rightIndex].GetComponent<RoomDetails>();
            }
            if(bottomIndex!=-1)
            {
                bottomRoomDetails = Rooms[bottomIndex].GetComponent<RoomDetails>();
            }
            if(leftIndex!=-1)
            {
                leftRoomDetails = Rooms[leftIndex].GetComponent<RoomDetails>();
            }
            if(topRightIndex!=-1)
            {
                topRightRoomDetails = Rooms[topRightIndex].GetComponent<RoomDetails>();
            }
            if(rightBottomIndex!=-1)
            {
                rightBottomRoomDetails = Rooms[rightBottomIndex].GetComponent<RoomDetails>();
            }
            if(bottomLeftIndex!=-1)
            {
                bottomLeftRoomDetails = Rooms[bottomLeftIndex].GetComponent<RoomDetails>();
            }
            if(leftTopIndex!=-1)
            {
                leftTopRoomDetails = Rooms[leftTopIndex].GetComponent<RoomDetails>();
            }

            // Debug.Log("TOP : " + topIndex  + "RIGHT : " + rightIndex + "BOTTOM : " + bottomIndex + "LEFT : " + leftIndex);
            // Debug.Log("TOPright : " + topRightIndex  + "RIGHTbottom : " + rightBottomIndex + "BOTTOMleft : " + bottomLeftIndex + "LEFTtop : " + leftTopIndex);
            if(topIndex != -1 && rightIndex != -1 && topRightIndex == -1)
            {
                // create new room
                Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                newExtraRoom.tag = "ExtraRoom";
                RoomDetails extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                if(extraRoomDetails!=null)
                {
                    // make sure it is set to extra room
                    extraRoomDetails.isExtraRoom = true; 

                    // set doors for extra room
                    extraRoomDetails.hasTopDoor = true;
                    extraRoomDetails.hasRightDoor = true;
                    extraRoomDetails.hasBottomDoor = false;
                    extraRoomDetails.hasLeftDoor = false;
                    if(bottomIndex != -1)
                    {
                        extraRoomDetails.hasBottomDoor = true;
                    }
                    if(leftIndex != -1)
                    {
                        extraRoomDetails.hasLeftDoor = true;
                    }
                }
                //update doors for adjicent rooms
                if(topRoomDetails!=null)
                {
                    topRoomDetails.hasBottomDoor = true;
                }
                if(rightRoomDetails!=null)
                {
                    rightRoomDetails.hasLeftDoor = true;
                }
                if(bottomIndex != -1)
                {
                    bottomRoomDetails.hasTopDoor = true;
                }
                if(leftIndex != -1)
                {
                    leftRoomDetails.hasRightDoor = true;
                }

                Rooms.Add(newExtraRoom);
                Grid.Add(freePosition);
                Debug.Log("(FIRST METHOD)SPAWNED ROOM AT ----> " + freePosition);
                FreePositions.Remove(freePosition);
                extraRooms--;
                //good
                return;
            }
            
            if(rightIndex != -1 && bottomIndex != -1 && rightBottomIndex == -1)
            {
                Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                newExtraRoom.tag = "ExtraRoom";
                RoomDetails extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                if(extraRoomDetails!=null)
                {
                    // make sure it is set to extra room
                    extraRoomDetails.isExtraRoom = true; 

                    // set doors for extra room
                    extraRoomDetails.hasTopDoor = false;
                    extraRoomDetails.hasRightDoor = true;
                    extraRoomDetails.hasBottomDoor = true;
                    extraRoomDetails.hasLeftDoor = false;
                    if(leftIndex != -1)
                    {
                        extraRoomDetails.hasLeftDoor = true;
                    }
                    if(topIndex != -1)
                    {
                        extraRoomDetails.hasTopDoor = true;
                    }
                }
                //update doors for adjicent rooms
                if(rightRoomDetails!=null)
                {
                    rightRoomDetails.hasLeftDoor = true;
                }
                if(bottomRoomDetails!=null)
                {
                    bottomRoomDetails.hasTopDoor = true;
                }
                if(leftIndex != -1)
                {
                    leftRoomDetails.hasRightDoor = true;
                }
                if(topIndex != -1)
                {
                    topRoomDetails.hasBottomDoor = true;
                }

                Rooms.Add(newExtraRoom);
                Grid.Add(freePosition);
                Debug.Log("(FIRST METHOD)SPAWNED ROOM AT ----> " + freePosition);
                FreePositions.Remove(freePosition);
                extraRooms--;
                //good
                return;
            }

            if(bottomIndex != -1 && leftIndex != -1 && bottomLeftIndex == -1)
            {
                Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                newExtraRoom.tag = "ExtraRoom";
                RoomDetails extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                if(extraRoomDetails!=null)
                {
                    // make sure it is set to extra room
                    extraRoomDetails.isExtraRoom = true; 

                    // set doors for extra room
                    extraRoomDetails.hasTopDoor = false;
                    extraRoomDetails.hasRightDoor = false;
                    extraRoomDetails.hasBottomDoor = true;
                    extraRoomDetails.hasLeftDoor = true;
                    if(topIndex != -1)
                    {
                        extraRoomDetails.hasTopDoor = true;
                    }
                    if(rightIndex != -1)
                    {
                        extraRoomDetails.hasRightDoor = true;
                    }
                }
                //update doors for adjicent rooms
                if(bottomRoomDetails!=null)
                {
                    bottomRoomDetails.hasTopDoor = true;
                }
                if(leftRoomDetails!=null)
                {
                    leftRoomDetails.hasRightDoor = true;
                }
                if(topIndex != -1)
                {
                    topRoomDetails.hasBottomDoor = true;
                }
                if(rightIndex != -1)
                {
                    rightRoomDetails.hasLeftDoor = true;
                }

                Rooms.Add(newExtraRoom);
                Grid.Add(freePosition);
                Debug.Log("(FIRST METHOD)SPAWNED ROOM AT ----> " + freePosition);
                FreePositions.Remove(freePosition);
                extraRooms--;
                //good
                return;
            }

            if(leftIndex != -1 && topIndex != -1 && leftTopIndex == -1)
            {
                Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                newExtraRoom.tag = "ExtraRoom";
                RoomDetails extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                if(extraRoomDetails!=null)
                {
                    // make sure it is set to extra room
                    extraRoomDetails.isExtraRoom = true; 

                    // set doors for extra room
                    extraRoomDetails.hasTopDoor = true;
                    extraRoomDetails.hasRightDoor = false;
                    extraRoomDetails.hasBottomDoor = false;
                    extraRoomDetails.hasLeftDoor = true;
                    if(leftIndex != -1)
                    {
                        extraRoomDetails.hasLeftDoor = true;
                    }
                    if(topIndex != -1)
                    {
                        extraRoomDetails.hasTopDoor = true;
                    }
                }
                //update doors for adjicent rooms
                if(leftRoomDetails!=null)
                {
                    leftRoomDetails.hasRightDoor = true;
                }
                if(topRoomDetails!=null)
                {
                    topRoomDetails.hasBottomDoor = true;
                }
                if(rightIndex != -1)
                {
                    rightRoomDetails.hasLeftDoor = true;
                }
                if(bottomIndex != -1)
                {
                    bottomRoomDetails.hasTopDoor = true;
                }

                Rooms.Add(newExtraRoom);
                Grid.Add(freePosition);
                Debug.Log("(FIRST METHOD)SPAWNED ROOM AT ----> " + freePosition);
                FreePositions.Remove(freePosition);
                extraRooms--;
                //good
                return;
            }

        }

        canSpawnPrimaryExtraRooms = false;
    }

    void spawnExtraRoom() // checks for rooms that are 2 spots in any direction and spawns an extra room between them
    {
        UpdateFreePositons();
        Debug.Log("~~~~~~~~~CALLED SECONDARY SPAWN EXTRA ROOMS~~~~~~~~~");

        // check top spot and right spot, if there are rooms there, then check top-right spot. If there is a room there. then go next
        foreach(Vector2 freePosition in FreePositions)
        {

            if(getRoomIndexAtCoords(freePosition) != -1)
            {
                //FreePositions.Remove(freePosition);
                continue;
            }

            Vector2 topPosition = new Vector2(freePosition.x, freePosition.y + 1);
            Vector2 rightPosition = new Vector2(freePosition.x + 1, freePosition.y);
            Vector2 bottomPosition = new Vector2(freePosition.x, freePosition.y - 1);
            Vector2 leftPosition = new Vector2(freePosition.x - 1, freePosition.y);
            Vector2 topRightPosition = new Vector2(freePosition.x + 1, freePosition.y + 1);
            Vector2 rightBottomPosition = new Vector2(freePosition.x + 1, freePosition.y - 1);
            Vector2 bottomLeftPosition = new Vector2(freePosition.x - 1, freePosition.y - 1);
            Vector2 leftTopPosition = new Vector2(freePosition.x - 1, freePosition.y + 1);

            int topIndex = getRoomIndexAtCoords(topPosition);
            int rightIndex = getRoomIndexAtCoords(rightPosition);
            int bottomIndex = getRoomIndexAtCoords(bottomPosition);
            int leftIndex = getRoomIndexAtCoords(leftPosition);
            int topRightIndex = getRoomIndexAtCoords(topRightPosition);
            int rightBottomIndex = getRoomIndexAtCoords(rightBottomPosition);
            int bottomLeftIndex = getRoomIndexAtCoords(bottomLeftPosition);
            int leftTopIndex = getRoomIndexAtCoords(leftTopPosition);

            RoomDetails topRoomDetails = null;
            RoomDetails rightRoomDetails = null;
            RoomDetails bottomRoomDetails = null;
            RoomDetails leftRoomDetails = null;
            RoomDetails topRightRoomDetails = null;
            RoomDetails rightBottomRoomDetails = null;
            RoomDetails bottomLeftRoomDetails = null;
            RoomDetails leftTopRoomDetails = null;

            if(topIndex!=-1)
            {
                topRoomDetails = Rooms[topIndex].GetComponent<RoomDetails>();
            }
            if(rightIndex!=-1)
            {
                rightRoomDetails = Rooms[rightIndex].GetComponent<RoomDetails>();
            }
            if(bottomIndex!=-1)
            {
                bottomRoomDetails = Rooms[bottomIndex].GetComponent<RoomDetails>();
            }
            if(leftIndex!=-1)
            {
                leftRoomDetails = Rooms[leftIndex].GetComponent<RoomDetails>();
            }
            if(topRightIndex!=-1)
            {
                topRightRoomDetails = Rooms[topRightIndex].GetComponent<RoomDetails>();
            }
            if(rightBottomIndex!=-1)
            {
                rightBottomRoomDetails = Rooms[rightBottomIndex].GetComponent<RoomDetails>();
            }
            if(bottomLeftIndex!=-1)
            {
                bottomLeftRoomDetails = Rooms[bottomLeftIndex].GetComponent<RoomDetails>();
            }
            if(leftTopIndex!=-1)
            {
                leftTopRoomDetails = Rooms[leftTopIndex].GetComponent<RoomDetails>();
            }

        if(topIndex!=-1 && bottomIndex!=-1)
            {
                if((rightIndex != -1 && leftIndex == -1) || (rightIndex == -1 && leftIndex != -1) || (rightIndex != -1 && leftIndex != -1))
                {
                    canSpawnPrimaryExtraRooms = true;

                    Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                    GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                    newExtraRoom.tag = "ExtraRoom";
                    RoomDetails extraRoomDetails = null;
                    if(newExtraRoom.GetComponent<RoomDetails>()!=null)
                    {
                        extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                    }
                    extraRoomDetails.isExtraRoom = true; 
                    extraRoomDetails.hasTopDoor = true;
                    extraRoomDetails.hasRightDoor = false;
                    extraRoomDetails.hasBottomDoor = true;
                    extraRoomDetails.hasLeftDoor = false;

                    topRoomDetails.hasBottomDoor = true;
                    bottomRoomDetails.hasTopDoor = true;

                    if(rightIndex!=-1)
                    {
                        extraRoomDetails.hasRightDoor = true;
                        rightRoomDetails.hasLeftDoor = true;
                    }
                    if(leftIndex!=-1)
                    {
                        extraRoomDetails.hasLeftDoor = true;
                        leftRoomDetails.hasRightDoor = true;
                    }
                    Rooms.Add(newExtraRoom);
                    Grid.Add(freePosition);
                    Debug.Log("(SECOND METHOD)SPAWNED ROOM AT ----> " + freePosition);
                    FreePositions.Remove(freePosition);
                    extraRooms--;
                    return;
                }
            }

            if(rightIndex!=-1 && leftIndex!=-1)
            {
                if((topIndex != -1 && bottomIndex == -1) || (topIndex == -1 && bottomIndex != -1) || (topIndex != -1 && bottomIndex != -1))
                {
                    canSpawnPrimaryExtraRooms = true;

                    Vector3 spawnPosition = new Vector3(freePosition.x * 18, freePosition.y * 10, 0);
                    GameObject newExtraRoom = Instantiate(ExtraRoom, spawnPosition, Quaternion.identity);
                    newExtraRoom.tag = "ExtraRoom";
                    RoomDetails extraRoomDetails = null;
                    if(newExtraRoom.GetComponent<RoomDetails>()!=null)
                    {
                        extraRoomDetails = newExtraRoom.GetComponent<RoomDetails>();
                    }
                    extraRoomDetails.isExtraRoom = true; 
                    extraRoomDetails.hasTopDoor = false;
                    extraRoomDetails.hasRightDoor = true;
                    extraRoomDetails.hasBottomDoor = false;
                    extraRoomDetails.hasLeftDoor = true;

                    rightRoomDetails.hasLeftDoor = true;
                    leftRoomDetails.hasRightDoor = true;

                    if(topIndex!=-1)
                    {
                        extraRoomDetails.hasTopDoor = true;
                        topRoomDetails.hasBottomDoor = true;
                    }
                    if(bottomIndex!=-1)
                    {
                        extraRoomDetails.hasBottomDoor = true;
                        bottomRoomDetails.hasTopDoor = true;
                    }
                    Rooms.Add(newExtraRoom);
                    Grid.Add(freePosition);
                    Debug.Log("(SECOND METHOD)SPAWNED ROOM AT ----> " + freePosition);
                    FreePositions.Remove(freePosition);
                    extraRooms--;
                    return;
                }
            }
        }
    }



    int getNumberOfAdjicentRooms(Vector2 position)
    {
        int index = 0;
        if(FreePositions.Contains(position))
        {
            index = Grid.IndexOf(position);
        }
        else
        {
            return -1;
        }

        Vector2 topPosition = new Vector2(position.x, position.y + 1);
        Vector2 rightPosition = new Vector2(position.x + 1, position.y);
        Vector2 bottomPosition = new Vector2(position.x, position.y - 1);
        Vector2 leftPosition = new Vector2(position.x - 1, position.y);
        int topIndex = getRoomIndexAtCoords(topPosition);
        int rightIndex = getRoomIndexAtCoords(rightPosition);
        int bottomIndex = getRoomIndexAtCoords(bottomPosition);
        int leftIndex = getRoomIndexAtCoords(leftPosition);

        // if it finds 2 isolated rooms, then spawn 


        // ~~~~~~~~~~~~~~~~~~~ checks if free space is surrounded by rooms
        int z = 0;
        if(topIndex != -1)
        {
            z++;
        }
        if(rightIndex != -1)
        {
            z++;
        }
        if(bottomIndex != -1)
        {
            z++;
        }
        if(leftIndex != -1)
        {
            z++;
        }

        return z;
    }

    int getNumberOfAdjicentDiagonalRooms(Vector2 position)
    {
        int index = 0;
        if(FreePositions.Contains(position))
        {
            index = Grid.IndexOf(position);
        }
        else
        {
            return -1;
        }

        Vector2 topPosition = new Vector2(position.x + 1, position.y + 1);
        Vector2 rightPosition = new Vector2(position.x + 1, position.y - 1);
        Vector2 bottomPosition = new Vector2(position.x- 1, position.y - 1);
        Vector2 leftPosition = new Vector2(position.x - 1, position.y + 1);
        int topIndex = getRoomIndexAtCoords(topPosition);
        int rightIndex = getRoomIndexAtCoords(rightPosition);
        int bottomIndex = getRoomIndexAtCoords(bottomPosition);
        int leftIndex = getRoomIndexAtCoords(leftPosition);

        // if it finds 2 isolated rooms, then spawn 


        // ~~~~~~~~~~~~~~~~~~~ checks if free space is surrounded by rooms
        int z = 0;
        if(topIndex != -1)
        {
            z++;
        }
        if(rightIndex != -1)
        {
            z++;
        }
        if(bottomIndex != -1)
        {
            z++;
        }
        if(leftIndex != -1)
        {
            z++;
        }

        return z;
    }

    void shuffleFreePositionsIndexes()
    {
        for (int i = 0; i < FreePositions.Count; i++) 
        {
            Vector2 temp = FreePositions[i];
            int randomIndex = Random.Range(i, FreePositions.Count);
            FreePositions[i] = FreePositions[randomIndex];
            FreePositions[randomIndex] = temp;
        }
    }

    



    void addTallAndWideRooms()
    {
        if(cannotSpawnTallOrWideRoooms)
        {
            return;
        }
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails == null)
            {
                continue;
            }
            if(room.tag!="BasicRoom")
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            if(roomDetails.hasOnlyTopAndBottomDoors())
            {
                Vector2 topRoomPosition = new Vector2(roomPosition.x, roomPosition.y + 1);

                if(RestrictedPositions.Contains(topRoomPosition))
                {
                    continue;
                }

                int topRoomIndex = getRoomIndexAtCoords(topRoomPosition);

                if(Rooms[topRoomIndex].GetComponent<RoomDetails>()!=null)
                {
                    RoomDetails topRoomDetails = Rooms[topRoomIndex].GetComponent<RoomDetails>();
                    if(topRoomDetails.hasOnlyTopAndBottomDoors())
                    {
                        float xPos = (room.transform.position.x + Rooms[topRoomIndex].transform.position.x) / 2;
                        float yPos = (room.transform.position.y + Rooms[topRoomIndex].transform.position.y) / 2;
                        Vector3 tallRoomPosition = new Vector3(xPos, yPos, room.transform.position.z);

                        GameObject newTallRoom = Instantiate(TallRoom, tallRoomPosition, Quaternion.identity);
                        newTallRoom.tag = "TallRoom";
                        
                        RestrictedPositions.Add(roomPosition);
                        RestrictedPositions.Add(getRoomCoords(Rooms[topRoomIndex]));

                        room.SetActive(false);
                        Rooms[topRoomIndex].SetActive(false);
                    }
                }
            }

            if(roomDetails.hasOnlyRightAndLeftDoors())
            {
                Vector2 rightRoomPosition = new Vector2(roomPosition.x + 1, roomPosition.y);

                if(RestrictedPositions.Contains(rightRoomPosition))
                {
                    continue;
                }

                int rightRoomIndex = getRoomIndexAtCoords(rightRoomPosition);

                if(Rooms[rightRoomIndex].GetComponent<RoomDetails>()!=null)
                {
                    RoomDetails topRoomDetails = Rooms[rightRoomIndex].GetComponent<RoomDetails>();
                    if(topRoomDetails.hasOnlyRightAndLeftDoors())
                    {
                        float xPos = (room.transform.position.x + Rooms[rightRoomIndex].transform.position.x) / 2;
                        float yPos = (room.transform.position.y + Rooms[rightRoomIndex].transform.position.y) / 2;
                        Vector3 wideRoomPosition = new Vector3(xPos, yPos, room.transform.position.z);

                        GameObject newWideRoom = Instantiate(WideRoom, wideRoomPosition, Quaternion.identity);
                        newWideRoom.tag = "WideRoom";
                        
                        RestrictedPositions.Add(roomPosition);
                        RestrictedPositions.Add(getRoomCoords(Rooms[rightRoomIndex]));

                        room.SetActive(false);
                        Rooms[rightRoomIndex].SetActive(false);
                    }
                }
            }
        }
        cannotSpawnTallOrWideRoooms = true;
    }

    void addLRoomTopLeft()
    {
        if(cannotSpawnLRoomTopLeft)
        {
            return;
        }
        Debug.Log("~~~~~~~~~~~~~~~~~~~STARTED TO ADD L ROOM 1111111~~~~~~~~~~~~~~~~~~");
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails == null)
            {
                continue;
            }
            if(room.tag!="BasicRoom")
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            if(!roomDetails.hasTopDoor)
            {
                continue;
            }
            if(!roomDetails.hasLeftDoor)
            {
                continue;
            }

            Vector2 topRoomPosition = new Vector2(roomPosition.x, roomPosition.y + 1);    //variable
            Vector2 leftRoomPosition = new Vector2(roomPosition.x - 1, roomPosition.y);         //variable

            int topLeftRoomIndex = getRoomIndexAtCoords(new Vector2(roomPosition.x - 1, roomPosition.y + 1));
            if(topLeftRoomIndex!=-1)
            {
                continue;
            }

            if(RestrictedPositions.Contains(topRoomPosition) || RestrictedPositions.Contains(leftRoomPosition))   //variable
            {
                continue;
            }

            int topRoomIndex = getRoomIndexAtCoords(topRoomPosition);   //variable
            int leftRoomIndex = getRoomIndexAtCoords(leftRoomPosition);     //variable

            RoomDetails topRoomDetails = Rooms[topRoomIndex].GetComponent<RoomDetails>();       //variable
            RoomDetails leftRoomDetails = Rooms[leftRoomIndex].GetComponent<RoomDetails>();     //variable

            if(topRoomDetails == null || leftRoomDetails == null)       //variable
            {
                continue;
            }

            if(!Chance(LChance1))
            {
                continue;
            }
            LChance1 = LChance1 / 2;

            GameObject newLRoom = Instantiate(LRoomTopLeft, room.transform.position, Quaternion.identity);  //variable
            newLRoom.tag = "LRoom";

            BigRoomDetails newLRoomDetails = newLRoom.GetComponent<BigRoomDetails>();

            if(newLRoomDetails == null)
            {
                Debug.Log("New L room details was null");
                Destroy(newLRoom);

                continue;
            }

            // set all rooms right

            // first set all to false
            newLRoomDetails.hasDoorTopLeft = false;
            newLRoomDetails.hasDoorTopRight = false;
            newLRoomDetails.hasDoorRightTop = false;
            newLRoomDetails.hasDoorRightBottom = false;
            newLRoomDetails.hasDoorBottomRight = false;
            newLRoomDetails.hasDoorBottomLeft = false;
            newLRoomDetails.hasDoorLeftBottom = false;
            newLRoomDetails.hasDoorLeftTop = false;
            
            // set to true rooms that match with top room       //variable//variable//variable//variable//variable//variable//variable//variable//variable
            if(topRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftTop = true;
            }
            if(topRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopRight = true;
            }
            if(topRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightTop = true;
            }

            // ~/~ centre room
            if(roomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightBottom = true;
            }
            if(roomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomRight = true;
            }

            // ~/~ left room
            if(leftRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomLeft = true;
            }
            if(leftRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftBottom = true;
            }
            if(leftRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopLeft = true;
            }

            RestrictedPositions.Add(roomPosition);   
            RestrictedPositions.Add(getRoomCoords(Rooms[topRoomIndex]));    //variable
            RestrictedPositions.Add(getRoomCoords(Rooms[leftRoomIndex]));   //variable

            room.SetActive(false);
            Rooms[topRoomIndex].SetActive(false);   //variable
            Rooms[leftRoomIndex].SetActive(false);  //variable

            LRoomsToSpawn--;
            Debug.Log("Decreased LRoomsToSpawn to "+LRoomsToSpawn+ " and added this new room : " +newLRoom);
            return;

        }

        cannotSpawnLRoomTopLeft = true;
        
    }

    void addLRoomTopRight()
    {
        if(cannotSpawnLRoomTopRight)
        {
            return;
        }
        Debug.Log("~~~~~~~~~~~~~~~~~~~STARTED TO ADD L ROOMS   2222222222~~~~~~~~~~~~~~~~~~");
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails == null)
            {
                continue;
            }
            if(room.tag!="BasicRoom")
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            if(!roomDetails.hasTopDoor)
            {
                continue;
            }
            if(!roomDetails.hasRightDoor)
            {
                continue;
            }

            Vector2 topRoomPosition = new Vector2(roomPosition.x, roomPosition.y + 1);    //variable
            Vector2 rightRoomPosition = new Vector2(roomPosition.x + 1, roomPosition.y);         //variable

            int topRightRoomIndex = getRoomIndexAtCoords(new Vector2(roomPosition.x + 1, roomPosition.y + 1));
            if(topRightRoomIndex!=-1)
            {
                continue;
            }

            if(RestrictedPositions.Contains(topRoomPosition) || RestrictedPositions.Contains(rightRoomPosition))   //variable
            {
                continue;
            }

            int topRoomIndex = getRoomIndexAtCoords(topRoomPosition);   //variable
            int rightRoomIndex = getRoomIndexAtCoords(rightRoomPosition);     //variable

            RoomDetails topRoomDetails = Rooms[topRoomIndex].GetComponent<RoomDetails>();       //variable
            RoomDetails rightRoomDetails = Rooms[rightRoomIndex].GetComponent<RoomDetails>();     //variable

            if(topRoomDetails == null || rightRoomDetails == null)       //variable
            {
                continue;
            }

            if(!Chance(LChance2))
            {
                continue;
            }
            LChance2 = LChance2 / 2;

            GameObject newLRoom = Instantiate(LRoomTopRight, room.transform.position, Quaternion.identity);  //variable
            newLRoom.tag = "LRoom";

            BigRoomDetails newLRoomDetails = newLRoom.GetComponent<BigRoomDetails>();

            if(newLRoomDetails == null)
            {
                Debug.Log("New L room details was null");
                Destroy(newLRoom);

                continue;
            }

            // set all rooms right

            // first set all to false
            newLRoomDetails.hasDoorTopLeft = false;
            newLRoomDetails.hasDoorTopRight = false;
            newLRoomDetails.hasDoorRightTop = false;
            newLRoomDetails.hasDoorRightBottom = false;
            newLRoomDetails.hasDoorBottomRight = false;
            newLRoomDetails.hasDoorBottomLeft = false;
            newLRoomDetails.hasDoorLeftBottom = false;
            newLRoomDetails.hasDoorLeftTop = false;
            
            // set to true rooms that match with top room       //variable//variable//variable//variable//variable//variable//variable//variable//variable
            if(topRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftTop = true;
            }
            if(topRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopLeft = true;
            }
            if(topRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightTop = true;
            }

            // ~/~ centre room
            if(roomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftBottom = true;
            }
            if(roomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomLeft = true;
            }

            // ~/~ right room
            if(rightRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomRight = true;
            }
            if(rightRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightBottom = true;
            }
            if(rightRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopRight = true;
            }

            RestrictedPositions.Add(roomPosition);   
            RestrictedPositions.Add(getRoomCoords(Rooms[topRoomIndex]));    //variable
            RestrictedPositions.Add(getRoomCoords(Rooms[rightRoomIndex]));   //variable

            room.SetActive(false);
            Rooms[topRoomIndex].SetActive(false);   //variable
            Rooms[rightRoomIndex].SetActive(false);  //variable

            LRoomsToSpawn--;
            Debug.Log("Decreased LRoomsToSpawn to "+LRoomsToSpawn+ " and added this new room : " +newLRoom);
            return;

        }

        cannotSpawnLRoomTopRight = true;
        
    }

    void addLRoomBottomRight()
    {
        if(cannotSpawnLRoomBottomRight)
        {
            return;
        }
        Debug.Log("~~~~~~~~~~~~~~~~~~~STARTED TO ADD L ROOMS    3333333333~~~~~~~~~~~~~~~~~~");
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails == null)
            {
                continue;
            }
            if(room.tag!="BasicRoom")
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            if(!roomDetails.hasBottomDoor)
            {
                continue;
            }
            if(!roomDetails.hasRightDoor)
            {
                continue;
            }

            Vector2 bottomRoomPosition = new Vector2(roomPosition.x, roomPosition.y - 1);    //variable
            Vector2 rightRoomPosition = new Vector2(roomPosition.x + 1, roomPosition.y);         //variable

            int bottomRightRoomIndex = getRoomIndexAtCoords(new Vector2(roomPosition.x + 1, roomPosition.y - 1));
            if(bottomRightRoomIndex!=-1)
            {
                continue;
            }

            if(RestrictedPositions.Contains(bottomRoomPosition) || RestrictedPositions.Contains(rightRoomPosition))   //variable
            {
                continue;
            }

            int bottomRoomIndex = getRoomIndexAtCoords(bottomRoomPosition);   //variable
            int rightRoomIndex = getRoomIndexAtCoords(rightRoomPosition);     //variable

            RoomDetails bottomRoomDetails = Rooms[bottomRoomIndex].GetComponent<RoomDetails>();       //variable
            RoomDetails rightRoomDetails = Rooms[rightRoomIndex].GetComponent<RoomDetails>();     //variable

            if(bottomRoomDetails == null || rightRoomDetails == null)       //variable
            {
                continue;
            }

            if(!Chance(LChance3))
            {
                continue;
            }
            LChance3 = LChance3 / 2;

            GameObject newLRoom = Instantiate(LRoomBottomRight, room.transform.position, Quaternion.identity);  //variable
            newLRoom.tag = "LRoom";

            BigRoomDetails newLRoomDetails = newLRoom.GetComponent<BigRoomDetails>();

            if(newLRoomDetails == null)
            {
                Debug.Log("New L room details was null");
                Destroy(newLRoom);

                continue;
            }

            // set all rooms right

            // first set all to false
            newLRoomDetails.hasDoorTopLeft = false;
            newLRoomDetails.hasDoorTopRight = false;
            newLRoomDetails.hasDoorRightTop = false;
            newLRoomDetails.hasDoorRightBottom = false;
            newLRoomDetails.hasDoorBottomRight = false;
            newLRoomDetails.hasDoorBottomLeft = false;
            newLRoomDetails.hasDoorLeftBottom = false;
            newLRoomDetails.hasDoorLeftTop = false;
            
            // set to true rooms that match with bottom room       //variable//variable//variable//variable//variable//variable//variable//variable//variable
            if(bottomRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftBottom = true;
            }
            if(bottomRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomLeft = true;
            }
            if(bottomRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightBottom = true;
            }

            // ~/~ centre room
            if(roomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopLeft = true;
            }
            if(roomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftTop = true;
            }

            // ~/~ right room
            if(rightRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomRight = true;
            }
            if(rightRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightTop = true;
            }
            if(rightRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopRight = true;
            }

            RestrictedPositions.Add(roomPosition);   
            RestrictedPositions.Add(getRoomCoords(Rooms[bottomRoomIndex]));    //variable
            RestrictedPositions.Add(getRoomCoords(Rooms[rightRoomIndex]));   //variable

            room.SetActive(false);
            Rooms[bottomRoomIndex].SetActive(false);   //variable
            Rooms[rightRoomIndex].SetActive(false);  //variable

            LRoomsToSpawn--;
            Debug.Log("Decreased LRoomsToSpawn to "+LRoomsToSpawn+ " and added this new room : " +newLRoom);
            return;

        }

        cannotSpawnLRoomBottomRight = true;
        
    }

    void addLRoomBottomLeft()
    {
        if(cannotSpawnLRoomBottomLeft)
        {
            return;
        }
        Debug.Log("~~~~~~~~~~~~~~~~~~~STARTED TO ADD L ROOMS     4444444444444~~~~~~~~~~~~~~~~~~");
        foreach(GameObject room in Rooms)
        {
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            if(roomDetails == null)
            {
                continue;
            }
            if(room.tag!="BasicRoom")
            {
                continue;
            }

            Vector2 roomPosition = getRoomCoords(room);
            if(RestrictedPositions.Contains(roomPosition))
            {
                continue;
            }

            if(!roomDetails.hasBottomDoor)
            {
                continue;
            }
            if(!roomDetails.hasLeftDoor)
            {
                continue;
            }

            Vector2 bottomRoomPosition = new Vector2(roomPosition.x, roomPosition.y - 1);    //variable
            Vector2 leftRoomPosition = new Vector2(roomPosition.x - 1, roomPosition.y);         //variable

            int bottomLeftRoomIndex = getRoomIndexAtCoords(new Vector2(roomPosition.x - 1, roomPosition.y - 1));
            if(bottomLeftRoomIndex!=-1)
            {
                continue;
            }

            if(RestrictedPositions.Contains(bottomRoomPosition) || RestrictedPositions.Contains(leftRoomPosition))   //variable
            {
                continue;
            }

            int bottomRoomIndex = getRoomIndexAtCoords(bottomRoomPosition);   //variable
            int leftRoomIndex = getRoomIndexAtCoords(leftRoomPosition);     //variable

            RoomDetails bottomRoomDetails = Rooms[bottomRoomIndex].GetComponent<RoomDetails>();       //variable
            RoomDetails leftRoomDetails = Rooms[leftRoomIndex].GetComponent<RoomDetails>();     //variable

            if(bottomRoomDetails == null || leftRoomDetails == null)       //variable
            {
                continue;
            }

            if(!Chance(LChance4))
            {
                continue;
            }
            LChance4 = LChance4 / 2;


            GameObject newLRoom = Instantiate(LRoomBottomLeft, room.transform.position, Quaternion.identity);  //variable
            newLRoom.tag = "LRoom";

            BigRoomDetails newLRoomDetails = newLRoom.GetComponent<BigRoomDetails>();

            if(newLRoomDetails == null)
            {
                Debug.Log("New L room details was null");
                Destroy(newLRoom);

                continue;
            }

            // set all rooms right

            // first set all to false
            newLRoomDetails.hasDoorTopLeft = false;
            newLRoomDetails.hasDoorTopRight = false;
            newLRoomDetails.hasDoorRightTop = false;
            newLRoomDetails.hasDoorRightBottom = false;
            newLRoomDetails.hasDoorBottomRight = false;
            newLRoomDetails.hasDoorBottomLeft = false;
            newLRoomDetails.hasDoorLeftBottom = false;
            newLRoomDetails.hasDoorLeftTop = false;
            
            // set to true rooms that match with bottom room       //variable//variable//variable//variable//variable//variable//variable//variable//variable
            if(bottomRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftBottom = true;
            }
            if(bottomRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomRight = true;
            }
            if(bottomRoomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightBottom = true;
            }

            // ~/~ centre room
            if(roomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopRight = true;
            }
            if(roomDetails.hasRightDoor)
            {
                newLRoomDetails.hasDoorRightTop = true;
            }

            // ~/~ left room
            if(leftRoomDetails.hasBottomDoor)
            {
                newLRoomDetails.hasDoorBottomLeft = true;
            }
            if(leftRoomDetails.hasLeftDoor)
            {
                newLRoomDetails.hasDoorLeftTop = true;
            }
            if(leftRoomDetails.hasTopDoor)
            {
                newLRoomDetails.hasDoorTopLeft= true;
            }

            RestrictedPositions.Add(roomPosition);   
            RestrictedPositions.Add(getRoomCoords(Rooms[bottomRoomIndex]));    //variable
            RestrictedPositions.Add(getRoomCoords(Rooms[leftRoomIndex]));   //variable

            room.SetActive(false);
            Rooms[bottomRoomIndex].SetActive(false);   //variable
            Rooms[leftRoomIndex].SetActive(false);  //variable

            LRoomsToSpawn--;
            Debug.Log("Decreased LRoomsToSpawn to "+LRoomsToSpawn+ " and added this new room : " +newLRoom);
            return;

        }

        cannotSpawnLRoomBottomLeft = true;
        
    }

    void SpawnLRoom()
    {
        if(cannotSpawnLRoomTopLeft && cannotSpawnLRoomTopRight && cannotSpawnLRoomBottomRight && cannotSpawnLRoomBottomLeft)
        {
            Debug.Log("Cannot spawn any more L Rooms");
            cannotSpawnLRooms = true;
            return;
        }
        int x = Random.Range(1,5);
        if(cannotSpawnLRoomTopLeft)
        {
            Debug.Log("Cannot spawn 111111111111111111111111");
            while(x == 1)
            {
                x = Random.Range(1,5);
            }
        }
        else if(cannotSpawnLRoomTopRight)
        {
            Debug.Log("Cannot spawn 22222222222222222222222");
            while(x == 2)
            {
                x = Random.Range(1,5);
            }
        }
        else if(cannotSpawnLRoomBottomRight)
        {
            Debug.Log("Cannot spawn 333333333333333333333");
            while(x == 3)
            {
                x = Random.Range(1,5);
            }
        }
        else if(cannotSpawnLRoomBottomLeft)
        {
            Debug.Log("Cannot spawn 44444444444444444444");
            while(x == 4)
            {
                x = Random.Range(1,5);
            }
        }
        if(x==1)
        {
            addLRoomTopLeft();
            return;
        }
        if(x==2)
        {
            addLRoomTopRight();
            return;
        }
        if(x==3)
        {
            addLRoomBottomRight();
            return;
        }
        if(x==4)
        {
            addLRoomBottomLeft();
            return;
        }
    }

    public bool Chance(float x)
    {
        return x > Random.Range(0,100);
    }

}
