using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _toMoveX;
    private float _toMoveY;
    private RoomManager _roomManager;
    private Room _currentRoom;

    public bool CanMoveLeft;
    public bool CanMoveRight;
    public bool CanMoveUp;
    public bool CanMoveDown;
    public bool StairsUp;
    public bool StairsDown;
    public bool DoorRight;
    public bool DoorLeft;
    public string CurrentRoomName;

    // Start is called before the first frame update
    void Start()
    {
        _roomManager = FindObjectOfType<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {        
		var xDir = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Right")) _toMoveX = 1;
        else if (Input.GetButtonDown("Left")) _toMoveX = -1;
        else if (Input.GetButtonDown("Down")) _toMoveY = -1;
        else if (Input.GetButtonDown("Up")) _toMoveY = 1;

        _currentRoom = _roomManager.GetRoomAtPosition(transform.position);
        CurrentRoomName = _currentRoom.Name;
        
        ResetBools();
        FindAvailableMoves();     
    }

    void FixedUpdate()
    {
        
        HandleMovement(_toMoveX, _toMoveY);
        _toMoveX = 0;
        _toMoveY = 0;
    }

    void FindAvailableMoves()
    {
        // Find left
        var leftPosition = (Vector2)transform.position + new Vector2(-1, 0);
        if (_currentRoom.roomSpaces.Contains(leftPosition))
        {
            CanMoveLeft = true;
        }
        else if (_currentRoom.doors.Any(x => x.position == (Vector2)transform.position && x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(leftPosition, doorIsLeft: false))
            {
                CanMoveLeft = true;
                DoorLeft = true;
            }
        }

        // Find right
        var rightPosition = (Vector2)transform.position + new Vector2(1, 0);
        if (_currentRoom.roomSpaces.Contains(rightPosition))
        {
            CanMoveRight = true;
        }
        else if (_currentRoom.doors.Any(x => x.position == (Vector2)transform.position && !x.isLeft))
        {
            if (_roomManager.IsDoorAtPosition(rightPosition, doorIsLeft: true))
            {
                CanMoveRight = true;
                DoorRight = true;
            }
        }

        // Find Up
        Debug.Log("Stairs pos " + _currentRoom.stairs[0].position);
        var upPosition = (Vector2)transform.position + new Vector2(0, 1);
        if (_currentRoom.roomSpaces.Contains(upPosition))
        {
            CanMoveUp = true;
        }
        else if (_currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && x.isUp))
        {
            Debug.Log("Found stairs up");
            if (_roomManager.IsStairAtPosition(upPosition, stairIsUp: false))
            {
                CanMoveUp = true;
                StairsUp = true;
            }
        }
    }

    void HandleMovement(float moveX, float moveY)
    {
        var newPosition = gameObject.transform.position;
        newPosition.x += moveX;
        newPosition.y += moveY;

        if (!IsMoveValid(newPosition)) return;

        gameObject.transform.position = newPosition;
    }

    private bool IsMoveValid(Vector3 newPosition)
    {
        // Player wants to move up or down, check if they are in a stair tile
        if (newPosition.y != gameObject.transform.position.y)
        {
            if (!IsPlayerOnStairs()) return false;
            else return true;
        }
        
        // Player wants to move left or right, check if they are in a doorway or moving in the same room
        else if (newPosition.x != gameObject.transform.position.x)
        {
            if (IsPositionInsideCurrentRoom(newPosition))
            {
                return true;
            }

            if (IsPlayerInDoorway())
            {
                return true;
            }
            
        }
        return false;
    }

    private bool IsPositionInsideCurrentRoom(Vector2 newPosition)
    {
        return true;
    }

    private bool IsPlayerInDoorway()
    {
        return true;
    }

    private bool IsPlayerOnStairs() 
    {
        if (gameObject.transform.position.x == 3.5f) {
            return true;
        }
        else{
            return false;
        }
    }

    void ResetBools()
    {
        CanMoveLeft = false;
        CanMoveDown = false;
        CanMoveRight = false;
        CanMoveUp = false;
        DoorLeft = false;
        DoorRight = false;
        StairsUp = false;
        StairsDown = false;
    }
}
