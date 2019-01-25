using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _toMoveX;
    private float _toMoveY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
		var xDir = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Right")) _toMoveX = 1;
        else if (Input.GetButtonDown("Left")) _toMoveX = -1;
        else if (Input.GetButtonDown("Down")) _toMoveY = -1;
        else if (Input.GetButtonDown("Up")) _toMoveY = 1;        
    }

    void FixedUpdate()
    {
        
        HandleMovement(_toMoveX, _toMoveY);
        _toMoveX = 0;
        _toMoveY = 0;
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
}
