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

    private void FindAvailableMoves()
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
        var upPosition = (Vector2)transform.position + new Vector2(0, 1);
        if (_currentRoom.roomSpaces.Contains(upPosition))
        {
            CanMoveUp = true;
        }
        else if (_currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && x.isUp))
        {
            if (_roomManager.IsStairAtPosition(upPosition, stairIsUp: false))
            {
                CanMoveUp = true;
                StairsUp = true;
            }
        }

        // Find Bottom
        var downPosition = (Vector2)transform.position + new Vector2(0, -1);
        if (_currentRoom.roomSpaces.Contains(downPosition))
        {
            CanMoveDown = true;
        }
        else if (_currentRoom.stairs.Any(x => x.position == (Vector2)transform.position && !x.isUp))
        {
            if (_roomManager.IsStairAtPosition(downPosition, stairIsUp: true))
            {
                CanMoveDown = true;
                StairsDown = true;
            }
        }
    }

    void HandleMovement(float moveX, float moveY)
    {
        if (!IsMoveValid()) return;
        
        var newPosition = gameObject.transform.position;
        newPosition.x += moveX;
        newPosition.y += moveY;

        gameObject.transform.position = newPosition;
    }

    private bool IsMoveValid()
    {
        if (_toMoveX == 1 && CanMoveRight) 
        {
            return true;
        }
        else if (_toMoveX == -1 && CanMoveLeft)
        {
            return true;
        }
        else if (_toMoveY == 1 && CanMoveUp)
        {
            return true;
        }
        else if (_toMoveY == -1 && CanMoveDown)
        {
            return true;
        }

        return false;
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
