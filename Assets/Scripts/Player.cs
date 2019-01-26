﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RoomManager _roomManager;
    private Room _currentRoom;

    public SpriteRenderer PlayerSprite;
    private Vector3 _playerSpriteOriginalPosition;
    
    public bool CanMoveLeft;
    public bool CanMoveRight;
    public bool CanMoveUp;
    public bool CanMoveDown;
    public bool StairsUp;
    public bool StairsDown;
    public bool DoorRight;
    public bool DoorLeft;
    public string CurrentRoomName;

    public int MoveFrames;
    public bool IsMoving;
    public bool MovingDisabled;

    // Start is called before the first frame update
    void Start()
    {
        _roomManager = FindObjectOfType<RoomManager>();
        _playerSpriteOriginalPosition = PlayerSprite.transform.localPosition;

        UpdateCurrentRoom();
    }

    // Update is called once per frame
    void Update()
    {     
        if(!IsMoving && !MovingDisabled)
        {
            var xDir = Input.GetAxis("Horizontal");
            var yDir = Input.GetAxis("Vertical");

            int moveX = 0;
            int moveY = 0;

            if (xDir > 0) moveX = 1;
            else if (xDir < 0) moveX = -1;
            else if (yDir < 0) moveY = -1;
            else if (yDir > 0) moveY = 1;

            StartCoroutine(HandleMovement(moveX, moveY));
        }

        UpdateCurrentRoom();
    }

    private void UpdateCurrentRoom()
    {
        _currentRoom = _roomManager.GetRoomAtPosition(transform.position);
        CurrentRoomName = _currentRoom.Name;

        ResetBools();
        FindAvailableMoves();
    }

    IEnumerator HandleMovement(int moveX, int moveY)
    {
        if (IsMoving ||
            (moveX == 0 && moveY == 0) ||
            !IsMoveValid(moveX, moveY))
        {
            yield break;
        }

        IsMoving = true;

        float moveXPerFrame = moveX / (float)MoveFrames;
        float moveYPerFrame = moveY / (float)MoveFrames;

        for (int i = 0; i < MoveFrames; i++)
        {
            PlayerSprite.transform.localPosition += new Vector3
            (
                moveXPerFrame,
                moveYPerFrame
            );

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        transform.position += new Vector3
        (
            moveX,
            moveY
        );

        PlayerSprite.transform.localPosition = _playerSpriteOriginalPosition;

        UpdateCurrentRoom();
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

    private bool IsMoveValid(int moveX, int moveY)
    {
        if (moveX == 1 && CanMoveRight) 
        {
            return true;
        }
        else if (moveX == -1 && CanMoveLeft)
        {
            return true;
        }
        else if (moveY == 1 && CanMoveUp)
        {
            return true;
        }
        else if (moveY == -1 && CanMoveDown)
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
